using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BowReload : StateBase<Bow.State, Bow>
{
	PlayerAttack playerAttack;
	PlayerMove playerMove;
	PlayerAnimEvent playerAnimEvent;

	bool arrowDraw;

	public BowReload(Bow owner, StateMachine<Bow.State, Bow> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		arrowDraw = false;
		playerMove.SprintLock = true;
		playerAttack.SetAnimTrigger("Upper4");
		playerAnimEvent.OnEquipChange.AddListener(EquipChange);
	}

	public override void Exit()
	{
		playerMove.SprintLock = false;
		playerAnimEvent.OnEquipChange.RemoveListener(EquipChange);
	}

	public override void Setup()
	{
		playerMove = owner.PlayerMove;
		playerAttack = owner.PlayerAttack;
		playerAnimEvent = owner.PlayerAnimEvent;
	}

	public override void Transition()
	{
		if(playerAttack.IsAnimWait(1) == true)
		{
			owner.Reloaded = true;
			playerAttack.SetAnimTrigger("UpperExit");
			stateMachine.ChangeState(Bow.State.Idle);
		}
	}

	public override void Update()
	{

	}

	private void EquipChange()
	{
		if(arrowDraw == false)
		{
			owner.RenderArrowToDraw();
			arrowDraw = true;
		}
		else
		{
			owner.RenderArrowToHold();
		}
	}
}