using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class KatanaAttackFail : StateBase<Katana.State, Katana>
{
	PlayerAttack playerAttack;
	public KatanaAttackFail(Katana owner, StateMachine<Katana.State, Katana> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		playerAttack.SetAnimFloat("Reverse", -0.4f);
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
		if(playerAttack.GetAnimNormalizedTime(0) < 0.05f)
		{
			playerAttack.SetAnimTrigger("BaseExit");
			stateMachine.ChangeState(Katana.State.Idle);
		}
	}

	public override void Update()
	{

	}
}