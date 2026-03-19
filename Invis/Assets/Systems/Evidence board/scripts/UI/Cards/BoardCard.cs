using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class BoardCard : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public ItemData data;
    public int guid;

    [Header("UI References")]
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public GameObject selectionVisual;
    public RectTransform pinPoint; // Точка-булавка снизу

    private EvidenceBoardController board;
    private RectTransform rectTransform;

    public void Setup(ItemData itemData, int uniqueId, EvidenceBoardController controller)
    {
        data = itemData;
        guid = uniqueId;
        board = controller;
        rectTransform = GetComponent<RectTransform>();

        if (iconImage != null) iconImage.sprite = data.icon;
        if (nameText != null) nameText.text = data.itemName;
        SetSelected(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        board.SelectCard(this);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / board.GetCurrentZoom();
        board.connectionManager.UpdateLinesForCard(this);
    }

    public void SetSelected(bool isSelected)
    {
        if (selectionVisual != null) selectionVisual.SetActive(isSelected);
    }
}