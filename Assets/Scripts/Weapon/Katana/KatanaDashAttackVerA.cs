﻿using System.Collections.Generic;
using UnityEngine;

public class KatanaDashAttackVerA : KatanaStandSwingBase
{
	public KatanaDashAttackVerA(Katana owner, StateMachine<Katana.State, Katana> stateMachine) 
		: base(owner, stateMachine, "Attack8")
	{

	}

	public override void Enter()
	{
		base.Enter();
		playerMove.GravityScale = 0.2f;
		playerAttack.SetAnimFloat("IdleAdapter", 1f);
	}

	public override void Exit()
	{
		base.Exit();
		playerMove.GravityScale = 1f;
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

	protected override void AttackFail()
	{
		base.AttackFail();
		player.SetAnimRootMotion(false);
		playerMove.SetAnimFloat("Reverse", -1f);
	}
}