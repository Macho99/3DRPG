using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : PopUpUI
{
	protected override void Awake()
	{
		base.Awake();
        buttons["InventoryButton"].onClick.AddListener(OpenInventory);
	}

	public void OpenInventory()
    {
        GameManager.UI.ShowPopUpUI<InvenUI>("UI/PopUpUI/Inventory/Inventory");
    }

    public void OpenEquip()
    {
        //GameManager.UI.ShowPopUpUI<EquipUI>("UI/PopUpUI/Equip/EquipUI");
    }
}
