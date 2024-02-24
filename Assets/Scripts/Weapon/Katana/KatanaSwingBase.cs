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
	protected PlayerMove playerMove;
	protected TargetFollower curTrail;

	protected AttackProcess curAttackProcess;
	protected string triggerName;
	protected bool attack1Pressed;
	protected bool attack2Up;
	protected bool attack1Holded;
	protected bool attack2Holded;

	protected int maxAttackNum;
	protected int curAttackNum;
	private Player.State attackMode;
	private string exitTriggerName;

	public KatanaSwingBase(Katana owner, StateMachine<Katana.State, Katana> stateMachine,
		string triggerName, Player.State attackMode, int maxAttackNum = 1)
		: base(owner, stateMachine)
	{
		this.triggerName = triggerName;
		this.maxAttackNum = maxAttackNum;
		switch (attackMode)
		{
			case Player.State.StandAttack:
			case Player.State.MoveAttack:
				exitTriggerName = "BaseExit";
				break;
			case Player.State.OnAirAttack:
				exitTriggerName = "OnAirAttackEnd";
				break;
			default:
				Debug.LogError($"{attackMode}는 유효하지 않습니다");
				return;
		}
		this.attackMode = attackMode;
	}

	public override void Enter()
	{
		curAttackNum = 0;
		attack1Pressed = false;
		attack2Up = false;
		attack1Holded = false;
		attack2Holded = false;
		curAttackProcess = AttackProcess.BeforeAttack;
		playerAttack.SetAnimTrigger(triggerName);
		playerAnimEvent.OnAttackStart.AddListener(AttackStart);
		playerAnimEvent.OnAttackEnd.AddListener(AttackEnd);
		playerAttack.OnAttack1Down.AddListener(AttackBtn1Pressed);
		playerAttack.OnAttack2Up.AddListener(AttackBtn2Up);
		playerAttack.OnAttack1Hold.AddListener(AttackBtn1Hold);
		playerAttack.OnAttack2Hold.AddListener(AttackBtn2Hold);
		player.ChangeState(attackMode);
	}

	public override void Exit()
	{
		curTrail?.SetTarget(null);
		curTrail = null;
		playerAnimEvent.OnAttackStart.RemoveListener(AttackStart);
		playerAnimEvent.OnAttackEnd.RemoveListener(AttackEnd);
		playerAttack.OnAttack1Down.RemoveListener(AttackBtn1Pressed);
		playerAttack.OnAttack2Up.RemoveListener(AttackBtn2Up);
		playerAttack.OnAttack1Hold.RemoveListener(AttackBtn1Hold);
		playerAttack.OnAttack2Hold.RemoveListener(AttackBtn2Hold);
	}

	protected virtual void AttackStart()
	{
		curAttackNum++;
		curAttackProcess = AttackProcess.Attacking;
		curTrail = owner.BeginAttack();
	}

	private void AttackEnd()
	{
		curTrail.SetTarget(null);
		if(curAttackNum == maxAttackNum)
		{
			curAttackProcess = AttackProcess.AfterAttack;
		}
	}

	public override void Setup()
	{
		playerAttack = owner.PlayerAttack;
		playerAnimEvent = owner.PlayerAnimEvent;
		player = playerAttack.GetComponent<Player>();
		playerMove = player.GetComponent<PlayerMove>();
	}

	public override void Transition()
	{
		if (curAttackProcess == AttackProcess.End)
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
		switch (curAttackProcess)
		{
			case AttackProcess.BeforeAttack:
				break;
			case AttackProcess.Attacking:
				bool result = owner.Attack();
				if (result == false)
				{
					AttackFail();
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
					curAttackProcess = AttackProcess.End;
					playerAttack.SetAnimTrigger(exitTriggerName);
				}
				break;
			default:
				break;
		}
	}

	protected virtual void AttackFail()
	{
		curTrail.SetTarget(null);
		stateMachine.ChangeState(Katana.State.AttackFail);
	}

	protected void AttackBtn1Pressed(Player.State state)
	{
		attack1Pressed = true;
	}

	protected void AttackBtn2Up(Player.State state)
	{
		attack2Up = true;
	}

	protected void AttackBtn1Hold(Player.State state)
	{
		attack1Holded = true;
	}

	private void AttackBtn2Hold(Player.State state)
	{
		attack2Holded = true;
	}

	protected abstract bool CheckTransition();
}