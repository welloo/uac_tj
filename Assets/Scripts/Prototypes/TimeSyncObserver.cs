using System;
using UnityEngine;

public abstract class TimeSyncObserver : MonoBehaviour
{
    protected ITimeSyncManager timeSyncManager;
    public void Initialize(ITimeSyncManager timeManager)
    {
        timeSyncManager = timeManager;
        timeSyncManager.OnTimeUpdated += OnTimeUpdated;
    }

    protected abstract void OnTimeUpdated(DateTime currentTime);

    protected virtual void OnDestroy()
    {
        if (timeSyncManager != null)
        {
            timeSyncManager.OnTimeUpdated -= OnTimeUpdated;
        }
    }
}