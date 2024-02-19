using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
	[SerializeField] State curState;

	[Serializable]
	public enum State { Idle, Walk, Run, Dash, Jump, OnAir, 
		StandAttack, OnAirAttack, MoveAttack,
		Stun, Die};

	private Animator anim;
	private PlayerLook playerLook;
	private PlayerMove playerMove;
	private PlayerAttack playerAttack;
	private StateMachine<State, Player> stateMachine;

	[HideInInspector] public UnityEvent OnWeaponIdle;

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

		OnWeaponIdle = new UnityEvent();

		stateMachine = new StateMachine<State, Player>(this);
		stateMachine.AddState(State.Idle, new PlayerIdle(this, stateMachine));
		stateMachine.AddState(State.Walk, new PlayerWalk(this, stateMachine));
		stateMachine.AddState(State.Run, new PlayerRun(this, stateMachine));
		//stateMachine.AddState(State.Jump, new )

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
	}
}