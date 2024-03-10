using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Alarm : HideableSceneUI
{
	[SerializeField] float fadeDuration = 2f;
	[SerializeField] float riseSpeed = 100f;

	Image image;
	TextMeshProUGUI upperText;
	TextMeshProUGUI lowerText;

	Vector2 initPos;

	protected override void Awake()
	{
		base.Awake();
		image = images["AlarmImage"];
		upperText = texts["UpperText"];
		lowerText = texts["LowerText"];
		initPos = rectTransform.anchoredPosition;
	}

	public void Init(string upperStr, string lowerStr = null, Sprite sprite = null)
	{
		upperText.text = upperStr;
		if(lowerStr == null)
		{
			lowerText.gameObject.SetActive(false);
		}
		else
		{
			lowerText.text = lowerStr;
			lowerText.gameObject.SetActive(true);
		}

		if(sprite == null)
		{
			image.gameObject.SetActive(false);
		}
		else
		{
			image.sprite = sprite;
			image.gameObject.SetActive(true);
		}

		_ = StartCoroutine(CoRise());
	}

	private IEnumerator CoRise()
	{
		float elapsed = 0f;

		while(elapsed < 0.5f)
		{
			elapsed += Time.deltaTime;
			rectTransform.position += new Vector3(0f, 0f, elapsed);
			canvasGroup.alpha = 1 - elapsed / fadeDuration;
			yield return null;
		}

		while(elapsed < fadeDuration)
		{
			elapsed += Time.deltaTime;
			float riseAmount = riseSpeed * Time.deltaTime;
			rectTransform.position += new Vector3(0f, riseAmount, riseAmount);
			canvasGroup.alpha = 1 - elapsed / fadeDuration;
			yield return null;
		}

		rectTransform.anchoredPosition = initPos;
		canvasGroup.alpha = 1f;
		CloseUI();
	}
}