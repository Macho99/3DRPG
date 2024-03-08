using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotUI : MenuToggleUI
{
	Image meleeImage;
	Image rangedImage;
	Image consump1Image;
	Image consump2Image;
	TextMeshProUGUI curWeaponNameText;

	PlayerAttack playerAttack;

	protected override void Awake()
	{
		base.Awake();
		meleeImage = images["MeleeImage"];
		rangedImage = images["RangedImage"];
		consump1Image = images["Consump3Image"];
		consump2Image = images["Consump4Image"];
		curWeaponNameText = texts["CurWeaponName"];
		playerAttack = FieldSFC.Player.PlayerAttack;
	}

	private void Start()
	{
		RefreshSlot();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		GameManager.Inven.OnItemChange.AddListener(RefreshSlot);
		playerAttack.OnCurHoldWeaponTypeChange.AddListener(CurHoldWeaponTypeChange);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		GameManager.Inven.OnItemChange.RemoveListener(RefreshSlot);
		playerAttack.OnCurHoldWeaponTypeChange.RemoveListener(CurHoldWeaponTypeChange);
	}

	private void SetItem(Image image, Item item)
	{
		Color color;
		if (item == null)
		{
			color = image.color;
			color.a = 0f;
			image.color = color;
		}
		else
		{
			image.sprite = item.Sprite;
			color = image.color;
			color.a = 1f;
			image.color = color;
		}
	}

	private void RefreshSlot()
	{
		WeaponItem melee = GameManager.Inven.GetWeaponSlot(WeaponType.Melee);
		WeaponItem ranged = GameManager.Inven.GetWeaponSlot(WeaponType.Ranged);
		ConsumpItem consump1 = GameManager.Inven.GetConsumpSlot(ConsumpSlotType.Slot1);
		ConsumpItem consump2 = GameManager.Inven.GetConsumpSlot(ConsumpSlotType.Slot2);

		SetItem(meleeImage, melee);
		SetItem(rangedImage, ranged);
		SetItem(consump1Image, consump1);
		SetItem(consump2Image, consump2);
	}

	private void CurHoldWeaponTypeChange(WeaponType newHold)
	{
		curWeaponNameText.text = GameManager.Inven.GetWeaponSlot(newHold).ItemName;
	}
}