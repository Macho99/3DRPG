using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class KatanaS2Combo02_01 : KatanaStandSwingBase
{
	public KatanaS2Combo02_01(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack28", 1)
	{
	}

	public override void Enter()
	{

		if (GameManager.Stat.TrySubCurMP(40) == false)
		{
			stateMachine.ChangeState(Katana.State.Idle);
			return;
		}
		base.Enter();
	}

	protected override bool CheckTransition()
	{
		if(attack1Pressed == true)
		{
			stateMachine.ChangeState(Katana.State.S2Combo02_02);
			return true;
		}
		return false;
	}
}

public class KatanaS2Combo02_02 : KatanaStandSwingBase
{
	public KatanaS2Combo02_02(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack29", 2)
	{
	}

	public override void Enter()
	{
		base.Enter();
		playerAttack.SetAnimFloat("IdleAdapter", -1f);
	}

	protected override bool CheckTransition()
	{
		if (attack1Pressed == true)
		{
			stateMachine.ChangeState(Katana.State.S2Combo02_03);
			return true;
		}
		return false;
	}
}

public class KatanaS2Combo02_03 : KatanaStandSwingBase
{
	public KatanaS2Combo02_03(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack30", 2)
	{
	}

	protected override bool CheckTransition()
	{
		if (attack1Pressed == true)
		{
			stateMachine.ChangeState(Katana.State.S2Combo02_04);
			return true;
		}
		return false;
	}
}

public class KatanaS2Combo02_04 : KatanaStandSwingBase
{
	public KatanaS2Combo02_04(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack32")
	{
	}

	protected override bool CheckTransition()
	{
		if (attack2Holded == true)
		{
			stateMachine.ChangeState(Katana.State.QuickDrawEntry);
			return true;
		}
		return false;
	}
}