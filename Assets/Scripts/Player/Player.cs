using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] string curState;
	public enum State { Idle, Walk, Run, Dash, Jump, OnAir, 
		StandAttack, OnAirAttack, MoveAttack,
		Stun, Die};

	private StateMachine<State, Player> stateMachine;

	private void Awake()
	{
		stateMachine = new StateMachine<State, Player>(this);
		stateMachine.AddState(State.Idle, new PlayerIdle(this, stateMachine));
	}
}