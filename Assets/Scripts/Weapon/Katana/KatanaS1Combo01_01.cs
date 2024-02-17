using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class KatanaS1Combo01_01 : StateBase<Katana.State, Katana>
{
	PlayerAttack playerAttack;
	PlayerAnimEvent playerAnimEvent;
	TargetFollower trail;
	AttackProcess attackProcess;

	public KatanaS1Combo01_01(Katana owner, StateMachine<Katana.State, Katana> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		attackProcess = AttackProcess.BeforeAttack;
		playerAttack.SetAnimTrigger("Attack1");
		playerAnimEvent.OnAttackStart.AddListener(AttackStart);
		playerAnimEvent.OnAttackEnd.AddListener(AttackEnd);
	}

	private void AttackStart()
	{
		attackProcess = AttackProcess.Attacking;
		trail = owner.BeginAttack();
	}

	private void AttackEnd()
	{
		trail.SetTarget(null);
		attackProcess = AttackProcess.AfterAttack;
	}

	public override void Exit()
	{
		playerAnimEvent.OnAttackStart.RemoveListener(AttackStart);
		playerAnimEvent.OnAttackEnd.RemoveListener(AttackEnd);
		trail = null;
	}

	public override void Setup()
	{
		playerAttack = owner.PlayerAttack;
		playerAnimEvent = owner.PlayerAnimEvent;
	}

	public override void Transition()
	{
		if(attackProcess == AttackProcess.End)
		{
			if(playerAttack.IsAnimWait(0) == false)
			{
				stateMachine.ChangeState(Katana.State.Idle); 
				return;
			}
		}
	}

	public override void Update()
	{
		switch (attackProcess)
		{
			case AttackProcess.BeforeAttack:
				break;
			case AttackProcess.Attacking:
				bool result = owner.Attack();
				if (result == false)
				{
					trail.SetTarget(null);
					stateMachine.ChangeState(Katana.State.AttackFail);
					return;
				}
				break;
			case AttackProcess.AfterAttack:
				if (playerAttack.IsAnimWait(0) == true)
				{
					attackProcess = AttackProcess.End;
					playerAttack.SetAnimTrigger("BaseExit");
				}
				break;
			default:
				break;
		}
	}
}