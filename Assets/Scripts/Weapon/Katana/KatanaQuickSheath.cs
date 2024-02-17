using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class KatanaQuickSheath : StateBase<Katana.State, Katana>
{
	PlayerAttack playerAttack;
	PlayerAnimEvent playerAnimEvent;
	public KatanaQuickSheath(Katana owner, StateMachine<Katana.State, Katana> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		playerAttack.SetAnimTrigger("Upper1");
		playerAnimEvent.OnEquipChange.AddListener(UnEquip);
	}

	public override void Exit()
	{
		playerAnimEvent.OnEquipChange.RemoveListener(UnEquip);
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
			stateMachine.ChangeState(Katana.State.Unarmed);
		}
	}

	public override void Update()
	{
		playerAttack.SetAnimFloat("Armed", 0f, 0.1f, Time.deltaTime);
	}

	private void UnEquip()
	{
		owner.SetDummyRender(true);
	}
}
