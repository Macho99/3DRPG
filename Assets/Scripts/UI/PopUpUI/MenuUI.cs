using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : PopUpUI
{
    public void OpenInventory()
    {
        GameManager.UI.ClosePopUpUI();
        GameManager.UI.ShowPopUpUI<InventoryUI>("UI/PopUpUI/Inventory/Inventory");
    }

    public void OpenEquip()
    {
        GameManager.UI.ClosePopUpUI();
        GameManager.UI.ShowPopUpUI<EquipUI>("UI/PopUpUI/Equip/EquipUI");
    }
}
