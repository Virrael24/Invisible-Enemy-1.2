using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("Настройки времени")]
    public int startHour = 6;
    public int endHour = 23;
    public float secondsPerFiveMinutes = 5f; // Сколько реальных секунд длится 5 игровых минут

    [Header("Текущее время")]
    public int currentHour;
    public int currentMinute;

    // Событие, на которое могут подписываться другие скрипты
    public static event Action<int, int> OnTimeChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        currentHour = startHour;
        currentMinute = 0;
        StartCoroutine(TimeTick());
    }

    IEnumerator TimeTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(secondsPerFiveMinutes);

            AddMinutes(5);
        }
    }

    void AddMinutes(int minutes)
    {
        currentMinute += minutes;

        if (currentMinute >= 60)
        {
            currentMinute = 0;
            currentHour++;
        }

        // Проверка окончания дня
        if (currentHour >= endHour && currentMinute > 0)
        {
            StopAllCoroutines(); // Останавливаем время в конце дня
            Debug.Log("День завершен");
            return;
        }

        // Оповещаем всех подписчиков (UI, триггеры событий)
        OnTimeChanged?.Invoke(currentHour, currentMinute);
    }

    // Вспомогательный метод для проверки времени (например, "сейчас больше чем 17:30?")
    public bool IsTimePast(int hour, int minute)
    {
        if (currentHour > hour) return true;
        if (currentHour == hour && currentMinute >= minute) return true;
        return false;
    }
}