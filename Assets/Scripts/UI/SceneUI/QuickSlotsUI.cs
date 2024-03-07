using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotsUI : MenuToggleUI
{
	Image meleeImage;
	Image rangedImage;
	Image consump3Image;
	Image consump4Image;
	TextMeshProUGUI curWeaponNameText;

	PlayerAttack playerAttack;

	protected override void Awake()
	{
		base.Awake();
		meleeImage = images["MeleeImage"];
		rangedImage = images["RangedImage"];
		consump3Image = images["Consump3Image"];
		consump4Image = images["Consump4Image"];
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

	private void RefreshSlot()
	{
		meleeImage.sprite = GameManager.Inven.GetWeaponSlot(WeaponType.Melee)?.Sprite;
		rangedImage.sprite = GameManager.Inven.GetWeaponSlot(WeaponType.Ranged)?.Sprite;
	}

	private void CurHoldWeaponTypeChange(WeaponType newHold)
	{
		curWeaponNameText.text = GameManager.Inven.GetWeaponSlot(newHold).ItemName;
	}
}