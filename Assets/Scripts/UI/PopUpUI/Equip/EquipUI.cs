using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipUI : PopUpUI
{
    public void CloseThis()
    {
        GameManager.UI.ClosePopUpUI();
        GameManager.UI.ShowPopUpUI(GameManager.UI.menu);
    }
}
