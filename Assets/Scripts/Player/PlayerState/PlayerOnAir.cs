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
		switch (owner.CurJumpState)
		{
			case Player.JumpState.Jump:
				playerMove.Jump();
				playerMove.SetAnimFloat("Jump", 1f);
				break;
			case Player.JumpState.OnAir:
				if (playerMove.CalcLandTime() > 0.1f)
				{
					playerMove.SetAnimFloat("Jump", 2f);
				}
				break;
			default:
				Debug.Log($"{owner.CurJumpState}는 OnAir상태에서 유효하지 않습니다");
				break;
		}
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
		if(time < 0.5f)
		{
			stateMachine.ChangeState(Player.State.Idle);
		}
	}

	public override void Update()
	{

	}
}