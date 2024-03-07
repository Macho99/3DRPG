using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine;

public class WeaponSlot : MountingSlot
{
	[SerializeField] WeaponType weaponType;

	protected override void Awake()
	{
		base.Awake();
	}

	public override void OnDrop(PointerEventData eventData)
	{
		if (eventData.pointerDrag.TryGetComponent(out ItemSlot otherSlot))
		{
			if (null == otherSlot.CurItem) return;

			if (otherSlot.CurItem is WeaponItem weaponItem)
			{
				if (weaponItem.WeaponType == weaponType)
				{
					Equip(weaponItem);
				}
			}
		}
	}

	public void Equip(WeaponItem weaponItem)
	{
		GameManager.Inven.SetWeaponSlot(weaponItem);
		SetItem(weaponItem);
	}

	public bool TryUnEquip()
	{
		bool result = GameManager.Inven.InitWeaponSlot(weaponType);
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