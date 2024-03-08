using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Item
{
	public enum Type { Weapon, Armor, Other, RecoveryConsump, }
	[Serializable]
	public enum Rate { Normal, Rare, Epic, Legend}

	protected ItemData itemData;

	public Item(ItemData itemData)
	{
		this.itemData = itemData;
	}

	public string ID { get { return itemData.name; } }
	public Sprite Sprite { get { return itemData.Sprite; } }
	public string ItemName { get { return itemData.ItemName; } }
	public string Summary { get { return itemData.Summary; } }
	public string DetailDesc { get { return itemData.DetailDesc; } }
	public Type ItemType {  get { return itemData.ItemType; } }
	public Rate ItemRate { get { return itemData.Rate; } }
	public int Price { get { return itemData.Price; } }

	public abstract Item Clone();
	public abstract void Use();
}