using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class KatanaJumpCombo01 : KatanaOnAirSwingBase
{
	public KatanaJumpCombo01(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack18")
	{
	}

	protected override bool CheckTransition()
	{
		if(attack1Pressed == true)
		{
			stateMachine.ChangeState(Katana.State.JumpCombo02);
			return true;
		}
		return false;
	}
}

public class KatanaJumpCombo02 : KatanaOnAirSwingBase
{
	public KatanaJumpCombo02(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack19")
	{
	}

	protected override bool CheckTransition()
	{
		if (attack1Pressed == true)
		{
			stateMachine.ChangeState(Katana.State.JumpCombo03);
			return true;
		}
		return false;
	}
}

public class KatanaJumpCombo03 : KatanaOnAirSwingBase
{
	public KatanaJumpCombo03(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack20")
	{
	}

	protected override bool CheckTransition()
	{
		if (attack1Pressed == true)
		{
			stateMachine.ChangeState(Katana.State.JumpCombo04);
			return true;
		}
		return false;
	}
}

public class KatanaJumpCombo04 : KatanaOnAirSwingBase
{
	public KatanaJumpCombo04(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack21")
	{
	}

	protected override bool CheckTransition()
	{
		if (attack1Pressed == true)
		{
			stateMachine.ChangeState(Katana.State.JumpCombo05);
			return true;
		}
		return false;
	}
}

public class KatanaJumpCombo05 : KatanaOnAirSwingBase
{
	public KatanaJumpCombo05(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack22")
	{
	}

	protected override bool CheckTransition()
	{
		if (attack1Pressed == true)
		{
			stateMachine.ChangeState(Katana.State.JumpCombo06);
			return true;
		}
		return false;
	}
}

public class KatanaJumpCombo06 : KatanaOnAirSwingBase
{
	public KatanaJumpCombo06(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack23")
	{
	}

	protected override bool CheckTransition()
	{
		if(attack1Pressed == true)
		{
			stateMachine.ChangeState(Katana.State.JumpCombo07);
			return true;
		}
		return false;
	}
}

public class KatanaJumpCombo07 : KatanaStandSwingBase
{
	public KatanaJumpCombo07(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack24", 2)
	{
	}
	public override void Enter()
	{
		base.Enter();
		playerMove.GravityScale = 0.2f;
		playerMove.OnAirJump();
	}

	public override void Exit()
	{
		base.Exit();
		playerMove.GravityScale = 1f;
	}

	protected override void AttackStart()
	{
		base.AttackStart();
		if(curAttackNum == 2)
		{
			playerMove.GravityScale = 5f;
		}
	}

	protected override bool CheckTransition()
	{
		if(attack2Holded== true)
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
