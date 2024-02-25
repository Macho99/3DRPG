using System.Collections.Generic;
using UnityEngine;

public class KatanaIdle : StateBase<Katana.State, Katana>
{
	Player player;
	PlayerAttack playerAttack;

	public KatanaIdle(Katana owner, StateMachine<Katana.State, Katana> stateMachine) : base(owner, stateMachine)
	{

	}

	public override void Enter()
	{
		owner.Armed = true;
		owner.QuickDrawCnt = 0;
		playerAttack.SetAnimFloat("Armed", 1f);
		playerAttack.OnAttack1Down.AddListener(BtnDownTransition);
		owner.SetDummyRender(false);
		player.WeaponIdle();
		player.OnDodgeAttackStart.AddListener(DodgeAttack);
	}

	public override void Exit()
	{
		playerAttack.OnAttack1Down.RemoveListener(BtnDownTransition);
		player.OnDodgeAttackStart.RemoveListener(DodgeAttack);
	}

	public override void Setup()
	{
		playerAttack = owner.PlayerAttack;
		player = playerAttack.GetComponent<Player>();
	}

	public override void Transition()
	{

	}

	public override void Update()
	{
		playerAttack.SetAnimFloat("IdleAdapter", 0f, 0.1f, Time.deltaTime);
	}

	private void BtnDownTransition(Player.State state)
	{
		switch(state)
		{
			case Player.State.Idle:
				stateMachine.ChangeState(Katana.State.S1Combo01_01);
				break;
			case Player.State.Walk:
				stateMachine.ChangeState(Katana.State.S2Combo01_01);
				break;
			case Player.State.Run:
				stateMachine.ChangeState(Katana.State.DashAttackVerA);
				break;
			case Player.State.OnAir:
			case Player.State.DoubleJump:
			case Player.State.DoubleOnAir:
				stateMachine.ChangeState(Katana.State.JumpCombo01); 
				break;
		}
	}

	private void DodgeAttack()
	{
		stateMachine.ChangeState(Katana.State.DodgeAttack);
	}
}