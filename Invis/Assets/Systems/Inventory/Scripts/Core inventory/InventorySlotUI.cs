using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    ISelectHandler, IDeselectHandler, IPointerClickHandler // Добавили интерфейс клика
{
    [SerializeField] private Image iconImage;
    private ItemData currentItem;

    public void SetItem(ItemData item)
    {
        currentItem = item;
        if (item != null)
        {
            iconImage.sprite = item.icon;
            iconImage.enabled = true;
        }
        else
        {
            iconImage.enabled = false;
        }
    }

    // Метод клика по ячейке
    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem == null) return;

        DialogueManager dialManager = FindObjectOfType<DialogueManager>();

        // Проверяем: если сейчас открыт диалог, то предмет "показываем"
        if (dialManager != null && dialManager.dialoguePanel.activeSelf)
        {
            dialManager.ReceiveItemFromInventory(currentItem);
        }
        else
        {
            // Обычная логика использования предмета вне диалога
            Debug.Log("Использован предмет: " + currentItem.itemName);
            // currentItem.Use(); // Раскомментируйте, если в ItemData есть метод Use
        }
    }

    // --- Логика Тултипов (сохранена как была) ---

    public void OnPointerEnter(PointerEventData eventData) => ShowTooltip();
    public void OnSelect(BaseEventData eventData) => ShowTooltip();

    public void OnPointerExit(PointerEventData eventData) => HideTooltip();
    public void OnDeselect(BaseEventData eventData) => HideTooltip();

    private void ShowTooltip()
    {
        if (currentItem != null && TooltipManager.Instance != null)
        {
            TooltipManager.Instance.Show(currentItem);
        }
    }

    private void HideTooltip()
    {
        if (TooltipManager.Instance != null)
        {
            TooltipManager.Instance.Hide();
        }
    }
}