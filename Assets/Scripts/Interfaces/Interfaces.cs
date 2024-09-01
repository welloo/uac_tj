using System;
using System.Collections;

public interface ITimeService
{
    string TimeServiceURL { get; }
    string IPServiceURL { get; }
    IEnumerator GetApiResult(Action<string> onTimeReceived);
}

public interface ITimeSyncManager
{
    event TimeUpdatedEventHandler OnTimeUpdated;
}

public interface IAlarmManager
{
    void SetAlarm(int hours, int minutes);
}

public interface IJsonTimeParser
{
    bool TryGetParsedTime(string json, out DateTime parsedTime);
}