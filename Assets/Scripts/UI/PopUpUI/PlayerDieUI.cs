using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDieUI : PopUpUI
{
	Button blocker;
	Action closeAction;

	protected override void Awake()
	{
		base.Awake();
		blocker = buttons["Blocker"];
	}

	public void Init(Action closeAction)
	{
		this.closeAction = closeAction;
		_ = StartCoroutine(CoAddListener());
	}

	private IEnumerator CoAddListener()
	{
		yield return new WaitForSeconds(2f);
		blocker.onClick.RemoveAllListeners();
		blocker.onClick.AddListener(CloseUI);
	}

	public override void CloseUI()
	{
		base.CloseUI();
		closeAction?.Invoke();
	}
}
