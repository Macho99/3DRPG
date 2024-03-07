using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class MountingSlot : BaseUI, IDropHandler, IPointerClickHandler
{
	InvenUI invenUI;
	protected Item curItem;
	Image backgroundImage;
	Image emptyImage;
	Image curItemImage;

	public override void CloseUI() { }

	public abstract void OnDrop(PointerEventData eventData);

	public abstract void OnPointerClick(PointerEventData eventData);

	protected override void Awake()
	{
		base.Awake();
		backgroundImage = images["BackgroundImage"];
		curItemImage = images["CurItemImage"];
		emptyImage = images["EmptyImage"];
		backgroundImage.gameObject.SetActive(false);
		invenUI = GetComponentInParent<InvenUI>();
	}

	protected void SetItem(Item item)
	{
		curItem = item;
		if(item == null )
		{
			backgroundImage.color = invenUI.GetRateColor(Item.Rate.Normal);
			//emptyImage.enabled = true;
			backgroundImage.gameObject.SetActive(false);
			return;
		}
		backgroundImage.color = invenUI.GetRateColor(item.ItemRate);
		curItemImage.sprite = curItem.Sprite;
		//emptyImage.enabled = false;
		backgroundImage.gameObject.SetActive(true);
	}
}
