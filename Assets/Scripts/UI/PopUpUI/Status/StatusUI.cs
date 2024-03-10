using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : PopUpUI
{
    TextMeshProUGUI healthPoint;
    TextMeshProUGUI manaPoint;
    TextMeshProUGUI recoveryHPPoint;
	TextMeshProUGUI recoveryMPPoint;
	TextMeshProUGUI armorPoint;
    TextMeshProUGUI stunResistPoint;
    TextMeshProUGUI currentMoney;
    StatManager stat;


	protected override void Awake()
    {
        base.Awake();
		stat = GameManager.Stat;
		healthPoint = texts["HealthPoint"];
        manaPoint = texts["ManaPoint"];
        recoveryHPPoint = texts["RecoveryHPPoint"];
        recoveryMPPoint = texts["RecoveryMPPoint"];
        armorPoint = texts["ArmorPoint"];
        stunResistPoint = texts["StunResistPoint"];
        currentMoney = texts["CurMoney"]; 
		buttons["CloseButton"].onClick.AddListener(CloseUI);
		buttons["ExitButton"].onClick.AddListener(GameManager.UI.MenuToggle);
	}

	private void OnEnable()
	{
        _ = StartCoroutine(CoRefresh());
	}

	private IEnumerator CoRefresh()
    {
        while (true)
		{
			healthPoint.text = $": {stat.CurHP} / {stat.MaxHP}";
			manaPoint.text = $": {stat.CurMP} / {stat.MaxMP}";
			recoveryHPPoint.text = $": {stat.RecoveryHP}";
			recoveryMPPoint.text = $": {stat.RecoveryMP}";
			armorPoint.text = $": {stat.Defence}";
			stunResistPoint.text = $": {stat.StunResistance}";
			currentMoney.text = $": {stat.Money}";
            yield return new WaitForSeconds(1f);
		}
    }
}
