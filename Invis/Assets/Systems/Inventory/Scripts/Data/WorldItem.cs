using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public ItemData itemData; // Данные предмета
    public GameObject interactPrompt; // Ссылка на наш World Space Canvas ("Нажми E")

    private bool isPlayerInside = false;

    private void Start()
    {
        // На старте подсказка всегда выключена
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    private void Update()
    {
        // Если игрок внутри зоны и нажал клавишу E
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            OpenInteractionMenu();
        }
    }

    private void OpenInteractionMenu()
    {
        // Скрываем подсказку "Нажми E", когда открываем основное меню
        interactPrompt.SetActive(false);

        // Передаем данные в PickupManager (тот скрипт, что мы писали раньше)
        PickupManager.Instance.OpenMenu(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            if (interactPrompt != null)
                interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (interactPrompt != null)
                interactPrompt.SetActive(false);

            // Если игрок ушел, закрываем и основное меню (если оно было открыто)
            PickupManager.Instance.CloseMenu();
        }
    }
}
