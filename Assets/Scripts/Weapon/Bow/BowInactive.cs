using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BowInactive : StateBase<Bow.State, Bow>
{
	public BowInactive(Bow owner, StateMachine<Bow.State, Bow> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{

	}

	public override void Exit()
	{

	}

	public override void Setup()
	{

	}

	public override void Transition()
	{
		stateMachine.ChangeState(Bow.State.Reload);
	}

	public override void Update()
	{

	}
}