using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class KatanaDashComboVerA01 : KatanaStandSwingBase
{
	public KatanaDashComboVerA01(Katana owner, StateMachine<Katana.State, Katana> stateMachine) 
		: base(owner, stateMachine, "Attack10")
	{
	}

	protected override bool CheckTransition()
	{
		if (attack1Pressed)
		{
			stateMachine.ChangeState(Katana.State.DashComboVerA02);
			return true;
		}
		return false;
	}
}

public class KatanaDashComboVerA02 : KatanaStandSwingBase
{
	public KatanaDashComboVerA02(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack11")
	{
	}

	protected override bool CheckTransition()
	{
		if (attack1Pressed)
		{
			stateMachine.ChangeState(Katana.State.DashComboVerA03);
			return true;
		}
		return false;
	}
}

public class KatanaDashComboVerA03 : KatanaStandSwingBase
{
	public KatanaDashComboVerA03(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack12")
	{
	}

	protected override bool CheckTransition()
	{
		if (attack1Pressed)
		{
			stateMachine.ChangeState(Katana.State.DashComboVerA04);
			return true;
		}
		return false;
	}
}

public class KatanaDashComboVerA04 : KatanaStandSwingBase
{
	public KatanaDashComboVerA04(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack13")
	{
	}

	protected override bool CheckTransition()
	{
		if (attack2Holded)
		{
			stateMachine.ChangeState(Katana.State.QuickDrawEntry);
			return true;
		}
		if (attack2Up)
		{
			stateMachine.ChangeState(Katana.State.S2Combo02_01);
			return true;
		}
		return false;
	}
}