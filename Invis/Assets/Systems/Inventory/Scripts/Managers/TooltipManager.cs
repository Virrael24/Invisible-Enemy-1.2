using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    public GameObject tooltipWindow;
    public TextMeshProUGUI contentText;
    public Image itemIconImage; // —сылка на иконку в тултипе

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Hide();
    }

    // “ултип теперь принимает целые данные предмета
    public void Show(ItemData item)
    {
        if (item == null) return;

        tooltipWindow.SetActive(true);
        contentText.text = $"<b>{item.itemName}</b>\n\n{item.description}";

        if (itemIconImage != null)
        {
            itemIconImage.sprite = item.icon;
            itemIconImage.enabled = true;
        }
    }

    public void Hide()
    {
        tooltipWindow.SetActive(false);
    }
}