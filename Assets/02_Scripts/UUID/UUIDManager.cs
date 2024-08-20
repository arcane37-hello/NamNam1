using UnityEngine;

public class UUIDManager : MonoBehaviour
{
    private const string UUID_KEY = "DeviceUUID";

    public static string GetUUID()
    {
        string uuid = PlayerPrefs.GetString(UUID_KEY, string.Empty);
        if (string.IsNullOrEmpty(uuid))
        {
            uuid = SystemInfo.deviceUniqueIdentifier;
            PlayerPrefs.SetString(UUID_KEY, uuid);
            PlayerPrefs.Save();
        }
        return uuid;
    }
}