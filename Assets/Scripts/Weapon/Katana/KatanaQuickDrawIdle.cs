﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaQuickDrawIdle : StateBase<Katana.State, Katana>
{
	private const float charge1Time = 1.5f;
	private const float charge2Time = 3f;

	Player player;
	PlayerAttack playerAttack;
	PlayerMove playerMove;

	int chargeLevel;
	float enterTime;
	float attack1DownTime;
	bool attack1DownTriggered;
	bool attack1Up;
	bool waitAnim;

	public KatanaQuickDrawIdle(Katana owner, StateMachine<Katana.State, Katana> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		waitAnim = false;
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

	private void ReturnToIdle()
	{
		owner.QuickDrawCnt = 0;
		player.ChangeState(Player.State.Idle);
		playerAttack.SetAnimTrigger("BaseExit");
		stateMachine.ChangeState(Katana.State.Unarmed);
	}

	public override void Transition()
	{
		if (playerMove.MoveInput.sqrMagnitude > 0.1f)
		{
			ReturnToIdle();
			return;
		}

		if (attack1DownTriggered == false) return;
		if (attack1Up == false) return;

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
			if(GameManager.Stat.TrySubCurMP(50) == false)
			{
				ReturnToIdle();
				return;
			}
			stateMachine.ChangeState(Katana.State.QuickDraw5);
		}
		else
		{
			if (GameManager.Stat.TrySubCurMP(70) == false)
			{
				ReturnToIdle();
				return;
			}
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
		if (waitAnim == true) return;

		if(attack1DownTriggered == false)
		{
			playerAttack.SetAnimTrigger("Hold2");
			attack1DownTime = Time.time;
			waitAnim = true;
			_ = owner.StartCoroutine(CoWaitAnim());
		}
	}

	private IEnumerator CoWaitAnim()
	{
		yield return new WaitUntil(() => playerAttack.IsAnimName(0, "Hold2"));
		attack1DownTriggered = true;
	}

	private void Attack1Up(Player.State state)
	{
		attack1Up = true;
	}
}