using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public enum WeaponType { Melee, Ranged, Size }
public class WeaponItem : EquipItem
{
	WeaponItemData weaponItemData;

	public WeaponItem(ItemData itemData) : base(itemData)
	{
		weaponItemData = itemData as WeaponItemData;
		if(weaponItemData == null)
		{
			Debug.LogError($"{ID}의 Type이 잘못 설정되어있습니다");
			return;
		}
	}

	public override Item Clone()
	{
		return new WeaponItem(itemData);
	}
	public WeaponType WeaponType { get { return weaponItemData.WeaponType; } }
	public Weapon WeaponPrefab { get { return weaponItemData.WeaponPrefab; } }
}