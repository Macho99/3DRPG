using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class InvenUI : PopUpUI
{
	InvenType curInvenType;
	ToggleGroup itemSlotGroup;

	Item[] inv;
	ItemSlot[] itemSlots;

	protected override void Awake()
	{
		base.Awake();
		buttons["CloseButton"].onClick.AddListener(CloseUI);
		toggles["EquipToggle"].onValueChanged.AddListener(EquipToggle);
		toggles["ConsumpToggle"].onValueChanged.AddListener(ConsumpToggle);
		toggles["OtherToggle"].onValueChanged.AddListener(OtherToggle);
		itemSlotGroup = transforms["ItemSlotContent"].GetComponent<ToggleGroup>();
		itemSlots = new ItemSlot[GameManager.Inven.InvSize];
		for(int i = 0; i < itemSlots.Length; i++)
		{
			ItemSlot slot = GameManager.Resource.Instantiate<ItemSlot>(
				"UI/PopUpUI/Inventory/ItemSlot", itemSlotGroup.transform, false);
			slot.Init(this);
			itemSlots[i] = slot;
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
		var toggles = itemSlotGroup.ActiveToggles();
		foreach(Toggle toggle in toggles)
		{
			toggle.isOn = false;
		}
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
}