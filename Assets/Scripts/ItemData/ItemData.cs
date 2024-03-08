using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Object/Item Data", order = int.MaxValue)]
public class ItemData : ScriptableObject
{
	[SerializeField] string itemName;
	protected Item.Type itemType = Item.Type.Other;
	[SerializeField] Item.Rate rate = Item.Rate.Normal;
	[SerializeField] Sprite sprite;
	[Multiline]
	[SerializeField] string summary;
	[Multiline]
	[SerializeField] string detailDesc;

	public Item.Type ItemType {  get { return itemType; } }
	public string ItemName { get { return itemName; } }
	public Sprite Sprite { get { return sprite; } }
	public string Summary { get { return summary; } }
	public string DetailDesc { get { return detailDesc; } }
	public Item.Rate Rate { get { return rate; } }
}