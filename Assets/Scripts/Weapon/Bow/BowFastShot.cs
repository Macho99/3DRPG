using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BowFastShot : StateBase<Bow.State, Bow>
{
	PlayerAttack playerAttack;
	PlayerAnimEvent playerAnimEvent;

	public BowFastShot(Bow owner, StateMachine<Bow.State, Bow> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		owner.Shot();
		owner.FastShotNum++;
		owner.SetBowWeight(0f);
		playerAttack.SetAnimTrigger("Shot");
		playerAnimEvent.OnEquipChange.AddListener(ReturnToFastAim);
	}

	public override void Exit()
	{
		playerAnimEvent.OnEquipChange.RemoveListener(ReturnToFastAim);
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

	private void ReturnToFastAim()
	{
		stateMachine.ChangeState(Bow.State.FastAim);
	}
}
