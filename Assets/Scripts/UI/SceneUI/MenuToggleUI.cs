using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MenuToggleUI : SceneUI
{
	CanvasGroup canvasGroup;

	protected override void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
	}

	private void OnEnable()
	{
		GameManager.UI.OnMenuToggle.AddListener(SetAlpha);
	}

	private void OnDisable()
	{
		GameManager.UI.OnMenuToggle.RemoveListener(SetAlpha);
	}

	private void SetAlpha(bool value)
	{
		canvasGroup.alpha = value ? 0f : 1f;
	}
}