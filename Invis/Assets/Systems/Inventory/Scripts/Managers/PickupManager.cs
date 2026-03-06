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
    public Button closeButton;
    public Button takeButton;

    public InventorySO inventory;

    private WorldItem currentWorldItem;

    private void Awake()
    {
        // Проверка на дубликаты
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (menuPanel != null) menuPanel.SetActive(false);

        closeButton.onClick.AddListener(CloseMenu);
        takeButton.onClick.AddListener(TakeItem);
    }

    public void OpenMenu(WorldItem worldItem)
    {
        currentWorldItem = worldItem;

        nameText.text = worldItem.itemData.itemName;
        descriptionText.text = worldItem.itemData.description;
        itemIcon.sprite = worldItem.itemData.icon;

        // ВАЖНО: Принудительно включаем панель
        menuPanel.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("PickupManager: Открываю панель для " + worldItem.itemData.itemName);
    }

    public void CloseMenu()
    {
        if (menuPanel != null) menuPanel.SetActive(false);
        currentWorldItem = null;

        // Передаем управление курсором глобальному менеджеру или блокируем здесь
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void TakeItem()
    {
        if (currentWorldItem != null && inventory != null)
        {
            if (inventory.AddItem(currentWorldItem.itemData))
            {
                Destroy(currentWorldItem.gameObject);
                CloseMenu();
            }
        }
    }
}