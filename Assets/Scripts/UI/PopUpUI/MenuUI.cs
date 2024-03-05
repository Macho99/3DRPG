using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : PopUpUI
{
    public void OpenInventory()
    {
        GameManager.UI.ClosePopUpUI();
        GameManager.UI.ShowPopUpUI(GameManager.Inven.inventoryUI);
    }

    public void OpenEquip()
    {
        GameManager.UI.ClosePopUpUI();
        GameManager.UI.ShowPopUpUI<EquipUI>("UI/PopUpUI/Equip/EquipUI");
    }
}
