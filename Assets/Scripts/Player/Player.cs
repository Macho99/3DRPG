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
	public enum JumpState { None, Jump, OnAir, Land, DoubleJump, DoubleOnAir, DoubleLand }

	[Serializable]
	public enum State { Idle, Walk, Run, Dodge, OnAir, DoubleJump, 
		StandAttack, OnAirAttack, MoveAttack,
		Stun, Die};

	public JumpState CurJumpState { get { return curJumpState; } set { curJumpState = value; } }
	[SerializeField] private JumpState curJumpState;
	private Animator anim;
	private MMFollowTarget camRootFollower;
	private PlayerLook playerLook;
	private PlayerMove playerMove;
	private PlayerAttack playerAttack;
	private StateMachine<State, Player> stateMachine;
	private Coroutine followSpeedCoroutine;

	[HideInInspector] public UnityEvent OnWeaponIdle;

	public Transform MoveRoot { get => playerMove.MoveRoot; }
	//public bool AimHolded {  get; set; }
	public State CurState { get { return curState; } }
	public PlayerLook PlayerLook { get {  return playerLook; } }
	public PlayerMove PlayerMove { get {  return playerMove; } }
	public PlayerAttack PlayerAttack { get {  return playerAttack; } }

	private void Awake()
	{
		anim = GetComponent<Animator>();
		playerLook = GetComponent<PlayerLook>();
		playerMove = GetComponent<PlayerMove>();
		playerAttack = GetComponent<PlayerAttack>();

		camRootFollower = playerLook.CamRoot.GetComponent<MMFollowTarget>();
		OnWeaponIdle = new UnityEvent();

		stateMachine = new StateMachine<State, Player>(this);
		stateMachine.AddState(State.Idle, new PlayerIdle(this, stateMachine));
		stateMachine.AddState(State.Walk, new PlayerWalk(this, stateMachine));
		stateMachine.AddState(State.Run, new PlayerRun(this, stateMachine));
		stateMachine.AddState(State.Dodge, new PlayerDodge(this, stateMachine));
		stateMachine.AddState(State.OnAir, new PlayerOnAir(this, stateMachine));

		stateMachine.AddState(State.StandAttack, new PlayerAttackStand(this, stateMachine));
		stateMachine.AddState(State.MoveAttack, new PlayerAttackMove(this, stateMachine));
		stateMachine.AddState(State.OnAirAttack, new PlayerAttackOnAir(this, stateMachine));
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

	public void SetAnimRootMotion(bool value)
	{
		anim.applyRootMotion = value;
		if(value == true)
		{
			camRootFollower.FollowPositionSpeed = 5f;
			if(followSpeedCoroutine != null)
				StopCoroutine(followSpeedCoroutine);
		}
		else
		{
			followSpeedCoroutine = StartCoroutine(CoFollowSpeedSet(50f));
		}
	}

	private IEnumerator CoFollowSpeedSet(float target)
	{
		while (true)
		{
			camRootFollower.FollowPositionSpeed = 
				Mathf.Lerp(camRootFollower.FollowPositionSpeed, target, 5f * Time.deltaTime);

			if(Mathf.Approximately(camRootFollower.FollowPositionSpeed, target))
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
		OnAir(true);
	}

	public void OnAir(bool jumpUp)
	{
		if (jumpUp == true) {
			CurJumpState = JumpState.Jump;
		}
		else
		{
			CurJumpState = JumpState.OnAir;
		}
		
		stateMachine.ChangeState(Player.State.OnAir);
	}
}