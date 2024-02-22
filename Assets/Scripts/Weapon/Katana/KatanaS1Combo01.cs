using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class KatanaS1Combo01_01 : KatanaSwingBase
{
	public KatanaS1Combo01_01(Katana owner, StateMachine<Katana.State, Katana> stateMachine) : base(owner, stateMachine, "Attack1")
	{
	}

	public override void Enter()
	{
		base.Enter();
		owner.ChangePlayerState(Player.State.StandAttack);
	}

	protected override bool CheckTransition()
	{
		if (attack1Pressed == true)
		{
			stateMachine.ChangeState(Katana.State.S1Combo01_02);
			return true;
		}
		return false;
	}
}

public class KatanaS1Combo01_02 : KatanaSwingBase
{
	public KatanaS1Combo01_02(Katana owner, StateMachine<Katana.State, Katana> stateMachine) : base(owner, stateMachine, "Attack2")
	{
	}

	public override void Enter()
	{
		base.Enter();
		owner.ChangePlayerState(Player.State.StandAttack);
	}


	protected override bool CheckTransition()
	{
		if (attack1Pressed == true)
		{
			stateMachine.ChangeState(Katana.State.S1Combo01_03);
			return true;
		}
		return false;
	}
}

public class KatanaS1Combo01_03 : KatanaSwingBase
{
	public KatanaS1Combo01_03(Katana owner, StateMachine<Katana.State, Katana> stateMachine) : base(owner, stateMachine, "Attack3")
	{
	}

	public override void Enter()
	{
		base.Enter();
		owner.ChangePlayerState(Player.State.StandAttack);
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