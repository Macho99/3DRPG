using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BowShot : StateBase<Bow.State, Bow>
{
	PlayerAttack playerAttack;
	public BowShot(Bow owner, StateMachine<Bow.State, Bow> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		playerAttack.SetAnimTrigger("Upper1");
	}

	public override void Exit()
	{

	}

	public override void Setup()
	{
		playerAttack = owner.PlayerAttack;
	}

	public override void Transition()
	{
		if(playerAttack.IsAnimWait(1) == true)
		{
			playerAttack.SetAnimTrigger("UpperExit");
			stateMachine.ChangeState(Bow.State.Reload);
		}
	}

	public override void Update()
	{

	}
}
