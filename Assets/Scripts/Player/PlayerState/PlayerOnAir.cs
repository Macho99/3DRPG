using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerOnAir : StateBase<Player.State, Player>
{
	PlayerMove playerMove;

	public PlayerOnAir(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		playerMove.OnJumpDown.AddListener(DoubleJump);
		owner.SetCamFollowSpeed(5f);
	}

	public override void Exit()
	{
		playerMove.OnJumpDown.RemoveListener(DoubleJump);
	}

	public override void Setup()
	{
		playerMove = owner.PlayerMove;
	}

	public override void Transition()
	{
		float time = playerMove.CalcLandTime();
		if (time < 0.05f || playerMove.IsGround == true)
		{
			stateMachine.ChangeState(Player.State.Land);
		}
	}

	public override void Update()
	{

	}
	public void DoubleJump()
	{
		if (owner.DoubleJumped == false)
		{
			stateMachine.ChangeState(Player.State.DoubleJump);
		}
	}
}