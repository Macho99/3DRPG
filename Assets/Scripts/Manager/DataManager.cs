using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DataManager : MonoBehaviour
{
	Dictionary<string, ItemData> itemDataDict = new();

	private void Awake()
	{
		ItemData[] itemDatas = Resources.LoadAll<ItemData>("Data/ItemData");
		foreach (ItemData data in itemDatas)
		{
			if(itemDataDict.ContainsKey(data.name) == true)
			{
				print($"{data.name}이 중복됩니다");
				continue;
			}
			itemDataDict.Add(data.name, data);
		}
	}

	public Item GetItem(string id, int amount = 1)
	{
		ItemData itemData = itemDataDict[id];
		Item item = null;

		switch (itemData.ItemType)
		{
			case Item.Type.Weapon:
				item = new WeaponItem(itemData);
				return item;
			case Item.Type.Armor:
				item = new ArmorItem(itemData);
				return item;
			case Item.Type.Other:
				item = new OtherItem(itemData, amount);
				break;
			case Item.Type.RecoveryConsump:
				item = new RecoveryConsumpItem(itemData, amount);
				break;
			default:
				Debug.LogError($"{itemData.ItemType}에 해당하는 switch문이 빠져있습니다");
				break;
		}

		return item;
	}
}