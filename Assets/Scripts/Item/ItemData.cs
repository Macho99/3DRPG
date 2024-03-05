using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Object/Item Data", order = int.MaxValue)]
public class ItemData : ScriptableObject
{
	[SerializeField] private string itemName;
	[SerializeField] private Sprite sprite;
	[Multiline]
	[SerializeField] private string desc;
	[Multiline]
	[SerializeField] private string detailDesc;

	public string ItemName { get { return itemName; } }
	public Sprite Sprite { get { return sprite; } }
	public string Desc { get { return desc; } }
	public string DetailDesc { get { return detailDesc; } }
}