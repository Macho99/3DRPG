using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class KatanaEquip : StateBase<Katana.State, Katana>
{
	PlayerAttack playerAttack;
	PlayerAnimEvent playerAnimEvent;
	public KatanaEquip(Katana owner, StateMachine<Katana.State, Katana> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		owner.SetDummyRender(true);
		playerAttack.SetAnimTrigger("Upper2");
		playerAnimEvent.OnEquipChange.AddListener(Equip);
	}

	public override void Exit()
	{
		playerAnimEvent.OnEquipChange.RemoveListener(Equip);
	}

	public override void Setup()
	{
		playerAttack = owner.PlayerAttack;
		playerAnimEvent = owner.PlayerAnimEvent;
	}

	public override void Transition()
	{
		if(playerAttack.IsAnimWait(1) == true)
		{
			playerAttack.SetAnimTrigger("UpperExit");
			stateMachine.ChangeState(Katana.State.Idle);
			return;
		}
	}

	public override void Update()
	{
		playerAttack.SetAnimFloat("Armed", 1f, 0.1f, Time.deltaTime);
	}

	private void Equip()
	{
		owner.SetDummyRender(false);
	}
}