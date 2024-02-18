using System.Collections.Generic;
using UnityEngine;

public class KatanaQuickDrawIdle : StateBase<Katana.State, Katana>
{
	Player player;
	PlayerAttack playerAttack;
	PlayerAnimEvent playerAnimEvent;
	PlayerMove playerMove;
	public KatanaQuickDrawIdle(Katana owner, StateMachine<Katana.State, Katana> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		if(owner.QuickDrawCnt == 0)
		{
			playerAttack.SetAnimTrigger("Hold1");
		}
		else
		{
			playerAttack.SetAnimTrigger("Hold1Idle");
		}
		owner.QuickDrawCnt++;
		playerAttack.OnAttack1Down.AddListener(QuickDrawTransition);
		playerAnimEvent.OnEquipChange.AddListener(EquipChange);
	}

	public override void Exit()
	{
		playerAttack.OnAttack1Down.RemoveListener(QuickDrawTransition);
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
			player.ChangeState(Player.State.Idle);
			playerAttack.SetAnimTrigger("BaseExit");
			stateMachine.ChangeState(Katana.State.Unarmed);
			return;
		}
	}

	public override void Update()
	{

	}

	private void EquipChange()
	{
		owner.SetDummyRender(true);
	}

	private void QuickDrawTransition(Player.State state)
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
				owner.QuickDrawCnt = 1;
				break;
			default:
				Debug.Log("올바르지 않은 QuickDrawCnt : " + owner.QuickDrawCnt);
				break;
		}
	}
}