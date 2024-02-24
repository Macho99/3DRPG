using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PlayerDodge : StateBase<Player.State, Player>
{
	const float dodgeSpeed = 1.5f;
	const float minumTransitionTime = 0.5f;
	float enterTime;
	PlayerMove playerMove;
	PlayerAfterimage afterimage;
	Vector2 moveVec;


	public PlayerDodge(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		enterTime = Time.time;

		Vector3 moveVec3;
		if(playerMove.MoveInput.sqrMagnitude < 0.1f)
		{
			moveVec3 = new Vector3(0f, 0f, -1f);
		}
		else
		{
			moveVec3 = new Vector3(playerMove.MoveInput.x, 0f, playerMove.MoveInput.y);
		}

		Quaternion characterToMoveRootRotation = 
			Quaternion.FromToRotation(owner.transform.forward, owner.MoveRoot.transform.forward);
		moveVec3 = characterToMoveRootRotation * moveVec3;

		moveVec = new Vector2(moveVec3.x, moveVec3.z);

		playerMove.SetAnimFloat("DodgeX", moveVec.x);
		playerMove.SetAnimFloat("DodgeY", moveVec.y);
		playerMove.SetAnimTrigger("Dodge");

		playerMove.GravityScale = 0f;
		playerMove.MoveMultiplier = 0f;
		owner.SetAnimRootMotion(true);
		afterimage.Play();
	}

	public override void Exit()
	{
		playerMove.GravityScale = 1f;
		playerMove.MoveMultiplier = 1f;
		owner.SetAnimRootMotion(false);
	}

	public override void Setup()
	{
		playerMove = owner.PlayerMove;
		afterimage = owner.GetComponent<PlayerAfterimage>();
	}

	public override void Transition()
	{
		if (Time.time < enterTime + minumTransitionTime) return;

		if(playerMove.IsAnimWait(0) == true)
		{
			playerMove.SetAnimTrigger("BaseExit");
			stateMachine.ChangeState(Player.State.Idle);
		}
	}

	public override void Update()
	{
		Vector3 vec = new Vector3();
		vec += owner.transform.right * moveVec.x;
		vec += owner.transform.forward * moveVec.y;
		vec *= dodgeSpeed;
		playerMove.ManualMove(vec, Time.deltaTime);
	}
}