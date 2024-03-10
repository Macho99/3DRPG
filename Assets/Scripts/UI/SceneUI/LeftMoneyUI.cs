using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LeftMoneyUI : HideableSceneUI
{
    TextMeshProUGUI leftMoney;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    private void Start()
    {
        leftMoney = texts["LeftMoney"];
    }

    private void Update()
    {
        leftMoney.text = GameManager.Stat.Money.ToString();
    }

    protected override void OnDisable()
    {
        base.OnEnable();
    }
}
