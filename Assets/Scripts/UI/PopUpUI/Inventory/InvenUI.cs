using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.UI;

public class InvenUI : PopUpUI
{
	[SerializeField] private Color[] rateColors;
	[SerializeField] private Color redColor;
	[SerializeField] private Color greenColor;

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
	Image armorInfo;
	TextMeshProUGUI armorNameText;
	TextMeshProUGUI armorStatText;
	TextMeshProUGUI[] comparisonTexts = new TextMeshProUGUI[6];

	ArmorSlot[] armorSlots;
	WeaponSlot[] weaponSlots;
	ConsumpSlot[] consumpSlots;

	bool started;

	public InvenType CurInvenType { get { return curInvenType; } }

	protected override void Awake()
	{
		base.Awake();
		buttons["CloseButton"].onClick.AddListener(CloseUI);
		buttons["ExitButton"].onClick.AddListener(GameManager.UI.MenuToggle);
		buttons["SortButton"].onClick.AddListener(SortInven);

		toggles["EquipToggle"].onValueChanged.AddListener(EquipToggle);
		toggles["ConsumpToggle"].onValueChanged.AddListener(ConsumpToggle);
		toggles["OtherToggle"].onValueChanged.AddListener(OtherToggle);

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
		weaponSlots[(int)WeaponType.Ranged] = transforms["RangedSlot"].GetComponent <WeaponSlot>();

		consumpSlots = new ConsumpSlot[(int)ConsumpSlotType.Size];
		consumpSlots[(int)ConsumpSlotType.Slot1] = transforms["ConsumpSlot1"].GetComponent<ConsumpSlot>();
		consumpSlots[(int)ConsumpSlotType.Slot2] = transforms["ConsumpSlot2"].GetComponent<ConsumpSlot>();

		armorInfo = images["ArmorInfo"];
		armorInfo.gameObject.SetActive(false);
		armorNameText = texts["ArmorName"];
		armorStatText = texts["ArmorStatText"];
		comparisonTexts[0] = texts["MaxHPText"];
		comparisonTexts[1] = texts["MaxMPText"];
		comparisonTexts[2] = texts["RecoveryHPText"];
		comparisonTexts[3] = texts["RecoveryMPText"];
		comparisonTexts[4] = texts["DefenceText"];
		comparisonTexts[5] = texts["StunResistText"];

		dragInfo = images["DragInfo"];
		dragInfo.gameObject.SetActive(false);

		itemSlotGroup = transforms["ItemSlotContent"].GetComponent<ToggleGroup>();
		itemSlots = new ItemSlot[GameManager.Inven.InvSize];
		for(int i = 0; i < itemSlots.Length; i++)
		{
			ItemSlot slot = GameManager.Resource.Instantiate<ItemSlot>(
				"UI/PopUpUI/Inventory/ItemSlot", itemSlotGroup.transform, false);
			slot.name = $"itemSlot{i}";
			itemSlots[i] = slot;
			slot.Init(this, i);
		}
		curInvenType = InvenType.Equip;
	}

	private void OnEnable()
	{
		GameManager.Inven.OnItemChange.AddListener(RefreshInven);
		if(started == true)
			RefreshInven();
	}

	private void OnDisable()
	{
		GameManager.Inven.OnItemChange.RemoveListener(RefreshInven);
	}

	private void Start()
	{
		started = true;
		RefreshInven();
	}

	private void RefreshInven()
	{
		switch(curInvenType)
		{
			case InvenType.Equip:
				inv = GameManager.Inven.GetEquipInv();
				break;
			case InvenType.Consump:
				inv = GameManager.Inven.GetConsumpInv();
				break;
			case InvenType.Other:
				inv = GameManager.Inven.GetOtherInv();
				break;
		}

		for(int i=0; i<itemSlots.Length;i++)
		{
			itemSlots[i].SetItem(inv[i]);
		}

		for(int i = 0; i < consumpSlots.Length; i++)
		{
			consumpSlots[i].Refresh();
		}

		itemSlotGroup.SetAllTogglesOff();
	}

	private void EquipToggle(bool value)
	{
		if (value == true)
		{
			curInvenType = InvenType.Equip;
		}
		RefreshInven();
	}

