using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TimeAPIService : ITimeService
{
    public string TimeServiceURL { get; }
    public string IPServiceURL { get; }

    public TimeAPIService(string IPServiceURL, string TimeServiceURL)
    {
        this.TimeServiceURL = TimeServiceURL;
        this.IPServiceURL = IPServiceURL;
    }

    public IEnumerator GetApiResult(Action<string> onTimeReceived)
    {
        if (!string.IsNullOrEmpty(TimeServiceURL) || !string.IsNullOrEmpty(IPServiceURL))
        {
            using (UnityWebRequest getIPRequest = UnityWebRequest.Get(IPServiceURL))
            {
                yield return getIPRequest.SendWebRequest();
                if (getIPRequest.result == UnityWebRequest.Result.Success)
                {
                    var ip = getIPRequest.downloadHandler.text;
                    var requestUrl = $"{TimeServiceURL}{getIPRequest.downloadHandler.text}";
                    using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
                    {
                        yield return request.SendWebRequest();

                        if (request.result == UnityWebRequest.Result.Success)
                        {
                            onTimeReceived?.Invoke(request.downloadHandler.text);
                        }
                        else
                            OnError($"Error getting time from {requestUrl}: {request.error}");
                    }
                }
                else
                    OnError($"Error getting an IP address : {getIPRequest.error}");
            }
        }
        else
            OnError($"Invalid url! IPServiceURL: {IPServiceURL}  TimeServiceURL: {TimeServiceURL}");

        void OnError(string errorMessage)
        {
            Debug.LogError(errorMessage);
            onTimeReceived?.Invoke(null);
        }
    }
}