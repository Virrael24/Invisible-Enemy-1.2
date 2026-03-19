using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EvidenceEntry : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Настройки данных")]
    public ItemData data; // Данные улики

    [Header("Ссылки на UI элементы префаба")]
    public Image entryIcon;         // Сюда перетащи картинку из префаба
    public TextMeshProUGUI entryName; // Сюда перетащи текст из префаба

    private GameObject dragIcon;    // Временный объект для визуализации перетаскивания
    private Canvas mainCanvas;      // Ссылка на главный канвас

    private void Awake()
    {
        // Ищем канвас, чтобы иконка при перетаскивании была поверх всего
        mainCanvas = GetComponentInParent<Canvas>();
    }

    // Метод для заполнения данных (вызывается из EvidenceListUI)
    public void Setup(ItemData item)
    {
        data = item;
        if (entryIcon != null) entryIcon.sprite = data.icon;
        if (entryName != null) entryName.text = data.itemName;
    }

    // 1. ОБЫЧНЫЙ КЛИК: Показываем описание в правой панели
    public void OnPointerClick(PointerEventData eventData)
    {
        // Находим контроллер доски и просим его обновить правую панель
        EvidenceBoardController board = FindObjectOfType<EvidenceBoardController>();
        if (board != null)
        {
            board.ShowDescription(data);
        }
    }

    // 2. НАЧАЛО ПЕРЕТАСКИВАНИЯ
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Создаем временную иконку, которая будет летать за курсором
        dragIcon = new GameObject("TempDragIcon");
        dragIcon.transform.SetParent(mainCanvas.transform);
        dragIcon.transform.SetAsLastSibling(); // Поверх всех окон

        // Добавляем ей компонент Image и копируем иконку улики
        Image img = dragIcon.AddComponent<Image>();
        img.sprite = data.icon;
        img.raycastTarget = false; // Чтобы иконка не мешала лучу определять объекты под ней

        // Задаем размер иконки (например, 100x100)
        dragIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(100f, 100f);
    }

    // 3. ПРОЦЕСС ПЕРЕТАСКИВАНИЯ (движение за мышкой)
    public void OnDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
        {
            dragIcon.transform.position = eventData.position;
        }
    }

    // 4. КОНЕЦ ПЕРЕТАСКИВАНИЯ (отпускаем кнопку)
    public void OnEndDrag(PointerEventData eventData)
    {
        // Удаляем временную иконку
        if (dragIcon != null) Destroy(dragIcon);

        // Проверяем, на каком объекте мы отпустили мышку
        // Важно: объект центральной панели должен называться "CenterPanel_Board" 
        // или иметь скрипт EvidenceBoardController
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;

            // Если мы попали по доске
            EvidenceBoardController board = hitObject.GetComponentInParent<EvidenceBoardController>();
            if (board != null)
            {
                // Создаем карточку на доске в месте отпускания мыши
                board.AddItemToBoard(data, eventData.position);
            }
        }
    }
}