	private void ConsumpToggle(bool value)
	{
		if (value == true)
		{
			curInvenType = InvenType.Consump;
		}
		RefreshInven();
	}

	private void OtherToggle(bool value)
	{
		if (value == true)
		{
			curInvenType = InvenType.Other;
		}
		RefreshInven();
	}

	private void SortInven()
	{
		GameManager.Inven.SortInv(curInvenType);
	}

	public void Selected(ItemSlot slot, bool value)
	{
		if(value == true)
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

	public void SlotDragStart(ItemSlot slot)
	{
		curDragingSlot = slot;
		dragInfo.gameObject.SetActive(true);
		dragInfo.sprite = slot.CurItem.Sprite;
	}

	public void SlotDragMove(ItemSlot slot, Vector3 position)
	{
		dragInfo.rectTransform.position = position;
	}

	public void SlotDragEnd(ItemSlot slot)
	{
		dragInfo.gameObject.SetActive(false);
	}

	public void SlotPointerEnter(ArmorItem armorItem)
	{
		armorNameText.text = armorItem.ItemName;

		StringBuilder sb = new StringBuilder();
		ArmorStat slotStat = armorItem.ArmorStat;
		sb.Append($"{slotStat.maxHP}\n");
		sb.Append($"{slotStat.maxMP}\n");
		sb.Append($"{slotStat.recoveryHP}\n");
		sb.Append($"{slotStat.recoveryMP}\n");
		sb.Append($"{slotStat.defence}\n");
		sb.Append($"{slotStat.stunResistance}\n");
		armorStatText.text = sb.ToString();

		ArmorItem equipedArmor = GameManager.Inven.GetArmorSlot(armorItem.ArmorType);
		ArmorStat equipedStat;
		if (equipedArmor == null)
		{
			equipedStat = new();
		}
		else
		{
			equipedStat = equipedArmor.ArmorStat;
		}

		SetComparisonText(comparisonTexts[0], equipedStat.maxHP, slotStat.maxHP);
		SetComparisonText(comparisonTexts[1], equipedStat.maxMP, slotStat.maxMP);
		SetComparisonText(comparisonTexts[2], equipedStat.recoveryHP, slotStat.recoveryHP);
		SetComparisonText(comparisonTexts[3], equipedStat.recoveryMP, slotStat.recoveryMP);
		SetComparisonText(comparisonTexts[4], equipedStat.defence, slotStat.defence);
		SetComparisonText(comparisonTexts[5], equipedStat.stunResistance, slotStat.stunResistance);

		armorInfo.gameObject.SetActive(true);
	}

	private void SetComparisonText(TextMeshProUGUI targetText, int equiped, int itemSlot)
	{
		StringBuilder sb = new StringBuilder();
		Color color = Color.white;
		if(equiped == itemSlot)
		{
			sb.Append(" ");
		}
		else
		{
			sb.Append('(');
			if (equiped > itemSlot)
			{
				color = Color.red;
				sb.Append('▼');
			}
			else if(equiped < itemSlot)
			{
				color = Color.green;
				sb.Append('▲');
			}
			sb.Append(Mathf.Abs(equiped - itemSlot).ToString());
			sb.Append(')');
		}

		targetText.text = sb.ToString();
		targetText.color = color;
	}

	public void SlotPointerMove(Vector3 position)
	{
		position += new Vector3(150f, 0f);
		armorInfo.rectTransform.position = position;
	}

	public void SlotPointerExit()
	{
		armorInfo.gameObject.SetActive(false);
	}

	public void Equip(ArmorItem armorItem)
	{
		armorSlots[(int) armorItem.ArmorType].Equip(armorItem);
		SlotPointerExit();
	}

	public void Equip(WeaponItem weaponItem)
	{
		weaponSlots[(int) weaponItem.WeaponType].Equip(weaponItem);
	}

	public void Equip(ConsumpItem consumpItem)
	{
		int emptySlotIdx = -1;
		for(int i = 0; i < (int) ConsumpSlotType.Size; i++)
		{
			if(GameManager.Inven.GetConsumpSlot((ConsumpSlotType)i) == null)
			{
				emptySlotIdx = i;
				break;
			}
		}
		if(emptySlotIdx == -1)
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