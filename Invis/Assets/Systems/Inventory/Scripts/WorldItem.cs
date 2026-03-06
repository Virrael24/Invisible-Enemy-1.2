using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public ItemData itemData; // Ссылка на ScriptableObject предмета

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что в зону вошел Игрок (тег "Player")
        if (other.CompareTag("Player"))
        {
            // Передаем данные в менеджер интерфейса
            PickupManager.Instance.OpenMenu(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickupManager.Instance.CloseMenu();
        }
    }
}
