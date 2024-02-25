using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class KatanaDodgeAttack : KatanaStandSwingBase
{
	public KatanaDodgeAttack(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack33")
	{
	}

	public override void Enter()
	{
		base.Enter();
		playerMove.GravityScale = 0f;
		playerAttack.SetAnimFloat("IdleAdapter", 1f);
	}

	public override void Exit()
	{
		base.Exit();
		playerMove.GravityScale = 1f;
	}

	protected override bool CheckTransition()
	{
		if(attack1Pressed == true)
		{
			stateMachine.ChangeState(Katana.State.JumpCombo01);
			return true;
		}
		return false;
	}
}
