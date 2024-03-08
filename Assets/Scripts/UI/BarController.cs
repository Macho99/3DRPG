using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BarController : BaseUI
{
	//Image background;
	Image mediumMask;
	Image frontMask;
	float lerpSpeed = 5f;
	float prevRatio = 1f;

	protected override void Awake()
	{
		base.Awake();
		mediumMask = images["MediumMask"];
		frontMask = images["FrontMask"];
	}

	public void Init(float ratio)
	{
		frontMask.fillAmount = ratio;
		mediumMask.fillAmount = ratio;
		prevRatio = ratio;
	}

	public void UIUpdate(float ratio)
	{
		StopAllCoroutines();
		bool heal = prevRatio < ratio;
		_ = StartCoroutine(CoLerp(ratio, heal));
		prevRatio = ratio;
	}

	private IEnumerator CoLerp(float ratio, bool heal)
	{
		float elapsed = 0f;
		while (elapsed < 1f)
		{
			frontMask.fillAmount = Mathf.Lerp(frontMask.fillAmount, ratio, Time.deltaTime * lerpSpeed);
			elapsed += Time.deltaTime;
			if(heal == true)
			{
				mediumMask.fillAmount = frontMask.fillAmount;
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
