using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHPBar : BarController
{
    Monster stat;
    protected override void SetFuncAndEvent()
    {
        stat = GetComponentInParent<Monster>();
        OnBarChange = stat.OnMonsterHPChange;
        initValueFunc = () => { return stat.hPRatio; };
        updateTextFunc = null;
    }
}
