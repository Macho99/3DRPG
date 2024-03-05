using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSelect : PopUpUI
{
	Action<Bow.ArrowProperty> resultFunc;
	protected override void Awake()
	{
		base.Awake();
		buttons["Blocker"].onClick.AddListener(NoneSelect);
		buttons["IceArrow"].onClick.AddListener(IceSelect);
		buttons["WindArrow"].onClick.AddListener(WindSelect);
		buttons["FireArrow"].onClick.AddListener(FireSelect);
	}

	public void Init(Action<Bow.ArrowProperty> resultFunc)
	{
		this.resultFunc = resultFunc;
	}

	private void WindSelect()
	{
		resultFunc?.Invoke(Bow.ArrowProperty.Wind);
		CloseUI();
	}

	private void FireSelect()
	{
		resultFunc?.Invoke(Bow.ArrowProperty.Fire);
		CloseUI();
	}

	private void IceSelect()
	{
		resultFunc?.Invoke(Bow.ArrowProperty.Ice);
		CloseUI();
	}

	private void NoneSelect()
	{
		resultFunc?.Invoke(Bow.ArrowProperty.None);
		CloseUI();
	}
}
