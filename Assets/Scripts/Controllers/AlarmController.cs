using UnityEngine;
using UnityEngine.UI;

public class AlarmController : MonoBehaviour
{
    [SerializeField] InputField hoursInput, minutesInput;
    [SerializeField] Button setAlarmButton;

    private IAlarmManager alarmManager;

    private void Start()
    {
        ResetInputs();
        setAlarmButton.onClick.AddListener(OnSetAlarmClicked);
    }

    public void Initialize(IAlarmManager alarmManager)
    {
        this.alarmManager = alarmManager;
    }

    private void OnSetAlarmClicked()
    {
        if (alarmManager != null)
        {
            if (int.TryParse(hoursInput.text, out int hours) && int.TryParse(minutesInput.text, out int minutes))
            {
                if (IsValidTime(hours, minutes))
                    alarmManager.SetAlarm(hours, minutes);
                else
                {
                    ResetInputs();
                    Debug.LogWarning("Wrong time. Enter the hours from 0 to 23 and the minutes from 0 to 59.");
                }
            }
            else
                Debug.LogError("Incorrect input. Please enter numeric values for hours and minutes.");
        }
        else
            Debug.LogError("You must assign an AlarmManager.");
    }

    private void ResetInputs() => hoursInput.text = minutesInput.text = "0";

    private bool IsValidTime(int hours, int minutes) => hours >= 0 && hours <= 23 && minutes >= 0 && minutes <= 59;
}
