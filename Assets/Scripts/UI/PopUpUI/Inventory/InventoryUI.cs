using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : PopUpUI
{
    public ToggleGroup tabToggle;
    public Toggle onToggle{
        get { return tabToggle.ActiveToggles().FirstOrDefault(); } }
    public Toggle defaultToggle;

    public RectTransform showItemArea;

    public ItemListUI weaponList;
    public ItemListUI armorList;
    public ItemListUI expList;

    private void Start()
    {
        weaponList = GameManager.Resource.Instantiate<ItemListUI>("UI/PopUpUI/Inventory/ItemList");
        weaponList.gameObject.name = "WeaponList";
        weaponList.transform.parent = showItemArea.transform;
        
        armorList = GameManager.Resource.Instantiate<ItemListUI>("UI/PopUpUI/Inventory/ItemList");
        armorList.gameObject.name = "ArmorList";
        armorList.transform.parent = showItemArea.transform;
        
        expList = GameManager.Resource.Instantiate<ItemListUI>("UI/PopUpUI/Inventory/ItemList");
        expList.gameObject.name = "EXPList";
        expList.transform.parent = showItemArea.transform;
    }

    private void Update()
    {
        switch (onToggle.gameObject.name)
        {
            case "ShowAll":
                weaponList.gameObject.SetActive(true);
                armorList.gameObject.SetActive(true);
                expList.gameObject.SetActive(true);
                break;
            case "ShowWeapon":
                weaponList.gameObject.SetActive(true);
                armorList.gameObject.SetActive(false);
                expList.gameObject.SetActive(false);
                break;
            case "ShowArmor":
                weaponList.gameObject.SetActive(false);
                armorList.gameObject.SetActive(true);
                expList.gameObject.SetActive(false);
                break;
            case "ShowExp":
                weaponList.gameObject.SetActive(false);
                armorList.gameObject.SetActive(false);
                expList.gameObject.SetActive(true);
                break;
        }
    }

    private void OnEnable()
    {
        foreach(Transform child in tabToggle.GetComponentInChildren<Transform>())
        {
            child.GetComponent<Toggle>().isOn = false;
        }
    }

    public void CloseThis()
    {
        GameManager.UI.ClosePopUpUI();
        GameManager.UI.ShowPopUpUI(GameManager.UI.menu);
    }
}
