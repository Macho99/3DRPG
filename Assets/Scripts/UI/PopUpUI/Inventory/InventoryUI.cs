using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : PopUpUI
{
    public ToggleGroup tabToggle;
    public Toggle onToggle{
        get { return tabToggle.ActiveToggles().FirstOrDefault(); } }
    public Toggle defaultToggle;

    public RectTransform weaponDisplay;
    public RectTransform armorDisplay;
    public RectTransform consumDisplay;
    public InventorySlot slot;

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemExplain;
    public TextMeshProUGUI itemStatus;
    public Image itemIcon;

    private void Update()
    {
        switch (onToggle.gameObject.name)
        {
            case "ShowAll":
                weaponDisplay.gameObject.SetActive(true);
                armorDisplay.gameObject.SetActive(true);
                consumDisplay.gameObject.SetActive(true);
                break;
            case "ShowWeapon":
                weaponDisplay.gameObject.SetActive(true);
                armorDisplay.gameObject.SetActive(false);
                consumDisplay.gameObject.SetActive(false);
                break;
            case "ShowArmor":
                weaponDisplay.gameObject.SetActive(false);
                armorDisplay.gameObject.SetActive(true);
                consumDisplay.gameObject.SetActive(false);
                break;
            case "ShowConsum":
                weaponDisplay.gameObject.SetActive(false);
                armorDisplay.gameObject.SetActive(false);
                consumDisplay.gameObject.SetActive(true);
                break;
        }

        if (GameManager.Inven.focusSlot == null)
        {
            itemName.text = "-";
            itemExplain.text = "-";
            itemStatus.text = "-";
            itemIcon.sprite = null;
        }
        else if(GameManager.Inven.focusSlot != null && GameManager.Inven.focusSlot.item == null)
        {
            itemName.text = "-";
            itemExplain.text = "-";
            itemStatus.text = "-";
            itemIcon.sprite = null;
        }
        else if(GameManager.Inven.focusSlot != null && GameManager.Inven.focusSlot.item != null)
        {
            itemName.text = GameManager.Inven.focusSlot.item.Name;
            itemExplain.text = GameManager.Inven.focusSlot.item.Description;
            itemStatus.text = GameManager.Inven.focusSlot.item.Summary;
            itemIcon.sprite = GameManager.Inven.focusSlot.item.Icon;
        }
    }

    private void OnEnable()
    {
        // 인벤토리 오픈 시 초기화
        foreach (Transform child in tabToggle.GetComponentInChildren<Transform>())
        {
            child.GetComponent<Toggle>().isOn = false;
        }
    }

    public void CloseThis()
    {
        GameManager.UI.ClearPopUpUI();
        GameManager.UI.ClearWindowUI();
        GameManager.UI.ShowPopUpUI(GameManager.UI.menu);
    }
}
