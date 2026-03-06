using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    public InventorySO inventory;
    public ItemData[] testItems; // Массив разных предметов
    private int currentIndex = 0; // Индекс текущего предмета

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddNextItem();
        }
    }

    private void AddNextItem()
    {
        // Проверки на ошибки
        if (inventory == null) return;
        if (testItems == null || testItems.Length == 0) return;

        // Берем предмет по текущему индексу
        ItemData itemToAdd = testItems[currentIndex];

        // Пытаемся добавить в инвентарь
        bool success = inventory.AddItem(itemToAdd);

        if (success)
        {
            Debug.Log($"Добавлен предмет: {itemToAdd.itemName}");

            // Увеличиваем индекс для следующего раза
            currentIndex++;

            // Если предметы кончились, начинаем сначала (зацикливаем)
            if (currentIndex >= testItems.Length)
            {
                currentIndex = 0;
            }
        }
        else
        {
            Debug.LogWarning("Инвентарь полон!");
        }
    }
}