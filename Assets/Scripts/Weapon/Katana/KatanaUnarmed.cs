using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class KatanaUnarmed : StateBase<Katana.State, Katana>
{
	PlayerAttack playerAttack;
	public KatanaUnarmed(Katana owner, StateMachine<Katana.State, Katana> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		owner.Armed = false;
		playerAttack.SetAnimFloat("Armed", 0f);
		owner.SetDummyRender(true);
		playerAttack.OnAttack1Down.AddListener(Equip);
	}

	public override void Exit()
	{
		playerAttack.OnAttack1Down.RemoveListener(Equip);
	}

	public override void Setup()
	{
		playerAttack = owner.PlayerAttack;
	}

	public override void Transition()
	{

	}

	public override void Update()
	{

	}

	private void Equip(Player.State state)
	{
		switch (state)
		{
			case Player.State.Idle:
			case Player.State.Walk:
				stateMachine.ChangeState(Katana.State.Equip);
				break;
			case Player.State.Run:
				stateMachine.ChangeState(Katana.State.DashAttackVerB);
				break;
		}
	}
}