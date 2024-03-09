using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

public class PlayerHPBar : BarController
{
	protected override void SetFuncAndEvent()
	{
		StatManager stat = GameManager.Stat;
		updateTextFunc = () => { return new BarText(stat.CurHP, stat.MaxHP); };
		initValueFunc = () => { return stat.HPRatio; };
		OnBarChange = stat.OnPlayerHPChange;
	}
}
