using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TimeUpdatedEventHandler(DateTime currentTime);

public class TimeSyncManager : MonoBehaviour, ITimeSyncManager
{
    private List<ITimeService> timeServices;
    private List<IJsonTimeParser> jsonParsers;
    private DateTime syncedTime = DateTime.MinValue;
    private float timeSinceLastSync = 0,
        syncRepeatingDelay = 3600f;
    private int currentServiceIndex = 0;
    private string getIpUrl = "https://icanhazip.com/";
    public event TimeUpdatedEventHandler OnTimeUpdated;

    private void Start()
    {
        timeServices = new List<ITimeService>
        {
            new TimeAPIService(getIpUrl, "https://www.timeapi.io/api/timezone/ip?ipAddress="),
            new TimeAPIService(getIpUrl, "http://worldtimeapi.org/api/ip/")
        };
        jsonParsers = new List<IJsonTimeParser>()
        {
            new DefaultTimeParser()
        };

        InvokeRepeating(nameof(Synchronize), 0, syncRepeatingDelay);
    }

    private IEnumerator SyncTime()
    {
        if (timeServices.Count > 0)
        {
            if (currentServiceIndex < timeServices.Count)
            {
                ITimeService currentService = timeServices[currentServiceIndex];
                yield return currentService.GetApiResult((json) =>
                {
                    if (!string.IsNullOrEmpty(json))
                    {
                        foreach (var parser in jsonParsers)
                        {
                            if (parser.TryGetParsedTime(json, out var parsedTime))
                            {
                                UpdateClockWithSyncedTime(parsedTime);
                                break;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("An error occurred when getting the time.");
                        currentServiceIndex++;
                        StartCoroutine(SyncTime());
                    }
                });
            }
            else
                UsingDeviceTime("There is no connection to the services, we use the device time.");
        }
        else
            UsingDeviceTime("There are no time services available, we use the device time.");

        void UsingDeviceTime(string errorMessage)
        {
            UpdateClockWithSyncedTime(DateTime.Now);
            Debug.LogError(errorMessage);
        }
    }

    private void UpdateClockWithSyncedTime(DateTime syncTime)
    {
        syncedTime = syncTime;
        timeSinceLastSync = 0;
        NotifyObservers(syncedTime);
    }

    private void Update()
    {
        timeSinceLastSync += Time.deltaTime;

        if (syncedTime != DateTime.MinValue)
        {
            DateTime currentTime = syncedTime.AddSeconds(timeSinceLastSync);
            NotifyObservers(currentTime);
        }
    }

    private void Synchronize()
    {
        Debug.Log("Start synchronize time.");
        currentServiceIndex = 0;
        StartCoroutine(SyncTime());
    }

    private void NotifyObservers(DateTime currentTime)
    {
        OnTimeUpdated?.Invoke(currentTime);
    }
}