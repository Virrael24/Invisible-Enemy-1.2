using UnityEngine;

public class EvidenceListUI : MonoBehaviour
{
    public InventorySO inventory;
    public GameObject evidenceItemPrefab; // Префаб кнопки в списке

    void Start()
    {
        inventory.OnInventoryChanged += RefreshList;
        RefreshList();
    }

    void RefreshList()
    {
        // Удаляем старые кнопки
        foreach (Transform child in transform) Destroy(child.gameObject);

        // Создаем новые для каждого предмета в инвентаре
        foreach (var item in inventory.slots)
        {
            if (item != null)
            {
                GameObject go = Instantiate(evidenceItemPrefab, transform);
                go.GetComponent<EvidenceEntry>().Setup(item);
            }
        }
    }
}