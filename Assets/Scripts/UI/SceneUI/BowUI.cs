using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BowUI : HideableSceneUI
{
	public struct SkillCooltimeStruct
	{
		public float qCooltime;
		public float eCooltime;
		public float rCooltime;
		public float qLefttime;
		public float eLefttime;
		public float rLefttime;

		public bool CooltimeEnd()
		{
			if(qCooltime > 0f || eCooltime > 0f || rCooltime > 0f)
				return false;
			return true;
		}
	}

	CanvasGroup aimCircleGroup;
	Image outCircle;

	[Header("얼음, 바람, 화염 순서로 넣으세요")]
	[SerializeField] Sprite[] QSprite;
	[SerializeField] Sprite[] ESprite;

	Image QImage;
	Image EImage;
	Image RImage;
	TextMeshProUGUI QText;
	TextMeshProUGUI EText;
	TextMeshProUGUI RText;

	SkillCooltimeStruct cooltime;

	protected override void Awake()
	{
		base.Awake();
		aimCircleGroup = images["AimCircle"].GetComponent<CanvasGroup>();
		outCircle = images["OutCircle"];
		QImage = images["QSkillImage"];
		EImage = images["ESkillImage"];
		RImage = images["RSkillImage"];
		QText = texts["QSkillText"];
		EText = texts["ESkillText"];
		RText = texts["RSkillText"];
	}

	public void UIUpdate(Bow.ArrowProperty property, SkillCooltimeStruct cooltime)
	{
		StopAllCoroutines();

		this.cooltime = cooltime;
		QImage.sprite = QSprite[(int) property];
		EImage.sprite = ESprite[(int) property];

		_ = StartCoroutine(CoUIUpdate());
		_ = StartCoroutine(CoTextSet());
	}

	private IEnumerator CoUIUpdate()
	{
		while(cooltime.CooltimeEnd() == false)
		{
			QImage.fillAmount = cooltime.qLefttime <= 0f ? 1 : 1 - cooltime.qLefttime / cooltime.qCooltime;
			EImage.fillAmount = cooltime.eLefttime <= 0f ? 1 : 1 - cooltime.eLefttime / cooltime.eCooltime;
			RImage.fillAmount = cooltime.rLefttime <= 0f ? 1 : 1 - cooltime.rLefttime / cooltime.rCooltime;
			yield return null;
			cooltime.qLefttime -= Time.deltaTime;
			cooltime.eLefttime -= Time.deltaTime;
			cooltime.rLefttime -= Time.deltaTime;
		}

		QImage.fillAmount = 1f;
		EImage.fillAmount = 1f;
		RImage.fillAmount = 1f;
	}

	private IEnumerator CoTextSet()
	{
		while(cooltime.CooltimeEnd() == false)
		{
			int leftTime = Mathf.RoundToInt(cooltime.qLefttime);
			QText.text = leftTime <= 0 ? "" : leftTime.ToString();
			leftTime = Mathf.RoundToInt(cooltime.eLefttime);
			EText.text = leftTime <= 0 ? "" : leftTime.ToString();
			leftTime = Mathf.RoundToInt(cooltime.rLefttime);
			RText.text = leftTime <= 0 ? "" : leftTime.ToString();

			yield return new WaitForSeconds(1f);
		}
	}

	public void AimPointHide(bool value)
	{
		if(value == true)
		{
			aimCircleGroup.alpha = 0f;
		}
		else
		{
			aimCircleGroup.alpha = 0.8f;
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