using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerAttackStand : StateBase<Player.State, Player>
{
	PlayerMove playerMove;
	PlayerAttack playerAttack;
	Vector3 moveForward;

	public PlayerAttackStand(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		moveForward = playerMove.MoveForward;
		owner.SetAnimRootMotion(true);
		playerMove.MoveMultiplier = 0f;
		owner.OnWeaponIdle.AddListener(ChangeToIdle);
	}

	public override void Exit()
	{
		owner.SetAnimRootMotion(false);
		playerMove.MoveMultiplier = 1f;
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
		stateMachine.ChangeState(Player.State.Idle);
	}
}
