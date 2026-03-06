using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public float itemID;
    [TextArea] public string description;
    public Sprite icon;
    // Можно добавить ID для систем сохранений в файл
    public string id = System.Guid.NewGuid().ToString();
}