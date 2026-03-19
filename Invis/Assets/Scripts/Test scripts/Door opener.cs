using UnityEngine;
using TMPro;

public class Dooropener : MonoBehaviour
{
    public GameObject DoorOne;
    public GameObject DoorTwo;
    public TextMeshProUGUI Hint;
    public KeyCode RemoveDoors = KeyCode.E;

    private bool isPlayerInside = false; // Флаг: внутри ли игрок

    void Start()
    {
        if (Hint != null) Hint.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Проверяем нажатие кнопки каждое мгновение, но только если игрок внутри
        if (isPlayerInside && Input.GetKeyDown(RemoveDoors))
        {
            OpenDoors();
        }
    }

    private void OpenDoors()
    {
        if (DoorOne != null) DoorOne.SetActive(false);
        if (DoorTwo != null) DoorTwo.SetActive(false);

        // Выключаем подсказку после открытия
        if (Hint != null) Hint.gameObject.SetActive(false);

        // Можно выключить сам скрипт или триггер, чтобы он больше не работал
        this.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что это именно игрок (желательно по тегу)
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            if (Hint != null) Hint.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (Hint != null) Hint.gameObject.SetActive(false);
        }
    }
}