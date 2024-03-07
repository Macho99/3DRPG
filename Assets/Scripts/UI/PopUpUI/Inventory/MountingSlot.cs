using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class MountingSlot : BaseUI, IDropHandler, IPointerClickHandler
{
	protected Item curItem;
	Image backgroundImage;
	Image curItemImage;

	public override void CloseUI() { }

	public abstract void OnDrop(PointerEventData eventData);

	public abstract void OnPointerClick(PointerEventData eventData);

	protected override void Awake()
	{
		base.Awake();
		backgroundImage = images["BackgroundImage"];
		curItemImage = images["CurItemImage"];
		backgroundImage.gameObject.SetActive(false);
	}

	protected void SetItem(Item item)
	{
		curItem = item;
		if(item == null )
		{
			backgroundImage.gameObject.SetActive(false);
			return;
		}
		curItemImage.sprite = curItem.Sprite;
		backgroundImage.gameObject.SetActive(true);
	}
}
