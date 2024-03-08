using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class ConsumpSlot : MountingSlot
{
	[SerializeField] ConsumpSlotType slotType;

	TextMeshProUGUI amountText;

	protected override void Awake()
	{
		base.Awake();
		amountText = texts["Amount"];
		amountText.gameObject.SetActive(false);
	}

	public override void OnDrop(PointerEventData eventData)
	{
		if (eventData.pointerDrag.TryGetComponent(out ItemSlot otherSlot))
		{
			if (null == otherSlot.CurItem) return;

			if (otherSlot.CurItem is ConsumpItem consumpItem)
			{
				Equip(consumpItem);
			}
		}
	}

	public void Equip(ConsumpItem consumpItem)
	{
		GameManager.Inven.SetConsumpSlot(slotType , consumpItem);
		SetItem(consumpItem);
	}

	public void Refresh()
	{
		SetItem(curItem);
	}

	protected override void SetItem(Item item)
	{
		if (invenUI == null) return;

		base.SetItem(item);
		if(item == null)
		{
			amountText.gameObject.SetActive(false);
		}
		else
		{
			amountText.text = ((MultipleItem)item).Amount.ToString();
			amountText.gameObject.SetActive(true);
		}
	}

	public bool TryUnEquip()
	{
		bool result = GameManager.Inven.InitConsumpSlot(slotType);
		if (result == true)
		{
			SetItem(null);
		}
		return result;
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		_ = TryUnEquip();
	}
}
