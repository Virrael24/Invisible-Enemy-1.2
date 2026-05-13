using UnityEngine;
using TMPro;

public class TimeUI : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    private void OnEnable()
    {
        TimeManager.OnTimeChanged += UpdateTimeDisplay;
    }

    private void OnDisable()
    {
        TimeManager.OnTimeChanged -= UpdateTimeDisplay;
    }

    void UpdateTimeDisplay(int hour, int minute)
    {
        // Форматирование 00:00
        timeText.text = string.Format("{0:D2}:{1:D2}", hour, minute);
    }
}