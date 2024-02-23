using System.Collections.Generic;
using UnityEngine;

public class PlayerDoubleJump : StateBase<Player.State, Player>
{
	PlayerMove playerMove;
	public PlayerDoubleJump(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		playerMove.Jump(true);
		owner.SetCamFollowSpeed(5f);
		playerMove.SetAnimTrigger("DoubleJump");
	}

	public override void Exit()
	{
	}

	public override void Setup()
	{
		playerMove = owner.PlayerMove;
	}

	public override void Transition()
	{
		float time = playerMove.CalcLandTime();
		if(playerMove.MoveInput.sqrMagnitude > 0.1f)
		{
			if(time < 0.05f || playerMove.IsGround == true)
			{
				stateMachine.ChangeState(Player.State.BreakFall);
			}
		}
		else
		{
			if (time < 0.2f || playerMove.IsGround == true)
			{
				stateMachine.ChangeState(Player.State.DoubleLand);
				return;
			}
		}

		if (playerMove.IsAnimName(0, "Double_Jump_Loop") == true)
		{
			stateMachine.ChangeState(Player.State.DoubleOnAir);
			return;
		}
	}

	public override void Update()
	{

	}
}