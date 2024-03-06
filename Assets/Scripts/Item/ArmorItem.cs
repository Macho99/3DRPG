using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ArmorItem : EquipItem
{
	ArmorItemData armorItemData;

	[Serializable]
	public enum ArmorType { Helmet, Body, Leg, Boots, Cape, Gauntlets }
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
}