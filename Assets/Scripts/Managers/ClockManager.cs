using UnityEngine;

public class ClockManager : MonoBehaviour
{
    [SerializeField] TimeSyncObserver[] timeSyncObservers;
    [SerializeField] TimeSyncManager timeSyncManager;
    [SerializeField] AlarmManager alarmManager;
    [SerializeField] AlarmController alarmController;

    private void Awake()
    {
        if (timeSyncManager)
        {
            foreach (var timeSyncItem in timeSyncObservers)
                timeSyncItem.Initialize(timeSyncManager);
        }
        else
            Debug.LogError("You need to assign a TimeSyncManager.");
        if (alarmController)
            alarmController.Initialize(alarmManager);
        else
            Debug.LogError("You must assign an AlarmController.");
    }
}
