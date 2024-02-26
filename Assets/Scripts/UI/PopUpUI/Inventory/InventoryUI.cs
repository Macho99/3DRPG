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

    public RectTransform showItemArea;

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemExplain;
    public TextMeshProUGUI itemStatus;
    public Image itemIcon;

    protected override void Awake()
    {
        GameManager.Inven.MakeSlotParent(this);
    }

    private void Update()
    {
        switch (onToggle.gameObject.name)
        {
            case "ShowAll":
                GameManager.Inven.weaponContent.gameObject.SetActive(true);
                GameManager.Inven.armorContent.gameObject.SetActive(true);
                GameManager.Inven.consumContent.gameObject.SetActive(true);
                break;
            case "ShowWeapon":
                GameManager.Inven.weaponContent.gameObject.SetActive(true);
                GameManager.Inven.armorContent.gameObject.SetActive(false);
                GameManager.Inven.consumContent.gameObject.SetActive(false);
                break;
            case "ShowArmor":
                GameManager.Inven.weaponContent.gameObject.SetActive(false);
                GameManager.Inven.armorContent.gameObject.SetActive(true);
                GameManager.Inven.consumContent.gameObject.SetActive(false);
                break;
            case "ShowConsum":
                GameManager.Inven.weaponContent.gameObject.SetActive(false);
                GameManager.Inven.armorContent.gameObject.SetActive(false);
                GameManager.Inven.consumContent.gameObject.SetActive(true);
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
            itemName.text = GameManager.Inven.focusSlot.item.itemName;
            itemExplain.text = GameManager.Inven.focusSlot.item.itemExplain;
            itemStatus.text = GameManager.Inven.focusSlot.item.itemStatus;
            itemIcon.sprite = GameManager.Inven.focusSlot.item.itemIcon;
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
