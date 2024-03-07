using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum InvenType { Equip, Consump, Other }
public class InventoryManager : MonoBehaviour
{
	EquipItem[] equipInv;
	ConsumpItem[] consumpInv;
	OtherItem[] otherInv;
	const int invSize = 20;
	public int InvSize {  get { return invSize; } }
	[HideInInspector] public UnityEvent<Item> OnItemGet = new UnityEvent<Item>();
	[HideInInspector] public UnityEvent<Item> OnItemDelete = new UnityEvent<Item>();
	[HideInInspector] public UnityEvent OnItemChange = new UnityEvent();

	private void Awake()
	{
		OnItemGet.AddListener(ItemChange);
		OnItemDelete.AddListener(ItemChange);
		equipInv = new EquipItem[invSize];
		consumpInv = new ConsumpItem[invSize];
		otherInv = new OtherItem[invSize];
	}

	private void Start()
	{
		AddItem(GameManager.Data.GetItem("KnightHelmet"));
		AddItem(GameManager.Data.GetItem("BasicKatana"));
		AddItem(GameManager.Data.GetItem("BasicKatana"));
		AddItem(GameManager.Data.GetItem("RedApple"));
	}

	private void ItemChange()
	{
		OnItemChange?.Invoke();
	}

	private void ItemChange(Item item)
	{
		OnItemChange?.Invoke();
	}

	private Item[] GetInv(Item item)
	{
		Item[] inv;
		switch (item.ItemType)
		{
			case Item.Type.Weapon:
			case Item.Type.Armor:
				inv = equipInv;
				break;
			case Item.Type.Other:
				inv = otherInv;
				break;
			case Item.Type.HPConsump:
			default:
				inv = consumpInv;
				break;
		}

		return inv;
	}

	private Item[] GetInv(InvenType invType)
	{
		Item[] inv;
		switch (invType)
		{
			case InvenType.Consump:
				inv = consumpInv;
				break;
			case InvenType.Other:
				inv = otherInv;
				break;
			case InvenType.Equip:
			default:
				inv = equipInv;
				break;
		}
		return inv;
	}

	//public void Refresh(InvenType invType, int idx, Item item)
	//{
	//	Item[] inv = GetInv(invType);

	//	if (idx >= inv.Length)
	//	{
	//		Debug.LogError($"{idx}는 ");
	//		return;
	//	}
	//	inv[idx] = item;
	//}

	public EquipItem[] GetEquipInv()
	{
		return equipInv;
	}

	public ConsumpItem[] GetConsumpInv()
	{
		return consumpInv;
	}

	public OtherItem[] GetOtherInv()
	{
		return otherInv;
	}

	private bool AddItem(Item item)
	{
		Item[] inv;
		switch (item.ItemType)
		{
			case Item.Type.Weapon:
			case Item.Type.Armor:
				inv = equipInv;
				break;
			default:
				return AddMultipleInv((MultipleItem) item);
		}

		int idx = GetEmptySlot(inv);
		if (idx == -1)
		{
			//GameManager.UI.InvenFullAlarm();
			return false;
		}

		inv[idx] = item;
		return true;
	}

	private int GetEmptySlot(Item[] inv)
	{
		int idx = -1;
		for (int i = 0; i < inv.Length; i++)
		{
			if (inv[i] == null)
			{
				idx = i;
				break;
			}
		}
		return idx;
	}

	private bool AddMultipleInv(MultipleItem newItem)
	{
		MultipleItem[] inv;
		switch (newItem.ItemType)
		{
			case Item.Type.Other:
				inv = otherInv;
				break;
			case Item.Type.HPConsump:
			default:
				inv = consumpInv;
				break;
		}

		int emptyIdx = inv.Length;
		bool find = false;
		for (int i = 0; i < inv.Length; i++)
		{
			if (null == inv[i])
				emptyIdx = Mathf.Min(i, emptyIdx);
			else if (inv[i].ID == newItem.ID)
			{
				find = true;
				inv[i].AddAmount(newItem.Amount);
				break;
			}
		}

		if (false == find)
		{
			if (inv.Length == emptyIdx)
			{
				GameManager.UI.InvenFullAlarm();
				return false;
			}

			inv[emptyIdx] = newItem;
		}
		OnItemGet?.Invoke(newItem);
		return true;
	}

	public void SortInv(InvenType invenType)
	{
		Item[] inv = GetInv(invenType);
		bool changed = false;
		bool finish = false;

		for (int i = 0; i < inv.Length; i++)
		{
			if (true == finish)
			{
				break;
			}
			if (null != inv[i])
			{
				continue;
			}

			for (int j = i + 1; j < inv.Length; j++)
			{
				if (null != inv[j])
				{
					Item temp = inv[j];
					inv[j] = inv[i];
					inv[i] = temp;
					changed = true;
					break;
				}

				if (j == inv.Length - 1)
				{
					finish = true;
				}
			}
		}

		if (true == changed)
		{
			ItemChange();
			return;
		}

		Array.Sort(inv, (a, b) =>
		{
			if (a == null)
			{
				if (b == null)
					return 0;
				else
					return 1;
			}
			else
			{
				if (b == null)
					return -1;
				else
					return string.Compare(a.ID, b.ID);
			}
		});
		ItemChange();
	}

	public void DeleteItem(Item item)
	{
		FindItem(item, out Item[] inv, out int idx);

		inv[idx] = null;
		OnItemDelete?.Invoke(item);
	}

	//public void ItemAmountChanged(Item item)
	//{
	//	if (item is MultipleItem multi)
	//	{
	//		if (0 == multi.Amount)
	//		{
	//			int idx;
	//			FindItem(item, out idx);
	//			if (-1 == idx)
	//			{
	//				Debug.LogError($"�κ��丮�� {item.Name}�� �����ϴ�!");
	//				return;
	//			}
	//			DeleteItem(item.Type, idx);
	//			return;
	//		}
	//	}

	//	onItemAmountChanged?.Invoke(item);
	//}

	private void FindItem(Item item, out Item[] inv, out int idx)
	{
		inv = GetInv(item);
		idx = -1;
		for (int i = 0; i < inv.Length; i++)
		{
			if (inv[i] == item)
			{
				idx = i;
				return;
			}
		}
		return;
	}

	public void SwapItem(Item item1, Item item2)
	{
		FindItem(item1, out Item[] inv1, out int idx1);
		FindItem(item2, out Item[] inv2, out int idx2);

		if(inv1 != inv2)
		{
			Debug.LogError($"둘이 다른 인벤토리인데 스왑하려고합니다");
			return;
		}

		inv1[idx1] = item2;
		inv1[idx2] = item1;
		ItemChange();
	}

	public void SwapItem(InvenType invenType, int idx1, int idx2)
	{
		Item[] inv = GetInv(invenType);

		Item temp = inv[idx1];
		inv[idx1] = inv[idx2];
		inv[idx2] = temp;
		ItemChange();
	}
}
