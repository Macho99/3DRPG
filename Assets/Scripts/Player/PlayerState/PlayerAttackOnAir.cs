using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerAttackOnAir : StateBase<Player.State, Player>
{
	PlayerMove playerMove;
	PlayerAttack playerAttack;
	Vector3 moveForward;
	public PlayerAttackOnAir(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		moveForward = playerMove.MoveForward;
		owner.SetAnimRootMotion(true);
		playerMove.MoveMultiplier = 0.5f;
		playerMove.GravityScale = 0.5f;
		playerMove.OnAirAttackJump();
		owner.OnWeaponIdle.AddListener(ChangeToIdle);
	}

	public override void Exit()
	{
		owner.SetAnimRootMotion(false);
		playerMove.MoveMultiplier = 1f;
		playerMove.GravityScale = 1f;
		owner.OnWeaponIdle.RemoveListener(ChangeToIdle);
	}

	public override void Setup()
	{
		playerMove = owner.PlayerMove;
		playerAttack = owner.PlayerAttack;
	}

	public override void Transition()
	{
		float time = playerMove.CalcLandTime();
		if (time < 0.05f || playerMove.IsGround == true)
		{
			playerMove.SetAnimTrigger("FastLand");
			stateMachine.ChangeState(Player.State.Land);
			playerAttack.ChangeStateToIdle();
		}
	}

	public override void Update()
	{
		owner.transform.rotation = Quaternion.Lerp(
			owner.transform.rotation,
			Quaternion.LookRotation(new Vector3(moveForward.x, 0f, moveForward.z)),
			Time.deltaTime * 10f);
	}

	private void ChangeToIdle()
	{
		stateMachine.ChangeState(Player.State.OnAir);
	}
}
