using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.NetworkInformation;

public class LocationManager : MonoBehaviour
{
    public string cur;

#if UNITY_ANDROID

    public Dictionary<string, string> locationSSIDs = new Dictionary<string, string>();

    public float delay = 30f;

    AndroidJavaObject mWiFiManager;

    private void Awake()
    {
        using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
        {
            mWiFiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi");
        }

        AndroidJavaObject wifiInfo = mWiFiManager.Call<AndroidJavaObject>("getConnectionInfo");
        string curSSID = wifiInfo.Call<string>("getSSID");
        //remove "" from string beginning and ending
        curSSID = curSSID.Substring(1, curSSID.Length - 2);

        Debug.Log("Current SSID: " + curSSID);

        locationSSIDs.Add(curSSID, "currentLocation");

        StartCoroutine(PollSSIDs());
    }

    List<string> GetSSIDs()
    {
        List<string> ssids = new List<string>();


        AndroidJavaObject results = mWiFiManager.Call<AndroidJavaObject>("getScanResults");
        int length = results.Call<int>("size");

        Debug.Log("got " + length + " ssids");

        for (int i = 0; i < length; i++)
        {
            AndroidJavaObject scanResult = results.Call<AndroidJavaObject>("get", i);
            ssids.Add(scanResult.Get<string>("SSID"));
        }

        return ssids;
    }

    IEnumerator PollSSIDs()
    {
        Debug.Log("polling for ssids");

        if (!mWiFiManager.Call<bool>("isWifiEnabled"))
            Debug.Log("wifi is disabled");

        while (true)
        {
            while (!mWiFiManager.Call<bool>("isWifiEnabled"))
                yield return new WaitForSeconds(delay);

            List<string> ssids = GetSSIDs();

            for (int i = 0; i < ssids.Count; i++)
            {
                Debug.Log(ssids[i]);

                if (locationSSIDs.TryGetValue(ssids[i], out cur))
                {
                    Debug.Log("Setting location: " + cur);
                    break;
                }
            }

            yield return new WaitForSeconds(delay);
        }
    }
#endif

}
