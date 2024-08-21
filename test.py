import cv2
import mediapipe as mp
from PIL import ImageFont, ImageDraw, Image
import numpy as np
from fastapi import FastAPI
from fastapi.responses import JSONResponse
import threading
import collections

app = FastAPI()

# 변수 초기화
previous_distance_ratio = None
chewing_threshold = 0.02  # 임계값 (조정 가능)
chewing_count = 0  # 씹는 동작 횟수
current_chewing_count = 0  # 전역 변수로 씹는 횟수를 저장
is_mouth_open = False  # 입이 열렸는지 닫혔는지 여부를 저장하는 플래그
ratio_history = collections.deque(maxlen=10)  # 최근 10개의 비율을 저장

# 나눔고딕 등 시스템에 설치된 한글 폰트의 경로를 지정하세요.
fontpath = "C:/mnproject/NanumGothicLight.ttf"
font = ImageFont.truetype(fontpath, 32)

def is_face_frontal(face_landmarks, img_width, img_height):
    """ 얼굴이 정면을 바라보고 있는지 판단 """
    left_cheek = face_landmarks.landmark[234]  # 왼쪽 광대뼈
    right_cheek = face_landmarks.landmark[454]  # 오른쪽 광대뼈
    nose_tip = face_landmarks.landmark[1]  # 코끝

    left_cheek_coords = np.array([left_cheek.x * img_width, left_cheek.y * img_height])
    right_cheek_coords = np.array([right_cheek.x * img_width, right_cheek.y * img_height])
    nose_tip_coords = np.array([nose_tip.x * img_width, nose_tip.y * img_height])

    # 코끝과 양쪽 광대뼈 간의 거리 차이를 확인하여 대칭성을 평가
    left_distance = np.linalg.norm(left_cheek_coords - nose_tip_coords)
    right_distance = np.linalg.norm(right_cheek_coords - nose_tip_coords)

    # 거리 차이가 작으면 얼굴이 정면을 바라보고 있다고 판단
    symmetry_threshold = 0.05 * img_width  # 대칭성을 판단하는 임계값
    return abs(left_distance - right_distance) < symmetry_threshold

def detect_chewing():
    global previous_distance_ratio, chewing_threshold, chewing_count, current_chewing_count, is_mouth_open, ratio_history

    cap = cv2.VideoCapture(0)

    mp_face_mesh = mp.solutions.face_mesh
    face_mesh = mp_face_mesh.FaceMesh()

    while cap.isOpened():
        ret, frame = cap.read()

        if not ret:
            break

        # 화면을 좌우 반전(플립)합니다.
        frame = cv2.flip(frame, 1)

        frame.flags.writeable = False
        results = face_mesh.process(cv2.cvtColor(frame, cv2.COLOR_BGR2RGB))
        frame.flags.writeable = True

        img_height, img_width, _ = frame.shape

        if results.multi_face_landmarks:
            for face_landmarks in results.multi_face_landmarks:
                if is_face_frontal(face_landmarks, img_width, img_height):
                    # 이마점, 코끝, 턱끝의 좌표 계산
                    forehead = face_landmarks.landmark[10]  # 이마 중앙점 (임의로 인덱스 10번 선택)
                    nose_tip = face_landmarks.landmark[1]   # 코끝
                    chin_tip = face_landmarks.landmark[152]  # 턱끝

                    # 화면 좌표로 변환
                    forehead_coords = (int(forehead.x * img_width), int(forehead.y * img_height))
                    nose_tip_coords = (int(nose_tip.x * img_width), int(nose_tip.y * img_height))
                    chin_tip_coords = (int(chin_tip.x * img_width), int(chin_tip.y * img_height))

                    # 선1: 이마점과 코끝 사이의 거리
                    line1_length = np.linalg.norm(np.array(forehead_coords) - np.array(nose_tip_coords))
                    # 선2: 코끝과 턱끝 사이의 거리
                    line2_length = np.linalg.norm(np.array(nose_tip_coords) - np.array(chin_tip_coords))

                    # 선1에 비례한 선2의 거리 비율 계산
                    distance_ratio = line2_length / line1_length

                    # 비율을 히스토리에 저장
                    ratio_history.append(distance_ratio)

                    # 히스토리의 평균값을 기준으로 삼음
                    average_ratio = np.mean(ratio_history)

                    if previous_distance_ratio is not None:
                        # 현재 비율과 기준 비율의 차이 계산
                        ratio_change = distance_ratio - average_ratio

                        if ratio_change > chewing_threshold:
                            if not is_mouth_open:
                                chewing_count += 1
                                current_chewing_count = chewing_count
                                is_mouth_open = True
                                print(f"씹고 있습니다. - 씹는 횟수: {chewing_count}")
                                if chewing_count >= 30:
                                    chewing_count = 0
                                    print("씹는 횟수가 30회에 도달하여 초기화되었습니다.")
                        elif ratio_change < -chewing_threshold:
                            if is_mouth_open:
                                is_mouth_open = False
                                print("입이 다물어졌습니다. (플래그 리셋)")

                    previous_distance_ratio = distance_ratio

                    # 디버그: 이마점, 코끝, 턱끝 위치 표시
                    cv2.circle(frame, forehead_coords, 2, (255, 0, 0), -1)
                    cv2.circle(frame, nose_tip_coords, 2, (0, 0, 255), -1)
                    cv2.circle(frame, chin_tip_coords, 2, (0, 255, 0), -1)

        # 씹는 횟수를 화면에 표시
        pil_img = Image.fromarray(cv2.cvtColor(frame, cv2.COLOR_BGR2RGB))
        draw = ImageDraw.Draw(pil_img)
        draw.text((50, 100), f"씹는 횟수: {chewing_count}", font=font, fill=(0, 255, 0, 0))
        frame = cv2.cvtColor(np.array(pil_img), cv2.COLOR_RGB2BGR)

        cv2.imshow('Chewing Detection', frame)
        if cv2.waitKey(1) == 27:  # ESC 키를 누르면 종료
            break

    cap.release()
    cv2.destroyAllWindows()

@app.get("/chewing_count")
async def get_chewing_count():
    # JSON 형식으로 현재 씹는 횟수를 반환합니다.
    return JSONResponse(content={"chewing_count": current_chewing_count})

if __name__ == "__main__":
    # OpenCV 루프를 별도의 스레드에서 실행
    threading.Thread(target=detect_chewing, daemon=True).start()

    # FastAPI 서버 시작
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)
