using MoreMountains.Feedbacks;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PlayerDodge : StateBase<Player.State, Player>
{
	const float dodgeSpeed = 1.5f;
	const float minimumTransitionTime = 0.5f;
	float enterTime;
	PlayerMove playerMove;
	PlayerAttack playerAttack;
	PlayerAfterimage playerAfterimage;
	PlayerAnimEvent playerAnimEvent;
	Vector2 moveVec;

	bool attack1Pressed;

	public PlayerDodge(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		enterTime = Time.time;
		Time.timeScale = 0.2f;

		SetAnim();

		attack1Pressed = false;

		playerAnimEvent.OnLandEnd.AddListener(CheckDodgeAttack);
		playerMove.GravityScale = 0f;
		playerMove.MoveMultiplier = 0f;
		owner.SetAnimRootMotion(true);
		playerAfterimage.Play();

		playerAttack.OnAttack1Down.AddListener(CheckAttackPressed);
	}

	public override void Exit()
	{
		playerMove.SetAnimFloat("IdleAdapter", -1f);
		playerAnimEvent.OnLandEnd.RemoveListener(CheckDodgeAttack);
		playerAttack.OnAttack1Down.RemoveListener(CheckAttackPressed);
		Time.timeScale = 1f;
		playerMove.GravityScale = 1f;
		playerMove.MoveMultiplier = 1f;
		owner.SetAnimRootMotion(false);
	}

	public override void Setup()
	{
		playerAttack = owner.PlayerAttack;
		playerMove = owner.PlayerMove;
		playerAfterimage = owner.GetComponent<PlayerAfterimage>();
		playerAnimEvent = owner.PlayerAnimEvent;
	}

	public override void Transition()
	{
		if (Time.time < enterTime + minimumTransitionTime) return;

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

		Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, Time.fixedDeltaTime);
	}

	private void CheckDodgeAttack()
	{
		//if (playerAfterimage에서 공격 받았는지 체크 == true)
		{
			if(attack1Pressed == true)
			{
				owner.DodgeAttackStart();
			}
		}
	}

	private void SetAnim()
	{

		Vector3 moveVec3;
		if (playerMove.MoveInput.sqrMagnitude < 0.1f)
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
	}

	private void CheckAttackPressed(Player.State state)
	{
		attack1Pressed = true;
	}
}