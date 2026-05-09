using UnityEngine;

[System.Serializable]
public class ItemReaction
{
    public ItemData item; // Ссылка на ваш ScriptableObject предмета
    [TextArea(2, 5)] public string lowRelationText;  // Если отношения < 10
    [TextArea(2, 5)] public string highRelationText; // Если отношения >= 10
    public Sprite npcEmotion; // Спрайт NPC при виде этого предмета
}