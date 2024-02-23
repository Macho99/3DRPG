using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpTest : StateBase<Player.State, Player>
{
	PlayerMove playerMove;
	PlayerAnimEvent playerAnimEvent;
	public PlayerJumpTest(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		playerMove.SetAnimTrigger("DoubleJump");
		playerAnimEvent.OnJumpStart.AddListener(Jump);
		owner.SetCamFollowSpeed(5f);
	}

	public override void Exit()
	{
		owner.SetCamFollowSpeed(50f, 1f);
		playerMove.SetAnimFloat("Grruzam", 1f);
		playerAnimEvent.OnJumpStart.RemoveListener(owner.Jump);
	}

	public override void Setup()
	{
		playerMove = owner.PlayerMove;
		playerAnimEvent = owner.PlayerAnimEvent;
	}

	public override void Transition()
	{
		if (playerMove.IsAnimName(0, "Double_Jump_Loop"))
		{
			float time = playerMove.CalcLandTime();
			if (time < 0.05f)
			{
				stateMachine.ChangeState(Player.State.DoubleLand);
			}
		}
	}

	public override void Update()
	{

	}

	private void Jump()
	{
		playerMove.Jump();
	}
}