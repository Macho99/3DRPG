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

	private void Start()
	{
		foreach(ItemData data in itemDataDict.Values)
		{
			print($"{data.ItemName}, { data.Desc }");
		}
	}

	public ItemData GetItemData(string id)
	{
		if(itemDataDict.ContainsKey(id) == false)
		{
			print($"{id}에 해당하는 itemData가 없습니다");
			return null;
		}
		return itemDataDict[id];
	}
}