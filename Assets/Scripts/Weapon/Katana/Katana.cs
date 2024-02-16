using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : Sword
{
	public enum State { Idle, S1Combo01_01 };

	private StateMachine<State, Katana> stateMachine;

	protected override void Awake()
	{
		base.Awake();
		stateMachine = new StateMachine<State, Katana>(this);
		stateMachine.AddState(State.Idle, new KatanaIdle(this, stateMachine));
	}
}
