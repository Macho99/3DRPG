using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemOptionUI : PopUpUI
{
	Vector2 offset = new Vector2(200f, 0f);
	Button equipButton;
	Item target;
	RectTransform panel;
	InvenUI invenUI;

	protected override void Awake()
	{
		base.Awake();
		buttons["Blocker"].onClick.AddListener(CloseUI);
		buttons["DeleteButton"].onClick.AddListener(CheckDelete);
		equipButton = buttons["EquipButton"];
		equipButton.onClick.AddListener(Equip);
		panel = transforms["ItemOptionPanel"];
	}

	public void Init(InvenUI invenUI, Item target, Vector2 position)
	{
		this.invenUI = invenUI;
		this.target = target;
		panel.position = position + offset;

		switch(target.ItemType) 
		{
			//case Item.Type.Weapon:
			//	break;
			//case Item.Type.Armor:
			//	break;
			case Item.Type.Other:
				equipButton.gameObject.SetActive(false);
				break;
			default:
				break;
		}
	}

	private void Equip()
	{
		switch (target.ItemType) {
			case Item.Type.Armor:
				invenUI?.Equip((ArmorItem)target);
				break;
			case Item.Type.Weapon:
				invenUI?.Equip((WeaponItem)target);
				break;
		}
		CloseUI();
	}


	private void CheckDelete()
	{
		CloseUI();
		if (target != null)
		{
			GameManager.UI.ShowPopUpUI<ItemDeletePopUpUI>("UI/PopUpUI/Inventory/ItemDeletePopUp", false).Init(target);
		}
	}
}
