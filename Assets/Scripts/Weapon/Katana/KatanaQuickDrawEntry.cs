using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class KatanaQuickDrawEntry : StateBase<Katana.State, Katana>
{
	Player player;
	PlayerAttack playerAttack;
	PlayerAnimEvent playerAnimEvent;

	public KatanaQuickDrawEntry(Katana owner, StateMachine<Katana.State, Katana> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		owner.QuickDrawCnt = 0;
		playerAnimEvent.OnEquipChange.AddListener(EquipChange);
		player.ChangeState(Player.State.StandAttack);
		if (GameManager.Stat.TrySubCurMP(40) == false)
		{
			playerAttack.SetAnimTrigger("BaseExit");
			stateMachine.ChangeState(Katana.State.Idle);
			return;
		}
		playerAttack.SetAnimFloat("IdleAdapter", 0f);
		playerAttack.SetAnimTrigger("Hold1");
	}

	public override void Exit()
	{
		playerAnimEvent.OnEquipChange.RemoveListener(EquipChange);
	}

	public override void Setup()
	{
		player = owner.Player;
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