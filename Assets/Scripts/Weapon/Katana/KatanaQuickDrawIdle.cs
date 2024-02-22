using System.Collections.Generic;
using UnityEngine;

public class KatanaQuickDrawIdle : StateBase<Katana.State, Katana>
{
	private const float charge1Time = 1f;
	private const float charge2Time = 2f;
	private const float minimumExitTime = 0.2f; 
	//너무 빨리 전환하면 Animator 트리거가 못따라와서 최소 대기시간을 가짐 

	Player player;
	PlayerAttack playerAttack;
	PlayerAnimEvent playerAnimEvent;
	PlayerMove playerMove;

	float enterTime;
	bool attack1DownTriggered;
	float attack1DownTime;
	bool attack1Up;
	bool readyToTransition;

	public KatanaQuickDrawIdle(Katana owner, StateMachine<Katana.State, Katana> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		enterTime = Time.time;
		attack1DownTriggered = false;
		attack1Up = false;
		playerAttack.SetAnimFloat("Grruzam", 0f);
		if(owner.QuickDrawCnt == 0)
		{
			playerAttack.SetAnimTrigger("Hold1");
			readyToTransition = false;
		}
		else if(playerAttack.Attack1Pressed == false)
		{
			playerAttack.SetAnimTrigger("Hold1Idle");
			readyToTransition = true;
		}
		else
		{
			Attack1Down(Player.State.StandAttack);
			readyToTransition = true;
		}
		owner.QuickDrawCnt++;
		playerAttack.OnAttack1Down.AddListener(Attack1Down);
		playerAttack.OnAttack1Up.AddListener(Attack1Up);
		playerAnimEvent.OnEquipChange.AddListener(EquipChange);
	}

	public override void Exit()
	{
		playerAttack.OnAttack1Down.RemoveListener(Attack1Down);
		playerAttack.OnAttack1Up.RemoveListener(Attack1Up);
		playerAnimEvent.OnEquipChange.RemoveListener(EquipChange);
	}

	public override void Setup()
	{
		playerAttack = owner.PlayerAttack;
		player = playerAttack.GetComponent<Player>();
		playerAnimEvent = owner.PlayerAnimEvent;
		playerMove = player.GetComponent<PlayerMove>();
	}

	public override void Transition()
	{
		if (playerMove.MoveInput.sqrMagnitude > 0.1f)
		{
			if (playerAttack.IsAnimName(0, "Hold1Idle"))
			{
				owner.QuickDrawCnt = 0;
				player.ChangeState(Player.State.Idle);
				playerAttack.SetAnimTrigger("BaseExit");
				stateMachine.ChangeState(Katana.State.Unarmed);
				return;
			}
		}

		if (readyToTransition == false) return;
		if (attack1Up == false) return;
		if (Time.time < enterTime + minimumExitTime) return;

		if (attack1DownTime + charge1Time > Time.time)
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
				default:
					Debug.Log("올바르지 않은 QuickDrawCnt : " + owner.QuickDrawCnt);
					break;
			}
		}
		else if (attack1DownTime + charge2Time > Time.time)
		{
			stateMachine.ChangeState(Katana.State.QuickDraw4);
		}
		else
		{
			stateMachine.ChangeState(Katana.State.QuickDraw5);
		}
	}

	public override void Update()
	{

	}

	private void EquipChange()
	{
		owner.SetDummyRender(true);
		readyToTransition = true;
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
		attack1Up = true;
	}
}