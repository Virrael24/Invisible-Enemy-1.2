using UnityEngine;
using UnityEngine.Events;

public class TimeTrigger : MonoBehaviour
{
    [Header("Когда сработать?")]
    public int targetHour;
    public int targetMinute;

    [Header("Что сделать?")]
    public UnityEvent action;

    private bool _hasTriggered = false;

    private void OnEnable()
    {
        TimeManager.OnTimeChanged += CheckTime;
    }

    private void OnDisable()
    {
        TimeManager.OnTimeChanged -= CheckTime;
    }

    void CheckTime(int hour, int minute)
    {
        if (_hasTriggered) return;

        // Если текущее время совпало или перевалило за нужное
        if (hour > targetHour || (hour == targetHour && minute >= targetMinute))
        {
            _hasTriggered = true;
            action.Invoke();
            Debug.Log($"Событие сработало в {hour}:{minute}");
        }
    }
}