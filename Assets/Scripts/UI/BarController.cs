using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class BarController : BaseUI
{
	public struct BarText
	{
		public float cur;
		public float max;
		public BarText(float cur, float max)
		{
			this.cur = cur;
			this.max = max;
		}
	}

	//Image background;
	Image mediumMask;
	Image frontMask;
	TextMeshProUGUI curText;
	TextMeshProUGUI maxText;
	float lerpSpeed = 5f;
	float prevRatio = 1f;

	protected Func<BarText> updateTextFunc;
	protected Func<float> initValueFunc;
	protected UnityEvent<float> OnBarChange;


	protected override void Awake()
	{
		base.Awake();
		mediumMask = images["MediumMask"];
		frontMask = images["FrontMask"];
		curText = texts["CurText"];
		maxText = texts["MaxText"];
		SetFuncAndEvent();
	}

	protected abstract void SetFuncAndEvent();

	private void OnEnable()
	{
		OnBarChange.AddListener(UIUpdate);
		float ratio = initValueFunc();
		mediumMask.fillAmount = ratio;
		frontMask.fillAmount = ratio;
		if(updateTextFunc != null)
		{
			BarText barText = updateTextFunc();
			curText.text = barText.cur.ToString();
			maxText.text = barText.max.ToString();
		}
	}

	private void OnDisable()
	{
		OnBarChange.RemoveListener(UIUpdate);
	}

	public void UIUpdate(float ratio)
	{
		StopAllCoroutines();
		bool heal = prevRatio < ratio;
		_ = StartCoroutine(CoLerp(ratio, heal));
		prevRatio = ratio;
		if (updateTextFunc != null)
		{
			BarText barText = updateTextFunc();
			curText.text = barText.cur.ToString();
			maxText.text = barText.max.ToString();
		}
	}

	private IEnumerator CoLerp(float ratio, bool heal)
	{
		float elapsed = 0f;
		while (elapsed < 1f)
		{
			frontMask.fillAmount = Mathf.Lerp(frontMask.fillAmount, ratio, Time.deltaTime * lerpSpeed * 2);
			elapsed += Time.deltaTime;
			if(heal == true)
			{
				mediumMask.fillAmount = Mathf.Lerp(mediumMask.fillAmount, frontMask.fillAmount, Time.deltaTime * lerpSpeed * 2);
			}
			yield return null;
		}

		if(heal == true)
		{
			yield break;
		}

		while (Mathf.Abs(mediumMask.fillAmount - frontMask.fillAmount) > 0.01f)
		{
			mediumMask.fillAmount = Mathf.Lerp(mediumMask.fillAmount, frontMask.fillAmount, Time.deltaTime * lerpSpeed);
			yield return null;
		}
	}

	public override void CloseUI() { }
}
