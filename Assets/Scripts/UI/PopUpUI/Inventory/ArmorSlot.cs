using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArmorSlot : MountingSlot
{
	[SerializeField] ArmorType armorType;

	protected override void Awake()
	{
		base.Awake();
	}

	public override void OnDrop(PointerEventData eventData)
	{
		if (eventData.pointerDrag.TryGetComponent(out ItemSlot otherSlot))
		{
			if (null == otherSlot.CurItem) return;

			if (otherSlot.CurItem is ArmorItem armorItem)
			{
				if(armorItem.ArmorType == armorType)
				{
					Equip(armorItem);
				}
			}
		}
	}

	public void Equip(ArmorItem armorItem)
	{
		GameManager.Inven.SetArmorSlot(armorItem);
		SetItem(armorItem);
	}

	public bool TryUnEquip()
	{
		bool result = GameManager.Inven.InitArmorSlot(armorType);
		if (result == true)
		{
			SetItem(null);
		}
		return result;
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		_ = TryUnEquip();
    }
}