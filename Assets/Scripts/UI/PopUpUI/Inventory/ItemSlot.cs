using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, /*IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,*/
	IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
	InvenUI invenUI;
	[SerializeField] Image itemImage;
	[SerializeField] TextMeshProUGUI amountText;
	Toggle toggle;

	Item curItem;
	int slotIdx;
	RectTransform rectTransform;


	public Item CurItem { get { return curItem; } }
	public int SlotIdx { get { return slotIdx; } }
	public RectTransform RectTransform { get { return rectTransform; } }

	private void Awake()
	{
		toggle = GetComponent<Toggle>();
		rectTransform = GetComponent<RectTransform>();
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
		this.curItem = item;
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

	//public void OnPointerEnter(PointerEventData eventData)
	//{
	//	if (curItem == null) return;
	//	GameManager.UI.ItemInfo.Set(curItem);
	//}

	//public void OnPointerMove(PointerEventData eventData)
	//{
	//	if (curItem == null) return;
	//	Vector2 pos = eventData.position;
	//	pos.x += 10f;
	//	GameManager.UI.ItemInfo.Move(pos);
	//}

	//public void OnPointerExit(PointerEventData eventData)
	//{
	//	if (curItem == null) return;
	//	GameManager.UI.ItemInfo.Set(null);
	//}

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
		//GameManager.UI.DragInfo.Move(eventData.position);
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
		if(eventData.button == PointerEventData.InputButton.Right)
		{
			GameManager.UI.ShowPopUpUI<ItemOptionUI>("UI/PopUpUI/Inventory/ItemOption"
				, false).Init(curItem, rectTransform.position);
		}
	}
}