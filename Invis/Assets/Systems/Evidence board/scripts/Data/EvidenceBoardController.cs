using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class EvidenceBoardController : MonoBehaviour, IDragHandler, IScrollHandler
{
    [Header("Settings")]
    public float zoomSpeed = 0.1f;
    public float minZoom = 0.5f;
    public float maxZoom = 2f;

    [Header("References: Board")]
    public RectTransform boardContent; // Îáúåêò, êîòîğûé ìû çóìèì (Viewport Content)
    public GameObject cardPrefab;
    public ConnectionManager connectionManager;

    [Header("References: Description UI")]
    public GameObject rightPanelParent;
    public TextMeshProUGUI rightPanelName;
    public TextMeshProUGUI rightPanelDescription;
    public Image rightPanelIcon;

    private List<BoardCard> spawnedCards = new List<BoardCard>();
    private BoardCard selectedCard;
    private int guidCounter = 0;

    void Update()
    {
        // Óäàëåíèå êàğòî÷êè èëè ëèíèè
        if (Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace))
        {
            DeleteSelected();
        }
    }

    // --- ËÎÃÈÊÀ ÂÛÄÅËÅÍÈß ---
    public void SelectCard(BoardCard card)
    {
        if (selectedCard != null) selectedCard.SetSelected(false);
        selectedCard = card;
        selectedCard.SetSelected(true);
        ShowDescription(card.data);
    }

    public void ShowDescription(ItemData data)
    {
        if (data == null) return;
        if (rightPanelParent != null) rightPanelParent.SetActive(true);
        if (rightPanelName != null) rightPanelName.text = data.itemName;
        if (rightPanelDescription != null) rightPanelDescription.text = data.itemDescription;
        if (rightPanelIcon != null) { rightPanelIcon.sprite = data.icon; rightPanelIcon.enabled = true; }
    }

    private void DeleteSelected()
    {
        if (selectedCard == null) return;
        connectionManager.RemoveLinesForCard(selectedCard);
        spawnedCards.Remove(selectedCard);
        Destroy(selectedCard.gameObject);
        selectedCard = null;
        if (rightPanelParent != null) rightPanelParent.SetActive(false);
    }

    // --- ÍÀÂÈÃÀÖÈß (ÇÓÌ È ÏÀÍ) ---
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && eventData.pointerPress == gameObject)
        {
            boardContent.anchoredPosition += eventData.delta;
        }
    }

    public void OnScroll(PointerEventData eventData)
    {
        float scroll = eventData.scrollDelta.y;
        float newScale = boardContent.localScale.x + scroll * zoomSpeed;
        newScale = Mathf.Clamp(newScale, minZoom, maxZoom);
        boardContent.localScale = Vector3.one * newScale;
    }

    public float GetCurrentZoom() => boardContent.localScale.x;

    // --- ÑÎÇÄÀÍÈÅ ÊÀĞÒÎ×ÅÊ ---
    public void AddItemToBoard(ItemData data, Vector2 screenPos)
    {
        GameObject go = Instantiate(cardPrefab, boardContent);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(boardContent, screenPos, null, out Vector2 localPos);
        go.GetComponent<RectTransform>().anchoredPosition = localPos;

        BoardCard card = go.GetComponent<BoardCard>();
        card.Setup(data, guidCounter++, this);
        spawnedCards.Add(card);
        SelectCard(card);
    }
}