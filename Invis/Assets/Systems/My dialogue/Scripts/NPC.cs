using UnityEngine;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{
    [Header("Основная информация")]
    public string npcName; // Именно это имя ищет DialogueManager
    public int relationshipPoints = 0;

    [Header("Диалоги")]
    public DialogueNode defaultDialogue;
    public DialogueNode highRelationshipDialogue;

    [Header("UI Подсказка")]
    public GameObject interactionHint; // Объект "Нажми E"

    [Header("Знания о предметах")]
    public List<ItemReaction> itemReactions = new List<ItemReaction>();
    [TextArea(2, 3)] public string unknownItemResponse = "Я не знаю, что это за вещь.";

    private bool _canTalk;
    private DialogueManager _manager;

    void Start()
    {
        _manager = FindObjectOfType<DialogueManager>();
        if (interactionHint != null) interactionHint.SetActive(false);
    }

    void Update()
    {
        // Начало диалога на клавишу E
        if (_canTalk && Input.GetKeyDown(KeyCode.E))
        {
            if (interactionHint != null) interactionHint.SetActive(false);

            // Выбираем диалог в зависимости от отношений
            DialogueNode nodeToUse = (relationshipPoints >= 10) ? highRelationshipDialogue : defaultDialogue;
            _manager.StartDialogue(nodeToUse, this);
        }
    }

    // Метод для получения реакции на предмет (используется Менеджером)
    public ItemReaction GetReaction(ItemData item)
    {
        return itemReactions.Find(r => r.item == item);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canTalk = true;
            if (interactionHint != null) interactionHint.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canTalk = false;
            if (interactionHint != null) interactionHint.SetActive(false);
            _manager.EndDialogue(); // Закрываем диалог, если игрок ушел
        }
    }
}