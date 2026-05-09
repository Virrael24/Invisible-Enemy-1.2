using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string characterName;
    [Header("Портреты")]
    public Sprite npcPortrait;    // Конкретная эмоция NPC для этой фразы
    public Sprite playerPortrait; // Конкретная эмоция ГГ для этой фразы

    [TextArea(3, 5)] public string text;
    public bool isPlayerSpeaking;
    public DialogueChoice[] choices;
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public DialogueNode nextNode;
    public int relationshipChange;
}

[CreateAssetMenu(fileName = "NewDialogueNode", menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    public DialogueLine[] lines;
    public DialogueNode nextNode; // Проверьте, что это поле ВНЕ класса DialogueLine
    public int minRelationshipRequired = 0;
}