using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class BowAimPoint : SceneUI
{
	CanvasGroup canvasGroup;
	Image outCircle;

	protected override void Awake()
	{
		base.Awake();
		outCircle = images["OutCircle"];
		canvasGroup = GetComponent<CanvasGroup>();
	}

	public void AimPointHide(bool value)
	{
		if(value == true)
		{
			canvasGroup.alpha = 0f;
		}
		else
		{
			canvasGroup.alpha = 0.8f;
		}
	}

	public void SetOutCircleScale(float weight)
	{
		Mathf.Clamp(weight, 0.1f, 1f);
		outCircle.rectTransform.localScale = Vector3.one * weight;
	}

	public override void CloseUI()
	{
		GameManager.UI.CloseSceneUI(this);
	}
}