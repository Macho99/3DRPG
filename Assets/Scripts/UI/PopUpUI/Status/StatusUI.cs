using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : PopUpUI
{
    [SerializeField] private Color[] rateColors;

    InvenType curInvenType;
    ToggleGroup itemSlotGroup;

    Item[] inv;
    ItemSlot[] itemSlots;
    ItemSlot curItemSlot;

    RectTransform itemInfoTrans;
    TextMeshProUGUI itemNameText;
    TextMeshProUGUI itemSummaryText;
    TextMeshProUGUI itemDetailDescText;
    Image itemImage;

    ItemSlot curDragingSlot;
    Image dragInfo;

    ArmorSlot[] armorSlots;
    WeaponSlot[] weaponSlots;
    ConsumpSlot[] consumpSlots;

    TextMeshProUGUI healthPoint;
    TextMeshProUGUI manaPoint;
    TextMeshProUGUI attackPoint;
    TextMeshProUGUI armorPoint;
    TextMeshProUGUI currentMoney;

    public InvenType CurInvenType { get { return curInvenType; } }

    protected override void Awake()
    {
        base.Awake();
        buttons["CloseButton"].onClick.AddListener(CloseUI);
        buttons["ExitButton"].onClick.AddListener(GameManager.UI.MenuToggle);

        itemInfoTrans = transforms["ItemInfo"];
        itemNameText = texts["ItemName"];
        itemSummaryText = texts["ItemSummary"];
        itemDetailDescText = texts["ItemDetailDesc"];
        itemImage = images["ItemImage"];
        itemInfoTrans.gameObject.SetActive(false);

        armorSlots = new ArmorSlot[(int)ArmorType.Size];
        armorSlots[(int)ArmorType.Helmet] = transforms["HelmetSlot"].GetComponent<ArmorSlot>();
        armorSlots[(int)ArmorType.Body] = transforms["BodySlot"].GetComponent<ArmorSlot>();
        armorSlots[(int)ArmorType.Legs] = transforms["LegsSlot"].GetComponent<ArmorSlot>();
        armorSlots[(int)ArmorType.Boots] = transforms["BootsSlot"].GetComponent<ArmorSlot>();
        armorSlots[(int)ArmorType.Cape] = transforms["CapeSlot"].GetComponent<ArmorSlot>();
        armorSlots[(int)ArmorType.Gauntlets] = transforms["GauntletsSlot"].GetComponent<ArmorSlot>();

        weaponSlots = new WeaponSlot[(int)WeaponType.Size];
        weaponSlots[(int)WeaponType.Melee] = transforms["MeleeSlot"].GetComponent<WeaponSlot>();
        weaponSlots[(int)WeaponType.Ranged] = transforms["RangedSlot"].GetComponent<WeaponSlot>();

        consumpSlots = new ConsumpSlot[(int)ConsumpSlotType.Size];
        consumpSlots[(int)ConsumpSlotType.Slot1] = transforms["ConsumpSlot1"].GetComponent<ConsumpSlot>();
        consumpSlots[(int)ConsumpSlotType.Slot2] = transforms["ConsumpSlot2"].GetComponent<ConsumpSlot>();

        dragInfo = images["DragInfo"];
        dragInfo.gameObject.SetActive(false);


        healthPoint = texts["HealthPoint"];
        manaPoint = texts["ManaPoint"];
        attackPoint = texts["AttackPoint"];
        armorPoint = texts["ArmorPoint"];
        currentMoney = texts["CurMoney"];
    }

    private void Update()
    {
        healthPoint.text = $"{GameManager.Stat.CurHP} / {GameManager.Stat.MaxHP}";
        manaPoint.text = $"{GameManager.Stat.CurMP} / {GameManager.Stat.MaxMP}";
        attackPoint.text = $"{GameManager.Stat.AttackMultiplier.ToString("N1")}";
        armorPoint.text = $"{GameManager.Stat.Defence}";
        currentMoney.text = $"{GameManager.Stat.Money}";
    }

    private void SortInven()
    {
        GameManager.Inven.SortInv(curInvenType);
    }

    public void Selected(ItemSlot slot, bool value)
    {
        if (value == true)
        {
            Item item = slot.CurItem;
            curItemSlot = slot;
            itemImage.sprite = item.Sprite;
            itemNameText.text = item.ItemName;
            itemSummaryText.text = item.Summary;
            itemDetailDescText.text = item.DetailDesc;
            itemInfoTrans.gameObject.SetActive(true);
        }
        else
        {
            itemInfoTrans.gameObject.SetActive(false);
            curItemSlot = null;
        }
    }

    public void Equip(ArmorItem armorItem)
    {
        armorSlots[(int)armorItem.ArmorType].Equip(armorItem);
    }

    public void Equip(WeaponItem weaponItem)
    {
        weaponSlots[(int)weaponItem.WeaponType].Equip(weaponItem);
    }

    public void Equip(ConsumpItem consumpItem)
    {
        int emptySlotIdx = -1;
        for (int i = 0; i < (int)ConsumpSlotType.Size; i++)
        {
            if (GameManager.Inven.GetConsumpSlot((ConsumpSlotType)i) == null)
            {
                emptySlotIdx = i;
                break;
            }
        }
        if (emptySlotIdx == -1)
        {
            emptySlotIdx = 0;
        }
        consumpSlots[emptySlotIdx].Equip(consumpItem);
    }

    public Color GetRateColor(Item.Rate rate)
    {
        int idx = (int)rate;
        if (idx >= rateColors.Length)
            idx = rateColors.Length - 1;
        return rateColors[idx];
    }
}
