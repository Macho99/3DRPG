using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public abstract class KatanaSwingBase : StateBase<Katana.State, Katana>
{
	protected Player player;
	protected PlayerAttack playerAttack;
	protected PlayerAnimEvent playerAnimEvent;
	protected TargetFollower trail;

	protected AttackProcess curAttack;
	protected string triggerName;

	public KatanaSwingBase(Katana owner, StateMachine<Katana.State, Katana> stateMachine, string triggerName) : base(owner, stateMachine)
	{
		this.triggerName = triggerName;
	}

	public override void Enter()
	{
		curAttack = AttackProcess.BeforeAttack;
		playerAttack.SetAnimTrigger(triggerName);
		playerAnimEvent.OnAttackStart.AddListener(AttackStart);
		playerAnimEvent.OnAttackEnd.AddListener(AttackEnd);
		playerAttack.OnAttack1Down.AddListener(Attack1BtnCombo);
	}

	public override void Exit()
	{
		trail = null;
		playerAnimEvent.OnAttackStart.RemoveListener(AttackStart);
		playerAnimEvent.OnAttackEnd.RemoveListener(AttackEnd);
		playerAttack.OnAttack1Down.RemoveListener(Attack1BtnCombo);
	}

	private void AttackStart()
	{
		curAttack = AttackProcess.Attacking;
		trail = owner.BeginAttack();
	}

	private void AttackEnd()
	{
		trail.SetTarget(null);
		curAttack = AttackProcess.AfterAttack;
	}

	public override void Setup()
	{
		playerAttack = owner.PlayerAttack;
		playerAnimEvent = owner.PlayerAnimEvent;
	}

	public override void Transition()
	{
		if (curAttack == AttackProcess.End)
		{
			if (playerAttack.IsAnimWait(0) == false)
			{
				stateMachine.ChangeState(Katana.State.Idle);
				return;
			}
		}
	}

	public override void Update()
	{
		switch (curAttack)
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
					curAttack = AttackProcess.End;
					playerAttack.SetAnimTrigger("BaseExit");
				}
				break;
			default:
				break;
		}
	}

	protected abstract void Attack1BtnCombo(Player.State state);
}