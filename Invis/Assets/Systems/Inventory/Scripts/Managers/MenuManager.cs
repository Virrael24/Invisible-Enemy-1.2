using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("ѕанели меню")]
    public GameObject inventoryPanel; // —сылка на панель инвентар€
    public GameObject pickupPanel;    // —сылка на панель описани€ (из PickupManager)

    private bool isInventoryOpen = false;

    void Update()
    {
        // ќткрытие/закрытие инвентар€ на I
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }

        // «акрытие всего на Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseEverything();
        }
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen);

        // ≈сли открыли инвентарь Ч закрываем окно подбора, чтобы не мешалось
        if (isInventoryOpen) pickupPanel.SetActive(false);

        UpdateCursorState();
    }

    public void CloseEverything()
    {
        isInventoryOpen = false;
        inventoryPanel.SetActive(false);
        pickupPanel.SetActive(false);

        UpdateCursorState();
    }

    // ”правление мышкой: если хоть одно меню открыто Ч показываем курсор
    private void UpdateCursorState()
    {
        bool anyMenuOpen = inventoryPanel.activeSelf || pickupPanel.activeSelf;

        if (anyMenuOpen)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
