using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemDeletePopUpUI : PopUpUI
{
	Item target;
	TextMeshProUGUI text;

	protected override void Awake()
	{
		base.Awake();
		buttons["CloseButton"].onClick.AddListener(CloseUI);
		buttons["Blocker"].onClick.AddListener(CloseUI);
		buttons["CancleButton"].onClick.AddListener(CloseUI);
		buttons["AcceptButton"].onClick.AddListener(DeleteItem);
		text = texts["ItemNameText"];
	}

	private void DeleteItem()
	{
		if(target != null)
		{
			GameManager.Inven.DeleteItem(target);
		}
		CloseUI();
	}

	public void Init(Item target)
	{
		this.target = target;
		text.text = target.ItemName;
	}
}
