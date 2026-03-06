
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PickupManager : MonoBehaviour
{
    public static PickupManager Instance;

    [Header("Ссылки на UI")]
    public GameObject menuPanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Image itemIcon;

    [Header("Кнопки")]
    public Button closeButton;
    public Button takeButton;

    [Header("Инвентарь")]
    public InventorySO inventory;

    private WorldItem currentWorldItem;

    private void Awake()
    {
        Instance = this;
        menuPanel.SetActive(false);

        // Назначаем функции кнопкам программно
        closeButton.onClick.AddListener(CloseMenu);
        takeButton.onClick.AddListener(TakeItem);
    }

    public void OpenMenu(WorldItem worldItem)
    {
        currentWorldItem = worldItem;
        ItemData data = worldItem.itemData;

        // Заполняем данные
        nameText.text = data.itemName;
        descriptionText.text = data.description;
        itemIcon.sprite = data.icon;

        menuPanel.SetActive(true);

        // Включаем курсор, чтобы нажать кнопки
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseMenu()
    {
        menuPanel.SetActive(false);
        currentWorldItem = null;

        // Возвращаем курсор в игру (опционально, зависит от вашего управления)
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    public void TakeItem()
    {
        if (currentWorldItem != null && inventory != null)
        {
            // Пробуем добавить в инвентарь
            bool wasAdded = inventory.AddItem(currentWorldItem.itemData);

            if (wasAdded)
            {
                // Если место было, удаляем объект со сцены
                Destroy(currentWorldItem.gameObject);
                CloseMenu();
            }
            else
            {
                Debug.Log("Нет места в инвентаре!");
            }
        }
    }
}
