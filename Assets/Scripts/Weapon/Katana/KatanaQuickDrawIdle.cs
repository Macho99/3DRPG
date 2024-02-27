using System.Collections.Generic;
using UnityEngine;

public class KatanaQuickDrawIdle : StateBase<Katana.State, Katana>
{
	private const float charge1Time = 1.5f;
	private const float charge2Time = 3f;
	private const float minimumExitTime = 0.15f; 
	//너무 빨리 전환하면 Animator 트리거가 못따라와서 최소 대기시간을 가짐

	Player player;
	PlayerAttack playerAttack;
	PlayerMove playerMove;

	int chargeLevel;
	float enterTime;
	float attack1DownTime;
	bool attack1DownTriggered;
	bool attack1Up;

	public KatanaQuickDrawIdle(Katana owner, StateMachine<Katana.State, Katana> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		chargeLevel = 0;
		enterTime = Time.time;
		attack1DownTriggered = false;
		attack1Up = false;

		playerAttack.SetAnimFloat("IdleAdapter", 0f);

		if(playerAttack.Attack1Pressed == false)
		{
			playerAttack.SetAnimTrigger("Hold1Idle");
		}
		else
		{
			Attack1Down(Player.State.StandAttack);
		}
		owner.QuickDrawCnt++;
		playerAttack.OnAttack1Down.AddListener(Attack1Down);
		playerAttack.OnAttack1Up.AddListener(Attack1Up);
	}

	public override void Exit()
	{
		playerAttack.OnAttack1Down.RemoveListener(Attack1Down);
		playerAttack.OnAttack1Up.RemoveListener(Attack1Up);
		if(chargeLevel != 0)
			owner.PlayChargeFeedback(0);
	}

	public override void Setup()
	{
		playerAttack = owner.PlayerAttack;
		player = owner.Player;
		playerMove = player.GetComponent<PlayerMove>();
	}

	public override void Transition()
	{
		if (playerMove.MoveInput.sqrMagnitude > 0.1f)
		{
			owner.QuickDrawCnt = 0;
			player.ChangeState(Player.State.Idle);
			playerAttack.SetAnimTrigger("BaseExit");
			stateMachine.ChangeState(Katana.State.Unarmed);
			return;
		}

		if (attack1DownTriggered == false) return;
		if (attack1Up == false) return;
		if (Time.time < enterTime + minimumExitTime) return;

		if (Time.time < attack1DownTime + charge1Time)
		{
			switch (owner.QuickDrawCnt)
			{
				case 1:
					stateMachine.ChangeState(Katana.State.QuickDraw1);
					break;
				case 2:
					stateMachine.ChangeState(Katana.State.QuickDraw2);
					break;
				case 3:
					stateMachine.ChangeState(Katana.State.QuickDraw3);
					break;
				case 4:
					stateMachine.ChangeState(Katana.State.QuickDraw4);
					break;
				default:
					Debug.Log("올바르지 않은 QuickDrawCnt : " + owner.QuickDrawCnt);
					break;
			}
		}
		else if (attack1DownTime + charge2Time > Time.time)
		{
			stateMachine.ChangeState(Katana.State.QuickDraw5);
		}
		else
		{
			stateMachine.ChangeState(Katana.State.QuickDraw6);
		}
	}

	public override void Update()
	{
		if (attack1DownTriggered == false) return;
		switch (chargeLevel)
		{
			case 0:
				chargeLevel++;
				owner.PlayChargeFeedback(1);
				break;
			case 1:
				if (Time.time > attack1DownTime + charge1Time)
				{
					chargeLevel++;
					owner.PlayChargeFeedback(chargeLevel);
				}
				break;
			case 2:
				if (Time.time > attack1DownTime + charge2Time)
				{
					chargeLevel++;
					owner.PlayChargeFeedback(chargeLevel);
				}
				break;
		}
	}

	private void Attack1Down(Player.State state)
	{
		if(attack1DownTriggered == false)
		{
			attack1DownTriggered = true;
			playerAttack.SetAnimTrigger("Hold2");
			attack1DownTime = Time.time;
		}
	}

	private void Attack1Up(Player.State state)
	{
		if (attack1DownTriggered == false)
			return;

		attack1Up = true;
	}
}