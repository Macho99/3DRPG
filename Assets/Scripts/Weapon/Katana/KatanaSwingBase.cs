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
	protected bool attack1Pressed;
	protected bool attack2Pressed;
	protected bool attack1Holded;
	protected bool attack2Holded;

	public KatanaSwingBase(Katana owner, StateMachine<Katana.State, Katana> stateMachine, string triggerName) : base(owner, stateMachine)
	{
		this.triggerName = triggerName;
	}

	public override void Enter()
	{
		attack1Pressed = false;
		attack2Pressed = false;
		attack1Holded = false;
		attack2Holded = false;
		curAttack = AttackProcess.BeforeAttack;
		playerAttack.SetAnimTrigger(triggerName);
		playerAnimEvent.OnAttackStart.AddListener(AttackStart);
		playerAnimEvent.OnAttackEnd.AddListener(AttackEnd);
		playerAttack.OnAttack1Down.AddListener(AttackBtn1Pressed);
		playerAttack.OnAttack2Down.AddListener(AttackBtn2Pressed);
		playerAttack.OnAttack1Hold.AddListener(AttackBtn1Hold);
		playerAttack.OnAttack2Hold.AddListener(AttackBtn2Hold);
	}

	public override void Exit()
	{
		trail = null;
		playerAnimEvent.OnAttackStart.RemoveListener(AttackStart);
		playerAnimEvent.OnAttackEnd.RemoveListener(AttackEnd);
		playerAttack.OnAttack1Down.RemoveListener(AttackBtn1Pressed);
		playerAttack.OnAttack2Down.RemoveListener(AttackBtn2Pressed);
		playerAttack.OnAttack1Hold.RemoveListener(AttackBtn1Hold);
		playerAttack.OnAttack2Hold.RemoveListener(AttackBtn2Hold);
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
				if(CheckTransition() == true)
				{
					return;
				}
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

	protected void AttackBtn1Pressed(Player.State state)
	{
		attack1Pressed = true;
	}

	protected void AttackBtn2Pressed(Player.State state)
	{
		attack2Pressed = true;
	}

	protected void AttackBtn1Hold(Player.State state)
	{
		attack1Holded = true;
	}

	private void AttackBtn2Hold(Player.State arg0)
	{
		attack2Holded = true;
	}

	protected abstract bool CheckTransition();
}