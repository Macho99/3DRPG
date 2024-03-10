using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BossHPBar : BarController
{
	DeathKnight stat;
	protected override void SetFuncAndEvent()
	{
		stat = FindObjectOfType<DeathKnight>();
		OnBarChange = stat.OnBossHPChange;
		initValueFunc = () => { return stat.hPRatio; };
		updateTextFunc = null;
	}
}