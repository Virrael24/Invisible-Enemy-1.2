using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Добавляем интерфейсы выделения (для клавиатуры)
public class InventorySlotUI : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    ISelectHandler, IDeselectHandler
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

    // Сбрасываем тултип при уходе курсора или потере фокуса
    public void OnPointerExit(PointerEventData eventData) => HideTooltip();
    public void OnDeselect(BaseEventData eventData) => HideTooltip();

    // Показываем тултип при наведении или выделении стрелками
    public void OnPointerEnter(PointerEventData eventData) => ShowTooltip();
    public void OnSelect(BaseEventData eventData) => ShowTooltip();

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