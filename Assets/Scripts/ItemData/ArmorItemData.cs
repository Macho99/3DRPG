using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor Item Data", menuName = "Scriptable Object/Armor Item Data", order = int.MaxValue)]
public class ArmorItemData : ItemData
{
	[SerializeField] ArmorType armorType;
	[SerializeField] string armorSkinName;

	private void Awake()
	{
		itemType = Item.Type.Armor;
	}

	public ArmorType ArmorType { get { return armorType; } }
	public string ArmorSkinName { get { return armorSkinName; } }
}