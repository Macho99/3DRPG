using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class KatanaS2Combo01_01 : KatanaStandSwingBase
{
	public KatanaS2Combo01_01(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack25")
	{
	}

	public override void Enter()
	{
		base.Enter();
		playerAttack.SetAnimFloat("IdleAdapter", -1f);
	}

	protected override bool CheckTransition()
	{
		if(attack1Pressed == true)
		{
			stateMachine.ChangeState(Katana.State.S2Combo01_02);
			return true;
		}
		return false;
	}
}

public class KatanaS2Combo01_02 : KatanaStandSwingBase
{
	public KatanaS2Combo01_02(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack26", 2)
	{
	}

	protected override bool CheckTransition()
	{
		if (attack1Pressed == true)
		{
			stateMachine.ChangeState(Katana.State.S2Combo01_03);
			return true;
		}
		return false;
	}
}

public class KatanaS2Combo01_03 : KatanaStandSwingBase
{
	public KatanaS2Combo01_03(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack27")
	{
	}

	protected override bool CheckTransition()
	{
		if(attack2Holded == true)
		{
			stateMachine.ChangeState(Katana.State.QuickDrawEntry);
			return true;
		}
		if (attack2Up == true)
		{
			stateMachine.ChangeState(Katana.State.S2Combo02_01);
			return true;
		}
		return false;
	}
}