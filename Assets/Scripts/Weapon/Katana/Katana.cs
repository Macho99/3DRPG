using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : Sword
{
	[SerializeField] State curState;
	[SerializeField] TargetFollower katanaHolderFollower;
	[SerializeField] TargetFollower swordCaseFollower;
	[SerializeField] TargetFollower swordDummyFollower;

	[Serializable]
	public enum State { Inactive, Idle, Unarmed, QuickSheath, Equip, 
		QuickDrawEntry, QuickDrawIdle, QuickDraw1, QuickDraw2, QuickDraw3, QuickDraw4, QuickDraw5, QuickDraw6, QuickDraw7,
		DashAttackVerA, DashAttackVerB,
		DashComboVerA01, DashComboVerA02, DashComboVerA03, DashComboVerA04,
		DashComboVerB01, DashComboVerB02, DashComboVerB03,
		S1Combo01_01, S1Combo01_02, S1Combo01_03,
		S2Combo01_01, S2Combo01_02, S2Combo01_03,
		S2Combo02_01, S2Combo02_02, S2Combo02_03, S2Combo02_04,
		JumpCombo01, JumpCombo02, JumpCombo03, JumpCombo04, JumpCombo05, JumpCombo06, JumpCombo07,
		DodgeAttack, 
		AttackFail };
	private StateMachine<State, Katana> stateMachine;

	public bool Armed { get; set; }
	public int QuickDrawCnt { get; set; } = 0;

	protected override void Awake()
	{
		base.Awake();
		OnMonsterAttack.AddListener(StealMP);
		stateMachine = new StateMachine<State, Katana>(this);
		stateMachine.AddState(State.Inactive, new KatanaInactive(this, stateMachine));
		stateMachine.AddState(State.Idle, new KatanaIdle(this, stateMachine));
		stateMachine.AddState(State.Unarmed, new KatanaUnarmed(this, stateMachine));
		stateMachine.AddState(State.QuickSheath, new KatanaQuickSheath(this, stateMachine));
		stateMachine.AddState(State.Equip, new KatanaEquip(this, stateMachine));
		stateMachine.AddState(State.QuickDrawEntry, new KatanaQuickDrawEntry(this, stateMachine));
		stateMachine.AddState(State.QuickDrawIdle, new KatanaQuickDrawIdle(this, stateMachine));
		stateMachine.AddState(State.QuickDraw1, new KatanaQuickDraw1(this, stateMachine));
		stateMachine.AddState(State.QuickDraw2, new KatanaQuickDraw2(this, stateMachine));
		stateMachine.AddState(State.QuickDraw3, new KatanaQuickDraw3(this, stateMachine));
		stateMachine.AddState(State.QuickDraw4, new KatanaQuickDraw4(this, stateMachine));
		stateMachine.AddState(State.QuickDraw5, new KatanaQuickDraw5(this, stateMachine));
		stateMachine.AddState(State.QuickDraw6, new KatanaQuickDraw6(this, stateMachine));
		stateMachine.AddState(State.QuickDraw7, new KatanaQuickDraw7(this, stateMachine));
		stateMachine.AddState(State.DashAttackVerA, new KatanaDashAttackVerA(this, stateMachine));
		stateMachine.AddState(State.DashAttackVerB, new KatanaDashAttackVerB(this, stateMachine));
		stateMachine.AddState(State.DashComboVerA01, new KatanaDashComboVerA01(this, stateMachine));
		stateMachine.AddState(State.DashComboVerA02, new KatanaDashComboVerA02(this, stateMachine));
		stateMachine.AddState(State.DashComboVerA03, new KatanaDashComboVerA03(this, stateMachine));
		stateMachine.AddState(State.DashComboVerA04, new KatanaDashComboVerA04(this, stateMachine));
		stateMachine.AddState(State.DashComboVerB01, new KatanaDashComboVerB01(this, stateMachine));
		stateMachine.AddState(State.DashComboVerB02, new KatanaDashComboVerB02(this, stateMachine));
		stateMachine.AddState(State.DashComboVerB03, new KatanaDashComboVerB03(this, stateMachine));
		stateMachine.AddState(State.S1Combo01_01, new KatanaS1Combo01_01(this, stateMachine));
		stateMachine.AddState(State.S1Combo01_02, new KatanaS1Combo01_02(this, stateMachine));
		stateMachine.AddState(State.S1Combo01_03, new KatanaS1Combo01_03(this, stateMachine));
		stateMachine.AddState(State.S2Combo01_01, new KatanaS2Combo01_01(this, stateMachine));
		stateMachine.AddState(State.S2Combo01_02, new KatanaS2Combo01_02(this, stateMachine));
		stateMachine.AddState(State.S2Combo01_03, new KatanaS2Combo01_03(this, stateMachine));
		stateMachine.AddState(State.S2Combo02_01, new KatanaS2Combo02_01(this, stateMachine));
		stateMachine.AddState(State.S2Combo02_02, new KatanaS2Combo02_02(this, stateMachine));
		stateMachine.AddState(State.S2Combo02_03, new KatanaS2Combo02_03(this, stateMachine));
		stateMachine.AddState(State.S2Combo02_04, new KatanaS2Combo02_04(this, stateMachine));
		stateMachine.AddState(State.JumpCombo01, new KatanaJumpCombo01(this, stateMachine));
		stateMachine.AddState(State.JumpCombo02, new KatanaJumpCombo02(this, stateMachine));
		stateMachine.AddState(State.JumpCombo03, new KatanaJumpCombo03(this, stateMachine));
		stateMachine.AddState(State.JumpCombo04, new KatanaJumpCombo04(this, stateMachine));
		stateMachine.AddState(State.JumpCombo05, new KatanaJumpCombo05(this, stateMachine));
		stateMachine.AddState(State.JumpCombo06, new KatanaJumpCombo06(this, stateMachine));
		stateMachine.AddState(State.JumpCombo07, new KatanaJumpCombo07(this, stateMachine));
		stateMachine.AddState(State.DodgeAttack, new KatanaDodgeAttack(this, stateMachine));
		stateMachine.AddState(State.AttackFail, new KatanaAttackFail(this, stateMachine));
	}

	public override void Init(WeaponItem weaponItem)
	{
		base.Init(weaponItem);
		katanaHolderFollower.SetTarget(player.GetTransform(Player.FollowTransform.KatanaHolder));
		swordCaseFollower.SetTarget(player.GetTransform(Player.FollowTransform.SwordCase));
		swordDummyFollower.SetTarget(player.GetTransform(Player.FollowTransform.SwordDummy));
	}

	protected override void Start()
	{
		base.Start();
		stateMachine.SetUp(State.Inactive);
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
			swordDummyFollower.Update();
			renderer.gameObject.SetActive(false);
			swordDummyFollower.gameObject.SetActive(true);
		}
		else
		{
			swordDummyFollower.Update();
			renderer.gameObject.SetActive(true);
			swordDummyFollower.gameObject.SetActive(false);
		}
	}

	public void PlayChargeFeedback(int level)
	{
		switch (level)
		{
			case 0:
				FieldSFC.Instance?.PlayCharge(false);
				return;
			case 1:
				FieldSFC.Instance?.PlayCharge(true);
				break;
			case 2:
			case 3:
				break;
			default:
				print($"레벨은 0~3이어야 합니다 {level}");
				return;
		}
		FieldSFC.Instance?.PlayCharge(level);
	}

	public override void ChangeStateToIdle(bool forceIdle = false)
	{
		if(forceIdle == true)
		{
			stateMachine.ChangeState(State.Idle);
			return;
		}

		if(Armed == true)
		{
			stateMachine.ChangeState(State.Idle);
		}
		else
		{
			stateMachine.ChangeState(State.Unarmed);
		}
	}

	public override void ForceInactive()
	{
		stateMachine.ChangeState(State.Inactive);
	}

	private void StealMP()
	{
		GameManager.Stat.AddCurMP(5);
	}
}