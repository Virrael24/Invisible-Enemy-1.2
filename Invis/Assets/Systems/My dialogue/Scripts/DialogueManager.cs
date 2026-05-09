using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Панели")]
    public GameObject dialoguePanel;
    public GameObject inventoryPanel;

    [Header("Текст и Имена")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    [Header("Портреты")]
    public Image npcPortraitHolder;
    public Image playerPortraitHolder;

    [Header("Кнопки")]
    public GameObject[] choiceButtons;
    public GameObject askItemButton;

    [Header("Настройки")]
    public float typingSpeed = 0.04f;
    public Color activeColor = Color.white;
    public Color dimColor = new Color(0.5f, 0.5f, 0.5f);

    private DialogueNode _currentNode;
    private int _lineIndex;
    private bool _isTyping;
    private bool _isAskingAboutItem;
    private NPC _currentNPC;

    public void StartDialogue(DialogueNode node, NPC npc)
    {
        _currentNPC = npc; // Сохраняем ссылку на NPC, с которым говорим
        _currentNode = node;
        _lineIndex = 0;
        _isAskingAboutItem = false;

        dialoguePanel.SetActive(true);
        if (askItemButton != null) askItemButton.SetActive(true);

        DisplayLine();
    }

    void Update()
    {
        // Листать фразы на Пробел (только если не открыт выбор или инвентарь)
        if (Input.GetKeyDown(KeyCode.Space) && !_isTyping && dialoguePanel.activeSelf && !_isAskingAboutItem)
        {
            if (_currentNode.lines[_lineIndex].choices.Length == 0)
                NextLine();
        }
    }

    void DisplayLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeSentence(_currentNode.lines[_lineIndex]));
    }

    IEnumerator TypeSentence(DialogueLine line)
    {
        _isTyping = true;
        dialogueText.text = "";

        // ВАЖНО: берем имя из класса NPC через переменную npcName
        nameText.text = line.isPlayerSpeaking ? "Вы" : _currentNPC.npcName;

        // Настройка портретов
        npcPortraitHolder.sprite = line.npcPortrait;
        playerPortraitHolder.sprite = line.playerPortrait;

        // Затенение неактивного персонажа
        npcPortraitHolder.color = line.isPlayerSpeaking ? dimColor : activeColor;
        playerPortraitHolder.color = line.isPlayerSpeaking ? activeColor : dimColor;

        foreach (char letter in line.text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        _isTyping = false;

        // Если есть кнопки выбора — показываем
        if (line.choices != null && line.choices.Length > 0)
            ShowChoices(line.choices);
    }

    // --- ЛОГИКА ИНВЕНТАРЯ ---

    public void ClickAskAboutItem()
    {
        if (_isTyping) return;
        _isAskingAboutItem = true;
        inventoryPanel.SetActive(true);
    }

    public void ReceiveItemFromInventory(ItemData item)
    {
        _isAskingAboutItem = false;
        inventoryPanel.SetActive(false);

        // Получаем реакцию из скрипта NPC
        ItemReaction reaction = _currentNPC.GetReaction(item);

        DialogueLine responseLine = new DialogueLine();
        responseLine.characterName = _currentNPC.npcName; // Ошибка должна исчезнуть здесь
        responseLine.isPlayerSpeaking = false;
        responseLine.playerPortrait = playerPortraitHolder.sprite;

        if (reaction != null)
        {
            responseLine.text = (_currentNPC.relationshipPoints >= 10)
                ? reaction.highRelationText
                : reaction.lowRelationText;
            responseLine.npcPortrait = reaction.npcEmotion;
        }
        else
        {
            responseLine.text = _currentNPC.unknownItemResponse;
            responseLine.npcPortrait = npcPortraitHolder.sprite;
        }

        StopAllCoroutines();
        StartCoroutine(TypeSentence(responseLine));
    }

    // --- ОСТАЛЬНАЯ ЛОГИКА ---

    void NextLine()
    {
        _lineIndex++;
        if (_lineIndex < _currentNode.lines.Length)
        {
            DisplayLine();
        }
        else if (_currentNode.nextNode != null)
        {
            _currentNode = _currentNode.nextNode;
            _lineIndex = 0;
            DisplayLine();
        }
        else
        {
            EndDialogue();
        }
    }

    void ShowChoices(DialogueChoice[] choices)
    {
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < choices.Length)
            {
                choiceButtons[i].SetActive(true);
                choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = choices[i].choiceText;

                int index = i;
                Button btn = choiceButtons[index].GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => {
                    _currentNPC.relationshipPoints += choices[index].relationshipChange;
                    _currentNode = choices[index].nextNode;
                    _lineIndex = 0;
                    foreach (var b in choiceButtons) b.SetActive(false);
                    DisplayLine();
                });
            }
            else choiceButtons[i].SetActive(false);
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        dialoguePanel.SetActive(false);
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        _isTyping = false;
        _isAskingAboutItem = false;
    }
}