using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotUI : HideableSceneUI
{
	Image meleeImage;
	Image rangedImage;
	Image meleeSelect;
	Image rangedSelect;
	Image consump1Image;
	Image consump2Image;
	TextMeshProUGUI curWeaponNameText;
	TextMeshProUGUI consump1AmountText;
	TextMeshProUGUI consump2AmountText;

	PlayerAttack playerAttack;

	protected override void Awake()
	{
		base.Awake();
		meleeImage = images["MeleeImage"];
		rangedImage = images["RangedImage"];
		meleeSelect = images["MeleeSelect"];
		rangedSelect = images["RangedSelect"];
		consump1Image = images["Consump1Image"];
		consump2Image = images["Consump2Image"];
		consump1AmountText = texts["Consump1Amount"];
		consump2AmountText = texts["Consump2Amount"];
		curWeaponNameText = texts["CurWeaponName"];
		playerAttack = FieldSFC.Player.PlayerAttack;

		rangedSelect.gameObject.SetActive(false);
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
		if(consump1 == null)
		{
			consump1AmountText.gameObject.SetActive(false);
		}
		else
		{
			consump1AmountText.text = consump1.Amount.ToString();
			consump1AmountText.gameObject.SetActive(true);
		}
		if(consump2 == null)
		{
			consump2AmountText.gameObject.SetActive(false);
		}
		else
		{
			consump2AmountText.text = consump2.Amount.ToString();
			consump2AmountText.gameObject.SetActive(true);
		}

		SetItem(meleeImage, melee);
		SetItem(rangedImage, ranged);
		SetItem(consump1Image, consump1);
		SetItem(consump2Image, consump2);
	}

	private void CurHoldWeaponTypeChange(WeaponType newHold)
	{
		switch (newHold)
		{
			case WeaponType.Melee:
				meleeSelect.gameObject.SetActive(true);
				rangedSelect.gameObject.SetActive(false);
				break;
			case WeaponType.Ranged:
				meleeSelect.gameObject.SetActive(false);
				rangedSelect.gameObject.SetActive(true);
				break;
		}

		WeaponItem weaponItem = GameManager.Inven.GetWeaponSlot(newHold);
		if(weaponItem == null)
		{
			curWeaponNameText.text = "";
		}
		else
		{
			curWeaponNameText.text = weaponItem.ItemName;
		}
	}
}