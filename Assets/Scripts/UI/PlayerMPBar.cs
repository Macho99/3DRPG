using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerMPBar : BarController
{
	protected override void SetFuncAndEvent()
	{
		StatManager stat = GameManager.Stat;
		updateTextFunc = () => { return new BarText(stat.CurMP, stat.MaxMP); };
		initValueFunc = () => { return stat.MPRatio; };
		OnBarChange = stat.OnPlayerMPChange;
	}
}
