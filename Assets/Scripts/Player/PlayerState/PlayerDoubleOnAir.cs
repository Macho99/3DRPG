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
		owner.SetCamFollowSpeed(50f, 1f);
	}

	public override void Setup()
	{
		playerMove = owner.PlayerMove;
	}

	public override void Transition()
	{
		float time = playerMove.CalcLandTime();
		if (time < 0.4f)
		{
			playerMove.SetAnimTrigger("Land");
			playerMove.SetAnimFloat("Grruzam", 1f);
			stateMachine.ChangeState(Player.State.Idle);
		}
	}

	public override void Update()
	{

	}
}