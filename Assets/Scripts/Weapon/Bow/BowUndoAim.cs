using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BowUndoAim : StateBase<Bow.State, Bow>
{
	PlayerAttack playerAttack;
	public BowUndoAim(Bow owner, StateMachine<Bow.State, Bow> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		playerAttack.SetAnimFloat("Reverse", -1f);
	}

	public override void Exit()
	{
		playerAttack.SetAnimFloat("Reverse", 1f);
	}

	public override void Setup()
	{
		playerAttack = owner.PlayerAttack;
	}

	public override void Transition()
	{
		if(playerAttack.GetAnimNormalizedTime(1) < 0.01f)
		{
			playerAttack.SetAnimTrigger("UpperExit");
			stateMachine.ChangeState(Bow.State.Idle);
		}
	}

	public override void Update()
	{

	}
}
