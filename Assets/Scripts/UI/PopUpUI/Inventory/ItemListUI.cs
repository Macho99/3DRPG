using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemListUI : MonoBehaviour
{
    private Slot[] slots;
    private GameObject slot;

    private void Awake()
    {
        slot = GameManager.Resource.Instantiate<Slot>("UI/PopUpUI/Inventory/Slot").gameObject;

        for (int i = 0; i < 5; i++)
        {
            var makeslot = GameManager.Pool.GetUI(slot);
            makeslot.name = $"Slot_{i}";
            makeslot.transform.parent = transform;
        }
        slots = GetComponentsInChildren<Slot>();
    }

}
