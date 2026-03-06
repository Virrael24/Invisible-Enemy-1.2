using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class InventoryUI : MonoBehaviour
{
    public InventorySO inventoryData;
    public GameObject slotPrefab;
    public Transform container;

    private void Start()
    {
        // Проверка на ошибки в инспекторе
        if (inventoryData == null) { Debug.LogError("Забыли перетащить Inventory Data в InventoryUI!"); return; }
        if (slotPrefab == null) { Debug.LogError("Забыли перетащить Slot Prefab в InventoryUI!"); return; }
        if (container == null) { Debug.LogError("Забыли перетащить Container (панель) в InventoryUI!"); return; }

        inventoryData.OnInventoryChanged += RefreshUI;
        RefreshUI();
    }

    private void OnDestroy()
    {
        if (inventoryData != null) inventoryData.OnInventoryChanged -= RefreshUI;
    }

    public void RefreshUI()
    {
        // Очистка
        foreach (Transform child in container) Destroy(child.gameObject);

        Debug.Log($"Рисую инвентарь. Количество слотов в данных: {inventoryData.slots.Length}");

        for (int i = 0; i < inventoryData.slots.Length; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, container);
            InventorySlotUI slotScript = newSlot.GetComponent<InventorySlotUI>();

            if (slotScript != null)
            {
                slotScript.SetItem(inventoryData.slots[i]);
            }

            if (i == 0) StartCoroutine(FocusFirstSlot(newSlot));
        }
    }

    private IEnumerator FocusFirstSlot(GameObject slot)
    {
        yield return new WaitForEndOfFrame();
        if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(slot);
    }
}