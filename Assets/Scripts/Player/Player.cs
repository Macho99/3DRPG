using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] string curState;
	public enum State { Idle, Walk, Run, Dash, Jump, OnAir, 
		StandAttack, OnAirAttack, MoveAttack,
		Stun, Die};

	private PlayerLook playerLook;
	private PlayerMove playerMove;
	private PlayerAttack playerAttack;
	private StateMachine<State, Player> stateMachine;

	public PlayerLook PlayerLook { get {  return playerLook; } }
	public PlayerMove PlayerMove { get {  return playerMove; } }
	public PlayerAttack PlayerAttack { get {  return playerAttack; } }

	private void Awake()
	{
		playerLook = GetComponent<PlayerLook>();
		playerMove = GetComponent<PlayerMove>();

		stateMachine = new StateMachine<State, Player>(this);
		stateMachine.AddState(State.Idle, new PlayerIdle(this, stateMachine));
		stateMachine.AddState(State.Walk, new PlayerWalk(this, stateMachine));
		stateMachine.AddState(State.Run, new PlayerRun(this, stateMachine));
		//stateMachine.AddState(State.Jump, new )
	}

	private void Start()
	{
		stateMachine.SetUp(State.Idle);
	}

	private void Update()
	{
		stateMachine.Update();
		curState = stateMachine.GetCurStateStr();
	}
}