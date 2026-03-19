using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BoardSaveData
{
    public List<CardSaveData> cards = new List<CardSaveData>();
    public List<ConnectionSaveData> connections = new List<ConnectionSaveData>();
}

[Serializable]
public class CardSaveData
{
    public string itemId; // Название или ID из ItemData
    public Vector2 position;
    public int guid; // Уникальный ID экземпляра на доске
}

[Serializable]
public class ConnectionSaveData
{
    public int startGuid;
    public int endGuid;
}