using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipListUI : MonoBehaviour
{
    public Slot[] weaponSlot;
    public Slot[] armorSlot;
    public Slot[] expSlot;
    private GameObject slotParent;
    private GameObject slot;

    private void Awake()
    {
        slot = GameManager.Resource.Instantiate<Slot>("UI/PopUpUI/Equip/EquipSlot").gameObject;
        slotParent = transform.Find("ListParent").gameObject;
        for (int i = 0; i < 4; i++)
        {
            var makeslot = GameManager.Pool.GetUI(slot);
            makeslot.name = $"{gameObject.name}_Slot_{i}";
            makeslot.transform.parent = slotParent.transform;
        }
    }
}
