using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : PopUpUI
{
	protected override void Awake()
	{
		base.Awake();
        buttons["InventoryButton"].onClick.AddListener(OpenInventory);
        buttons["StatusButton"].onClick.AddListener(OpenStatus);
        buttons["Blocker"].onClick.AddListener(GameManager.UI.MenuToggle);
	}

	public void OpenInventory()
    {
        GameManager.UI.ShowPopUpUI<InvenUI>("UI/PopUpUI/Inventory/Inventory");
    }

    public void OpenStatus()
    {
        GameManager.UI.ShowPopUpUI<StatusUI>("UI/PopUpUI/Status/Status");
    }
}
