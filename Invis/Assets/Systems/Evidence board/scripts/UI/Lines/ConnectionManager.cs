using UnityEngine;
using System.Collections.Generic;

public class ConnectionManager : MonoBehaviour
{
    public GameObject linePrefab; // UI Image с красным цветом
    public RectTransform linesContainer;

    private List<BoardLine> activeLines = new List<BoardLine>();

    public BoardLine CreateLine(BoardCard start, BoardCard end)
    {
        GameObject go = Instantiate(linePrefab, linesContainer);
        BoardLine line = go.GetComponent<BoardLine>();
        line.Setup(start, end);
        activeLines.Add(line);
        return line;
    }

    public void RemoveLinesForCard(BoardCard card)
    {
        List<BoardLine> toRemove = activeLines.FindAll(l => l.startCard == card || l.endCard == card);
        foreach (var l in toRemove)
        {
            activeLines.Remove(l);
            Destroy(l.gameObject);
        }
    }

    public void ClearAll()
    {
        foreach (var l in activeLines) Destroy(l.gameObject);
        activeLines.Clear();
    }

    public List<BoardLine> GetLines() => activeLines;
}