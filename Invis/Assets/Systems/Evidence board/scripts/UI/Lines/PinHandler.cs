using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PinHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private BoardCard ownerCard;
    private ConnectionManager connectionManager;
    private EvidenceBoardController boardController;

    void Awake()
    {
        ownerCard = GetComponentInParent<BoardCard>();
        connectionManager = FindObjectOfType<ConnectionManager>();
        boardController = FindObjectOfType<EvidenceBoardController>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Ќачинаем т€нуть временную линию от этой булавки
        connectionManager.StartDrawingLine(ownerCard, eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        connectionManager.UpdateDrawingLine(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // ѕровер€ем, попали ли мы на другую булавку
        PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = eventData.position };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            PinHandler targetPin = result.gameObject.GetComponent<PinHandler>();
            if (targetPin != null && targetPin != this)
            {
                connectionManager.FinalizeLine(targetPin.ownerCard);
                return;
            }
        }
        connectionManager.CancelDrawingLine();
    }
}