using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUI : MenuToggleUI
{
    BarController hpBar;
    DeathKnight stat;

    protected override void Awake()
    {
        base.Awake();
        stat = FindObjectOfType<DeathKnight>();
        hpBar = transforms["HPBar"].GetComponent<BarController>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        stat.OnBossHPChange.AddListener(hpBar.UIUpdate);
    }

    private void Start()
    {
        hpBar.Init(stat.hPRatio);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        stat.OnBossHPChange.RemoveListener(hpBar.UIUpdate);
    }
}
