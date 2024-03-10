using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
	IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
	InvenUI invenUI;
	[SerializeField] Image itemImage;
	[SerializeField] TextMeshProUGUI amountText;
	Image image;
	Toggle toggle;

	Item curItem;
	int slotIdx;
	RectTransform rectTransform;
	float lastLeftClickTime = 0f;
	float doubleClickTime = 0.3f;


	public Item CurItem { get { return curItem; } }
	public int SlotIdx { get { return slotIdx; } }
	public RectTransform RectTransform { get { return rectTransform; } }

	private void Awake()
	{
		toggle = GetComponent<Toggle>();
		rectTransform = GetComponent<RectTransform>();
		image = GetComponent<Image>();
	}

	public void Init(InvenUI invenUI, int slotIdx)
	{
		this.invenUI = invenUI;
		this.slotIdx = slotIdx;
		toggle.group = transform.parent.GetComponent<ToggleGroup>();
		toggle.onValueChanged.AddListener(Selected);
	}

	private void Selected(bool value)
	{
		invenUI.Selected(this, value);
	}

	public void SetItem(Item item)
	{
		Color color;
		this.curItem = item;
		if(item == null)
		{
			itemImage.sprite = null;
			color = itemImage.color;
			color.a = 0f;
			itemImage.color = color;
			image.color = invenUI.GetRateColor(Item.Rate.Normal);
			amountText.gameObject.SetActive(false);
			toggle.interactable = false;
			return;
		}

		itemImage.sprite = item.Sprite;
		color = itemImage.color;
		color.a = 1f;
		itemImage.color = color;
		image.color = invenUI.GetRateColor(item.ItemRate);
		if (item.ItemType == Item.Type.Other || item.ItemType == Item.Type.RecoveryConsump)
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

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (curItem == null) return;
		if(curItem.ItemType == Item.Type.Armor)
		{
			invenUI.SlotPointerEnter((ArmorItem)curItem);
		}
	}

	public void OnPointerMove(PointerEventData eventData)
	{
		if (curItem == null) return;
		if (curItem.ItemType == Item.Type.Armor)
		{
			invenUI.SlotPointerMove(eventData.position);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (curItem == null) return;
		if (curItem.ItemType == Item.Type.Armor)
		{
			invenUI.SlotPointerExit();
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (curItem == null) return;
		invenUI.SlotDragStart(this);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		invenUI.SlotDragEnd(this);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (curItem == null) return;
		invenUI.SlotDragMove(this, eventData.position);
	}

	public void OnDrop(PointerEventData eventData)
	{
		if (eventData.pointerDrag.TryGetComponent<ItemSlot>(out ItemSlot otherSlot))
		{
			if (null == otherSlot.curItem) return;
			if (otherSlot == this) return;

			GameManager.Inven.SwapItem(invenUI.CurInvenType, slotIdx, otherSlot.slotIdx);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (curItem == null) return;
		if (eventData.button == PointerEventData.InputButton.Right)
		{
			GameManager.UI.ShowPopUpUI<ItemOptionUI>("UI/PopUpUI/Inventory/ItemOption"
				, false).Init(invenUI, curItem, rectTransform.position);
		}
		else if(eventData.button == PointerEventData.InputButton.Left)
		{
			if(Time.time - lastLeftClickTime < doubleClickTime)
			{
				switch (curItem.ItemType)
				{
					case Item.Type.Armor:
						invenUI.Equip((ArmorItem)curItem);
						break;
					case Item.Type.Weapon:
						invenUI.Equip((WeaponItem)curItem);
						break;
					case Item.Type.Other:
						break;
					default:
						invenUI.Equip((ConsumpItem)curItem);
						break;
				}
			}
			lastLeftClickTime = Time.time;
		}
	}
}