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
		playerMove.SetAnimFloat("Grruzam", 1f);
		playerMove.SetAnimFloat("Jump", 2f);
		owner.CurJumpState = Player.JumpState.OnAir;
	}

	public override void Exit()
	{
		owner.SetCamFollowSpeed(50f, 1f);
	}

	public override void Setup()
	{
		playerMove = owner.PlayerMove;
	}

	public override void Transition()
	{
		float time = playerMove.CalcLandTime();
		print(time);
		if(time < 0.5f)
		{
			playerMove.SetAnimFloat("Jump", 3f);
			stateMachine.ChangeState(Player.State.Idle);
		}
	}

	public override void Update()
	{

	}
}