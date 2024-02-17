using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

	protected override void Attack1BtnCombo(Player.State state)
	{
		//empty
	}
}