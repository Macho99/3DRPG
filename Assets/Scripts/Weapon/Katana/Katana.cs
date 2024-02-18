using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : Sword
{
	[SerializeField] State curState;

	[Serializable]
	public enum State { Idle, Unarmed, QuickSheath, Equip, 
		QuickDrawIdle, QuickDraw1, QuickDraw2, QuickDraw3, QuickDraw4, 
		S1Combo01_01, S1Combo01_02, S1Combo01_03,
		AttackFail };
	private GameObject swordDummy;
	private StateMachine<State, Katana> stateMachine;

	public int QuickDrawCnt { get; set; } = 0;

	protected override void Awake()
	{
		base.Awake();
		stateMachine = new StateMachine<State, Katana>(this);
		stateMachine.AddState(State.Idle, new KatanaIdle(this, stateMachine));
		stateMachine.AddState(State.Unarmed, new KatanaUnarmed(this, stateMachine));
		stateMachine.AddState(State.QuickSheath, new KatanaQuickSheath(this, stateMachine));
		stateMachine.AddState(State.Equip, new KatanaEquip(this, stateMachine));
		stateMachine.AddState(State.QuickDrawIdle, new KatanaQuickDrawIdle(this, stateMachine));
		stateMachine.AddState(State.QuickDraw1, new KatanaQuickDraw1(this, stateMachine));
		stateMachine.AddState(State.QuickDraw2, new KatanaQuickDraw2(this, stateMachine));
		stateMachine.AddState(State.QuickDraw3, new KatanaQuickDraw3(this, stateMachine));
		stateMachine.AddState(State.QuickDraw4, new KatanaQuickDraw4(this, stateMachine));
		stateMachine.AddState(State.S1Combo01_01, new KatanaS1Combo01_01(this, stateMachine));
		stateMachine.AddState(State.S1Combo01_02, new KatanaS1Combo01_02(this, stateMachine));
		stateMachine.AddState(State.S1Combo01_03, new KatanaS1Combo01_03(this, stateMachine));
		stateMachine.AddState(State.AttackFail, new KatanaAttackFail(this, stateMachine));
	}

	protected override void Start()
	{
		base.Start();
		swordDummy = playerAttack.SwordDummy;
		stateMachine.SetUp(State.Idle);
	}

	private void Update()
	{
		stateMachine.Update();
		curState = stateMachine.GetCurState();
	}

	public override void SetUnArmed()
	{
		if(curState == State.Idle)
		{
			stateMachine.ChangeState(State.QuickSheath);
		}
	}

	public void SetDummyRender(bool value)
	{
		if(value == true)
		{
			renderer.gameObject.SetActive(false);
			swordDummy.gameObject.SetActive(true);
		}
		else
		{
			renderer.gameObject.SetActive(true);
			swordDummy.gameObject.SetActive(false);
		}
	}
}