using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSelect : PopUpUI
{
	Action<Bow.ArrowState> resultFunc;
	protected override void Awake()
	{
		base.Awake();
		buttons["Blocker"].onClick.AddListener(NoneSelect);
		buttons["NormalArrow"].onClick.AddListener(NormalSelect);
		buttons["WindArrow"].onClick.AddListener(WindSelect);
		buttons["FireArrow"].onClick.AddListener(FireSelect);
	}

	public void Init(Action<Bow.ArrowState> resultFunc)
	{
		this.resultFunc = resultFunc;
	}

	private void WindSelect()
	{
		resultFunc?.Invoke(Bow.ArrowState.Wind);
		CloseUI();
	}

	private void FireSelect()
	{
		resultFunc?.Invoke(Bow.ArrowState.Fire);
		CloseUI();
	}

	private void NormalSelect()
	{
		resultFunc?.Invoke(Bow.ArrowState.Normal);
		CloseUI();
	}

	private void NoneSelect()
	{
		resultFunc?.Invoke(Bow.ArrowState.None);
		CloseUI();
	}
}
