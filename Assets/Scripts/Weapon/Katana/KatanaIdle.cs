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
		playerAttack.SetAnimFloat("Armed", 1f);
		playerAttack.OnAttack1Down.AddListener(BtnDownTransition);
		owner.SetDummyRender(false);
		player.WeaponIdle();
	}

	public override void Exit()
	{
		playerAttack.OnAttack1Down.RemoveListener(BtnDownTransition);
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

	}

	private void BtnDownTransition(Player.State state)
	{
		switch(state)
		{
			case Player.State.Idle:
			case Player.State.Walk:
			case Player.State.Run:
				stateMachine.ChangeState(Katana.State.S1Combo01_01);
				break;
		}
	}
}
