using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

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

	protected override void Attack1BtnCombo(Player.State state)
	{
		if (curAttack == AttackProcess.AfterAttack)
		{
			stateMachine.ChangeState(Katana.State.S1Combo01_03);
			return;
		}
	}
}