using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvenUI : PopUpUI
{
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
		RefreshInven();
	}

	private void OnDisable()
	{
		GameManager.Inven.OnItemChange.RemoveListener(RefreshInven);
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
}