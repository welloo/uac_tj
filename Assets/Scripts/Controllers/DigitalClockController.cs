using System;
using UnityEngine;
using UnityEngine.UI;

public class DigitalClockController : TimeSyncObserver
{
    [SerializeField] Text timeText;

    protected override void OnTimeUpdated(DateTime currentTime)
    {
        timeText.text = currentTime.ToString("HH:mm:ss");
    }
}
