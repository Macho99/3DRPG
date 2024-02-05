using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : InGameUI
{
	private float lerpSpeed = 5f;

	private Image background;
	private Image greenMask;
	private Image redMask;
	private MonsterAction owner;

	protected override void Awake()
	{	
		base.Awake();
		background = images["Background"];
		greenMask = images["GreenMask"];
		redMask = images["RedMask"];
		SetOffset(new Vector3(0f, 80f, 0f));
	}

	protected override void Init()
	{
		owner = followTarget.GetComponent<MonsterAction>();
		owner.OnHpChanged.AddListener(UIUpdate);
		SetVisible(false);
	}

	private void OnDisable()
	{
		greenMask.fillAmount = 1f;
		redMask.fillAmount = 1f;
		owner.OnHpChanged.RemoveListener(UIUpdate);
	}

	public void UIUpdate(float ratio)
	{
		SetVisible(true);

		greenMask.fillAmount = ratio;
		StopAllCoroutines();

		if (owner.CurHp <= 0)
		{
			SetVisible(false);
			return;
		}

		_ = StartCoroutine(CoOff());
	}

	private IEnumerator CoOff()
	{
		yield return new WaitForSeconds(1f);

		while (Mathf.Abs(greenMask.fillAmount - redMask.fillAmount) > 0.0001f)
		{
			redMask.fillAmount = Mathf.Lerp(greenMask.fillAmount, redMask.fillAmount, 1 - Time.deltaTime * lerpSpeed);
			yield return null;
		}
	}

	private void SetVisible(bool val)
	{
		background.gameObject.SetActive(val);
		greenMask.gameObject.SetActive(val);
		redMask.gameObject.SetActive(val);
	}

}
