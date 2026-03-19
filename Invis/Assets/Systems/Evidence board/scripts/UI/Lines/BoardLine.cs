// BOARD LINE SCRIPT
using System.Collections.Generic;
using UnityEngine;

public class BoardLine : MonoBehaviour
{
    public BoardCard startCard;
    public BoardCard endCard;
    private RectTransform rt;

    void Awake() => rt = GetComponent<RectTransform>();

    public void Setup(BoardCard start, BoardCard end) { startCard = start; endCard = end; UpdateLine(); }

    public void UpdateLine()
    {
        if (startCard == null || endCard == null) return;
        Vector2 startPos = startCard.pinPoint.position;
        Vector2 endPos = endCard.pinPoint.position;
        DrawBetweenPoints(startPos, endPos);
    }

    public void DrawBetweenPoints(Vector2 start, Vector2 end)
    {
        Vector2 dir = end - start;
        rt.position = start;
        rt.sizeDelta = new Vector2(dir.magnitude / transform.lossyScale.x, 5f);
        rt.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
    }
}

// CONNECTION MANAGER SCRIPT
public class ConnectionManager : MonoBehaviour
{
    public GameObject linePrefab;
    private List<BoardLine> activeLines = new List<BoardLine>();
    private BoardLine currentDrawingLine;

    public void StartDrawingLine(BoardCard start, Vector2 mousePos)
    {
        GameObject go = Instantiate(linePrefab, transform);
        currentDrawingLine = go.GetComponent<BoardLine>();
        currentDrawingLine.startCard = start;
    }

    public void UpdateDrawingLine(Vector2 mousePos) => currentDrawingLine.DrawBetweenPoints(currentDrawingLine.startCard.pinPoint.position, mousePos);

    public void FinalizeLine(BoardCard end)
    {
        currentDrawingLine.Setup(currentDrawingLine.startCard, end);
        activeLines.Add(currentDrawingLine);
        currentDrawingLine = null;
    }

    public void CancelDrawingLine() { if (currentDrawingLine != null) Destroy(currentDrawingLine.gameObject); }

    public void UpdateLinesForCard(BoardCard card) { foreach (var l in activeLines) l.UpdateLine(); }

    public void RemoveLinesForCard(BoardCard card)
    {
        List<BoardLine> toRemove = activeLines.FindAll(l => l.startCard == card || l.endCard == card);
        foreach (var l in toRemove) { activeLines.Remove(l); Destroy(l.gameObject); }
    }

    public List<BoardLine> GetLines() => activeLines;
}