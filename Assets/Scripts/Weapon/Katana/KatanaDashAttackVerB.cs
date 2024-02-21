using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class KatanaDashAttackVerB : KatanaSwingBase
{
	public KatanaDashAttackVerB(Katana owner, StateMachine<Katana.State, Katana> stateMachine) 
		: base(owner, stateMachine, "Attack9")
	{
	}

	public override void Enter()
	{
		base.Enter();
		playerAttack.SetAnimFloat("Armed", 1f);
		owner.SetDummyRender(false);
		player.ChangeState(Player.State.StandAttack);
		playerAttack.SetAnimFloat("Grruzam", 1f);
	}

	protected override bool CheckTransition()
	{
		if(attack1Pressed == true)
		{
			stateMachine.ChangeState(Katana.State.DashComboVerB01);
			return true;
		}
		return false;
	}
}