using UnityEngine;
using System.IO;

public class EvidenceSaveManager : MonoBehaviour
{
    public EvidenceBoardController boardController;
    private string savePath;

    void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "evidence_board.json");
    }

    public void SaveBoard()
    {
        BoardSaveData data = new BoardSaveData();
        // Цикл по boardController.spawnedCards...
        // Собираем позиции и GUID

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
    }

    public void LoadBoard()
    {
        if (!File.Exists(savePath)) return;

        string json = File.ReadAllText(savePath);
        BoardSaveData data = JsonUtility.FromJson<BoardSaveData>(json);

        // Очищаем текущую доску и создаем объекты заново по данным из Data
    }
}