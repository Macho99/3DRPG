using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
	[SerializeField] State curState;

	[Serializable]
	public enum State { Idle, Walk, Run, Dodge, Jump, OnAir, Land,
		JumpTest,
		DoubleJump, DoubleOnAir, DoubleLand, 
		StandAttack, OnAirAttack, MoveAttack,
		Stun, Die};

	private Animator anim;
	private MMFollowTarget camRootFollower;
	private PlayerLook playerLook;
	private PlayerMove playerMove;
	private PlayerAttack playerAttack;
	private PlayerAnimEvent playerAnimEvent;
	private StateMachine<State, Player> stateMachine;
	private bool camFollowFixed;

	[HideInInspector] public UnityEvent OnWeaponIdle;

	public Transform MoveRoot { get => playerMove.MoveRoot; }
	public State CurState { get { return curState; } }
	public PlayerLook PlayerLook { get {  return playerLook; } }
	public PlayerMove PlayerMove { get {  return playerMove; } }
	public PlayerAttack PlayerAttack { get {  return playerAttack; } }
	public PlayerAnimEvent PlayerAnimEvent { get { return playerAnimEvent; } }

	private void Awake()
	{
		anim = GetComponent<Animator>();
		playerLook = GetComponent<PlayerLook>();
		playerMove = GetComponent<PlayerMove>();
		playerAttack = GetComponent<PlayerAttack>();
		playerAnimEvent = GetComponent<PlayerAnimEvent>();

		camRootFollower = playerLook.CamRoot.GetComponent<MMFollowTarget>();
		OnWeaponIdle = new UnityEvent();

		stateMachine = new StateMachine<State, Player>(this);
		stateMachine.AddState(State.Idle, new PlayerIdle(this, stateMachine));
		stateMachine.AddState(State.Walk, new PlayerWalk(this, stateMachine));
		stateMachine.AddState(State.Run, new PlayerRun(this, stateMachine));
		stateMachine.AddState(State.Dodge, new PlayerDodge(this, stateMachine));
		stateMachine.AddState(State.Jump, new PlayerJump(this, stateMachine));
		stateMachine.AddState(State.OnAir, new PlayerOnAir(this, stateMachine));
		stateMachine.AddState(State.Land, new PlayerLand(this, stateMachine));
		stateMachine.AddState(State.DoubleJump, new PlayerDoubleJump(this, stateMachine));
		stateMachine.AddState(State.DoubleOnAir, new PlayerDoubleOnAir(this, stateMachine));
		stateMachine.AddState(State.DoubleLand, new PlayerDoubleLand(this, stateMachine));

		stateMachine.AddState(State.StandAttack, new PlayerAttackStand(this, stateMachine));
		stateMachine.AddState(State.MoveAttack, new PlayerAttackMove(this, stateMachine));
		stateMachine.AddState(State.OnAirAttack, new PlayerAttackOnAir(this, stateMachine));

		stateMachine.AddState(State.JumpTest, new PlayerJumpTest(this, stateMachine));
	}

	private void Start()
	{
		stateMachine.SetUp(State.Idle);
	}

	private void Update()
	{
		stateMachine.Update();
		curState = stateMachine.GetCurState();
	}

	public void ChangeState(State newState)
	{
		stateMachine.ChangeState(newState);
	}

	public void WeaponIdle()
	{
		OnWeaponIdle?.Invoke();
	}

	public void SetAnimRootMotion(bool value, bool camSetting = true)
	{
		anim.applyRootMotion = value;
		if (camSetting == false) return;

		if(value == true)
		{
			SetCamFollowSpeed(5f);
		}
		else
		{
			SetCamFollowSpeed(50f, 1f);
		}
	}

	public void SetCamFollowSpeed(float speed)
	{
		camFollowFixed = true;
		camRootFollower.FollowPositionSpeed = 5f;
	}
	
	public void SetCamFollowSpeed(float speed, float lerpSpeed)
	{
		camFollowFixed = false;
		_ = StartCoroutine(CoFollowSpeedSet(speed, lerpSpeed));
	}

	private IEnumerator CoFollowSpeedSet(float target, float lerpSpeed)
	{
		while (camFollowFixed == false)
		{
			camRootFollower.FollowPositionSpeed = 
				Mathf.Lerp(camRootFollower.FollowPositionSpeed, target, lerpSpeed * Time.deltaTime);

			if(camRootFollower.FollowPositionSpeed > target - 10f)
			{
				camRootFollower.FollowPositionSpeed = target;
				break;
			}
			yield return null;
		}
	}

	public void Dodge()
	{
		playerAttack.ChangeStateToIdle(true);
		stateMachine.ChangeState(Player.State.Dodge);
	}

	public void Jump()
	{
		stateMachine.ChangeState(Player.State.Jump);
	}

	public void DoubleJump()
	{
		stateMachine.ChangeState(Player.State.DoubleJump);
	}
	public void OnAir()
	{
		stateMachine.ChangeState(Player.State.OnAir);
	}

}