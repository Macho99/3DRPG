using System.Collections.Generic;
using UnityEngine;

public class BowStartAim : StateBase<Bow.State, Bow>
{
	Player player;
	PlayerMove playerMove;
	PlayerAttack playerAttack;

	float rigWeight;
	public BowStartAim(Bow owner, StateMachine<Bow.State, Bow> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		rigWeight = 0f;
		playerAttack.SetAnimTrigger("UpperHold1");
		playerAttack.OnAttack1Up.AddListener(UndoAim);
		playerAttack.OnAttack2Down.AddListener(UndoAim);
		playerMove.AimLock = true;
	}

	public override void Exit()
	{
		player.SetBowAimRigWeight(1f);
		playerAttack.OnAttack1Up.RemoveListener(UndoAim);
		playerAttack.OnAttack2Down.RemoveListener(UndoAim);
		playerMove.AimLock = false;
	}

	public override void Setup()
	{
		player = owner.Player;
		playerMove = owner.PlayerMove;
		playerAttack = owner.PlayerAttack;
	}

	public override void Transition()
	{
		if(playerAttack.IsAnimName(1, "UpperHold1") == true)
		{
			if (playerAttack.GetAnimNormalizedTime(1) > 0.99f)
			{
				stateMachine.ChangeState(Bow.State.Aiming);
			}
		}
	}

	public override void Update()
	{
		rigWeight = Mathf.Lerp(rigWeight, 1f, Time.deltaTime * 4f);
		player.SetBowAimRigWeight(rigWeight);
	}

	private void UndoAim(Player.State state)
	{
		stateMachine.ChangeState(Bow.State.UndoAim);
	}
}