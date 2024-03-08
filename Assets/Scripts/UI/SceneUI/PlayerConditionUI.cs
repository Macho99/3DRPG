using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConditionUI : MenuToggleUI
{
	BarController hpBar;
	BarController mpBar;
	StatManager stat;

	protected override void Awake()
	{
		base.Awake();
		stat = GameManager.Stat;
		hpBar = transforms["HPBar"].GetComponent<BarController>();
		mpBar = transforms["MPBar"].GetComponent<BarController>();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		stat.OnPlayerHPChange.AddListener(hpBar.UIUpdate);
		stat.OnPlayerMPChange.AddListener(mpBar.UIUpdate);
	}

	private void Start()
	{
		hpBar.Init(stat.HPRatio);
		mpBar.Init(stat.MPRatio);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		stat.OnPlayerHPChange.RemoveListener(hpBar.UIUpdate);
		stat.OnPlayerMPChange.RemoveListener(mpBar.UIUpdate);
	}
}
