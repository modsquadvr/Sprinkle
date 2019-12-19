using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationGUI : MonoBehaviour
{
    [Tooltip("GPS accuracy in meters")]
    public float granularity = 10;
    [Tooltip("Meters moved to update the location.")]
    public float updateDistance = 30;
    public bool pollLocation = false;

    public Text toggleText;
    public Text latitude;
    public Text longitude;
    public Text altitude;
    public Text horizontalAccuracy;
    public Text timestamp;

    public InputField granularityField;
    public InputField distanceField;


    private void Awake()
    {
        granularityField.text = granularity.ToString();
        distanceField.text = updateDistance.ToString();
    }

    public void UpdateGranularity()
    {
        float g = granularity;
        if(!float.TryParse(granularityField.text, out granularity))
        {
            granularity = g;
            granularityField.text = granularity.ToString();
        }
    }

    public void UpdateDistance()
    {
        float d = updateDistance;
        if (!float.TryParse(distanceField.text, out updateDistance))
        {
            updateDistance = d;
            distanceField.text = updateDistance.ToString();
        }
    }

    public void TogglePolling()
    {
        pollLocation = !pollLocation;
    }

    public void ToggleLocation()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            toggleText.text = "Show Location";
        }
        else
        {
            gameObject.SetActive(true);
            StartCoroutine(GetLocation());
            toggleText.text = "Hide Location";
        }
    }

    IEnumerator GetLocation()
    {
        Debug.Log("Get location");
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service before querying location
        Input.location.Start(granularity, updateDistance);

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {

            // Access granted and location value could be retrieved
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        // Stop service if there is no need to query location updates continuously
        if(!pollLocation)
            Input.location.Stop();

        latitude.text = Input.location.lastData.latitude.ToString();
        longitude.text = Input.location.lastData.longitude.ToString();
        altitude.text = Input.location.lastData.altitude.ToString();
        horizontalAccuracy.text = Input.location.lastData.horizontalAccuracy.ToString();
        timestamp.text = Input.location.lastData.timestamp.ToString();
    }
}
