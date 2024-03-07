using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOptionUI : PopUpUI
{
	Vector2 offset = new Vector2(200f, 0f);
	Item target;
	RectTransform panel;

	protected override void Awake()
	{
		base.Awake();
		buttons["Blocker"].onClick.AddListener(CloseUI);
		buttons["DeleteButton"].onClick.AddListener(CheckDelete);
		panel = transforms["ItemOptionPanel"];
	}

	public void Init(Item target, Vector2 position)
	{
		this.target = target;
		panel.position = position + offset;
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
