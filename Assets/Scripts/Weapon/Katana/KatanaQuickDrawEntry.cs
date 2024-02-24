using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class KatanaQuickDrawEntry : StateBase<Katana.State, Katana>
{
	PlayerAttack playerAttack;
	PlayerAnimEvent playerAnimEvent;

	public KatanaQuickDrawEntry(Katana owner, StateMachine<Katana.State, Katana> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		playerAttack.SetAnimFloat("IdleAdapter", 0f);
		playerAttack.SetAnimTrigger("Hold1");
		playerAnimEvent.OnEquipChange.AddListener(EquipChange);
	}

	public override void Exit()
	{
		playerAnimEvent.OnEquipChange.RemoveListener(EquipChange);
	}

	public override void Setup()
	{
		playerAttack = owner.PlayerAttack;
		playerAnimEvent = owner.PlayerAnimEvent;
	}

	public override void Transition()
	{

	}

	public override void Update()
	{

	}

	private void EquipChange()
	{
		owner.SetDummyRender(true);
		stateMachine.ChangeState(Katana.State.QuickDrawIdle);
	}
}