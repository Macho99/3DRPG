using System.Collections.Generic;
using UnityEngine;

public class PlayerDoubleOnAir : StateBase<Player.State, Player>
{
	PlayerMove playerMove;

	public PlayerDoubleOnAir(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		owner.SetCamFollowSpeed(5f);
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
		if (playerMove.MoveInput.sqrMagnitude > 0.1f)
		{
			if (time < 0.05f || playerMove.IsGround == true)
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
	}

	public override void Update()
	{

	}
}