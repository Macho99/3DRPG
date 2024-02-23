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
		playerMove.MoveMultiplier = 0f;
		playerMove.GravityMultiplier = 0.2f;
		playerMove.OnAirAttack();
		owner.OnWeaponIdle.AddListener(ChangeToIdle);
	}

	public override void Exit()
	{
		owner.SetAnimRootMotion(false);
		playerMove.MoveMultiplier = 1f;
		playerMove.GravityMultiplier = 1f;
		owner.OnWeaponIdle.RemoveListener(ChangeToIdle);
	}

	public override void Setup()
	{
		playerMove = owner.PlayerMove;
		playerAttack = owner.PlayerAttack;
	}

	public override void Transition()
	{

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
		playerMove.SetAnimTrigger("Fall");
		stateMachine.ChangeState(Player.State.DoubleOnAir);
	}
}
