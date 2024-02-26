using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquipUI : PopUpUI
{
    public RectTransform weaponContent;
    public RectTransform armorContent;
    public RectTransform consumContent;

    private List<EquipSlot> weaponList = new List<EquipSlot>();
    private List<EquipSlot> armorList = new List<EquipSlot>();
    private List<EquipSlot> consumList = new List<EquipSlot>();

    private void Start()
    {
        MakeSlotsList(weaponList, weaponContent);

        MakeSlotsList(armorList, armorContent);

        MakeSlotsList(consumList, consumContent);
    }

    private void MakeSlotsList(List<EquipSlot> equipList, RectTransform equipTrans)
    {
        for (int i = 0; i < 4; i++)
        {
            var makeSlot = GameManager.Resource.Instantiate<EquipSlot>("UI/PopUpUI/Equip/EquipSlot", equipTrans.transform);
            makeSlot.name = $"{equipTrans.gameObject.name}_{i}";
            equipList.Add(makeSlot);
            if (equipTrans.gameObject.name == "WeaponList")
            {
                makeSlot.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            }
            if (equipTrans.gameObject.name == "ArmorList")
            {
                makeSlot.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            }
        }
    }

    public void CloseThis()
    {
        GameManager.UI.ClearPopUpUI();
        GameManager.UI.ClearWindowUI();
        GameManager.UI.ShowPopUpUI(GameManager.UI.menu);
    }
}
