using System;
using UnityEngine;

public enum HandType
{
    none = -1,
    Second,
    Minute,
    Hour
}
public class ClockHandController : TimeSyncObserver
{
    [SerializeField] HandType type = HandType.none;
    protected override void OnTimeUpdated(DateTime currentTime)
    {
        if (type == HandType.none)
        {
            Debug.LogError("You need to set the hand type.");
            return;
        }
        float value = type switch
        {
            HandType.Hour => 12 + currentTime.Hour % 12,
            HandType.Minute => currentTime.Minute,
            HandType.Second or _ => currentTime.Second,
        };
        float maxCount = type switch
        {
            HandType.Hour => 12f,
            HandType.Minute or HandType.Second or _ => 60f,
        };
        transform.localEulerAngles = new Vector3(0.0f, value / maxCount * 360.0f, 0.0f);
    }
}
