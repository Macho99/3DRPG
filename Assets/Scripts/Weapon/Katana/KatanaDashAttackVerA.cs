﻿using System.Collections.Generic;
using UnityEngine;

public class KatanaDashAttackVerA : KatanaSwingBase
{
	public KatanaDashAttackVerA(Katana owner, StateMachine<Katana.State, Katana> stateMachine) 
		: base(owner, stateMachine, "Attack8")
	{

	}

	public override void Enter()
	{
		base.Enter();
		player.ChangeState(Player.State.OnAirAttack);
		playerMove.GravityMultiplier = 0.2f;
	}

	protected override bool CheckTransition()
	{
		if (attack1Pressed == true) 
		{ 
			stateMachine.ChangeState(Katana.State.DashComboVerA01);
			return true;
		}
		return false;
	}
}