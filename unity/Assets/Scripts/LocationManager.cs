using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LocationManager : MonoBehaviour
{
    public static LocationManager instance;

    public string cur;

#if UNITY_ANDROID


    public Dictionary<string, string> locationSSIDs = new Dictionary<string, string>();

    public float delay = 30f;

    int api_v;
    AndroidJavaObject mWiFiManager;
#endif

    private void Awake()
    {
        if(instance != null)
        {
            DestroyImmediate(instance);
            return;
        }
        else
        {
            instance = this;
        }
        
#if UNITY_ANDROID

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
#endif

    }

#if UNITY_ANDROID
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
                    break;
            }

            yield return new WaitForSeconds(delay);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if(focus)
        {
            //stop service that checks wifi ssids
        }
        else
        {
            //start service that checks wifi ssids
        }
    }
#endif

}
