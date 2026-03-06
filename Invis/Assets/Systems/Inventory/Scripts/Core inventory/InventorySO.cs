using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/System")]
public class InventorySO : ScriptableObject
{
    public ItemData[] slots; // Убрали фиксацию здесь
    public int slotCount = 20; // Вынесли размер отдельной переменной
    public event Action OnInventoryChanged;

    // Этот метод вызывается автоматически в Unity
    private void OnEnable()
    {
        // Если массив не создан или его размер не совпадает с нужным
        if (slots == null || slots.Length != slotCount)
        {
            Array.Resize(ref slots, slotCount);
        }
    }

    public bool AddItem(ItemData item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = item;
                OnInventoryChanged?.Invoke();
                return true;
            }
        }
        return false;
    }
}