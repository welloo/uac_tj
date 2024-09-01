using System;
using UnityEngine;

public class AlarmManager : TimeSyncObserver, IAlarmManager
{
    [SerializeField] AudioClip alarmSound;
    [SerializeField] AudioSource audioSource;
    private DateTime alarmTime;
    private bool isAlarmSet = false;

    public void SetAlarm(int hours, int minutes)
    {
        alarmTime = DateTime.Today.AddHours(hours).AddMinutes(minutes);
        if (alarmTime < DateTime.Now)
            alarmTime = alarmTime.AddDays(1);
        isAlarmSet = true;
        Debug.Log($"The alarm clock is set to: {alarmTime.ToString("HH: mm")}");
    }

    private void TriggerAlarm()
    {
        isAlarmSet = false;
        PlayAlarmSound();
        Debug.Log("The alarm went off!");
    }

    private void PlayAlarmSound()
    {
        if (audioSource && alarmSound)
            audioSource.PlayOneShot(alarmSound);
    }

    protected override void OnTimeUpdated(DateTime currentTime)
    {
        if (isAlarmSet && currentTime >= alarmTime)
            TriggerAlarm();
    }
}
