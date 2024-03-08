using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class KatanaInactive : StateBase<Katana.State, Katana>
{
	public KatanaInactive(Katana owner, StateMachine<Katana.State, Katana> stateMachine) : base(owner, stateMachine)
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
		stateMachine.ChangeState(Katana.State.Equip);
	}

	public override void Update()
	{

	}
}