using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class KatanaQuickDrawBase : KatanaSwingBase
{
	bool dummyActive;

	public KatanaQuickDrawBase(Katana owner, StateMachine<Katana.State, Katana> stateMachine, string triggerName) : base(owner, stateMachine, triggerName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		dummyActive = true;
		playerAnimEvent.OnEquipChange.AddListener(EquipChange);
	}

	public override void Exit()
	{
		base.Exit();
		playerAnimEvent.OnEquipChange.RemoveListener(EquipChange);
	}

	protected override bool CheckTransition()
	{
		stateMachine.ChangeState(Katana.State.QuickDrawIdle);
		return true;
	}

	private void EquipChange()
	{
		dummyActive = !dummyActive;
		owner.SetDummyRender(dummyActive);
	}
}