using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : PopUpUI
{
	protected override void Awake()
	{
		base.Awake();


	}

	public void OpenInventory()
    {
        //GameManager.UI.ShowPopUpUI<InventoryUI>("UI/PopUpUI/Inventory/InventoryDisplay");
    }

    public void OpenEquip()
    {
        //GameManager.UI.ShowPopUpUI<EquipUI>("UI/PopUpUI/Equip/EquipUI");
    }
}
