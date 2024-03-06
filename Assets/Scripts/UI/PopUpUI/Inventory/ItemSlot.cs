using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
	InvenUI invenUI;
	[SerializeField] Image itemImage;
	[SerializeField] TextMeshProUGUI amountText;
	Toggle toggle;

	Item item;

	private void Awake()
	{
		toggle = GetComponent<Toggle>();
	}

	public void Init(InvenUI invenUI)
	{
		this.invenUI = invenUI;
		toggle.group = transform.parent.GetComponent<ToggleGroup>();
	}

	public void SetItem(Item item)
	{
		this.item = item;
		if(item == null)
		{
			itemImage.sprite = null;
			amountText.gameObject.SetActive(false);
			toggle.interactable = false;
			return;
		}

		itemImage.sprite = item.Sprite;
		if(item.ItemType == Item.Type.Other || item.ItemType == Item.Type.HPConsump)
		{
			MultipleItem multiple = (MultipleItem) item;
			amountText.text = multiple.Amount.ToString();
			amountText.gameObject.SetActive(true);
		}
		else
		{
			amountText.gameObject.SetActive(false);
		}
		toggle.interactable = true;
	}
}
