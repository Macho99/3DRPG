using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Item
{
	public enum Type { Weapon, Armor, Consump, Other, }

	private string id;
	private ItemData data;
	private Type itemType;

	public Item(string id, Type itemType)
	{
		this.itemType = itemType;
		data = GameManager.Data.GetItemData(id);
	}

	public Sprite Sprite { get { return data.Sprite; } }
	public string ItemName { get { return data.ItemName; } }
	public string Desc { get { return data.Desc; } }
	public string DetailDesc { get { return data.DetailDesc; } }
	public Type ItemType {  get { return itemType; } }

	public abstract Item Clone();

	public abstract void Use();
}