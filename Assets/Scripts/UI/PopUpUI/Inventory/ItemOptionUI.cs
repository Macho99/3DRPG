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

	Button sellButton;

	protected override void Awake()
	{
		base.Awake();
		buttons["Blocker"].onClick.AddListener(CloseUI);
		buttons["DeleteButton"].onClick.AddListener(CheckDelete);
		equipButton = buttons["EquipButton"];
		equipButton.onClick.AddListener(Equip);
		panel = transforms["ItemOptionPanel"];

		sellButton = buttons["SellButton"];
		sellButton.onClick.AddListener(CheckSell);
		sellButton.gameObject.SetActive(false);
	}

	public void Init(InvenUI invenUI, Item target, Vector2 position)
	{
		this.invenUI = invenUI;
		this.target = target;
		panel.position = position + offset;

		switch(target.ItemType) 
		{
			case Item.Type.Weapon:
				equipButton.gameObject.SetActive(true);
				break;
			case Item.Type.Armor:
				equipButton.gameObject.SetActive(true);
				break;
			case Item.Type.Other:
				equipButton.gameObject.SetActive(false);
				break;
			default:
				equipButton.gameObject.SetActive(true);
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
			case Item.Type.Other:
				break;
			default:
				invenUI?.Equip((ConsumpItem)target);
				break;
		}
		CloseUI();
	}

    private void Update()
    {
        if (GameManager.Dialogue.InteractionNPC.GetComponent<IsTradeAble>() == null)
        {
			sellButton.gameObject.SetActive(false);
        }
		else
		{
            sellButton.gameObject.SetActive(true);
        }
    }

    private void CheckDelete()
	{
		CloseUI();
		if (target != null)
		{
			GameManager.UI.ShowPopUpUI<ItemDeletePopUpUI>("UI/PopUpUI/Inventory/ItemDeletePopUp", false).Init(target);
		}
	}

    private void CheckSell()
    {
		if(GameManager.Dialogue.InteractionNPC == null)
		{
			return;
		}

        CloseUI();
        if (target != null)
        {
            GameManager.UI.ShowPopUpUI<ItemSellPopUp>("UI/PopUpUI/Shop/ItemSellPopUp", false).Init(target);
        }
    }
}
