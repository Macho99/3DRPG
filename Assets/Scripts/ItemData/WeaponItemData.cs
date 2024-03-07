using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Item Data", menuName = "Scriptable Object/Weapon Item Data", order = int.MaxValue)]
public class WeaponItemData : ItemData
{
	[SerializeField] private Weapon weaponPrefab;

	private void Awake()
	{
		itemType = Item.Type.Weapon;
	}

	public Weapon WeaponPrefab { get { return weaponPrefab; } }
}