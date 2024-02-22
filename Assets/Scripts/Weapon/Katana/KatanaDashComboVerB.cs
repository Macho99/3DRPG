using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class KatanaDashComboVerB01 : KatanaSwingBase
{
	public KatanaDashComboVerB01(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack14")
	{
	}

	protected override bool CheckTransition()
	{
		if(attack1Pressed == true)
		{
			stateMachine.ChangeState(Katana.State.DashComboVerB02);
			return true;
		}
		return false;
	}
}

public class KatanaDashComboVerB02 : KatanaSwingBase
{
	public KatanaDashComboVerB02(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack15")
	{
	}

	protected override bool CheckTransition()
	{
		if (attack1Pressed == true)
		{
			stateMachine.ChangeState(Katana.State.DashComboVerB03);
			return true;
		}
		return false;
	}
}
public class KatanaDashComboVerB03 : KatanaSwingBase
{
	public KatanaDashComboVerB03(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack16")
	{
	}

	protected override bool CheckTransition()
	{
		if(attack2Holded == true)
		{
			stateMachine.ChangeState(Katana.State.QuickDrawEntry);
			return true;
		}
		return false;
	}
}