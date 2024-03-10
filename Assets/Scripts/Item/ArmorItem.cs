using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public enum ArmorType { Helmet, Body, Legs, Boots, Cape, Gauntlets, Size }

[Serializable]
public struct ArmorStat
{
	public int maxHP;
	public int maxMP;
	public int recoveryHP;
	public int recoveryMP;
	public int defence;
	public int stunResistance;
}

public class ArmorItem : EquipItem
{
	ArmorItemData armorItemData;

	public ArmorItem(ItemData itemData) : base(itemData)
	{
		armorItemData = itemData as ArmorItemData;
		if (armorItemData == null)
		{
			Debug.LogError($"{ID}의 Type이 잘못 설정되어있습니다");
			return;
		}
	}

	public override Item Clone()
	{
		return new ArmorItem(itemData);
	}

	public ArmorType ArmorType { get { return armorItemData.ArmorType; } }
	public string ArmorSkinName { get { return armorItemData.ArmorSkinName; } }
	public ArmorStat ArmorStat { get { return armorItemData.ArmorStat; } }
}