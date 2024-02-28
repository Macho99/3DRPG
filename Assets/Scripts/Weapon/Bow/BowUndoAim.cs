using System.Collections.Generic;
using UnityEngine;

public class BowUndoAim : StateBase<Bow.State, Bow>
{
	Player player;
	PlayerAttack playerAttack;

	float rigWeight;

	public BowUndoAim(Bow owner, StateMachine<Bow.State, Bow> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		rigWeight = player.GetBowAimRigWeight();
		playerAttack.SetAnimFloat("Reverse", -2f);
	}

	public override void Exit()
	{
		player.SetBowAimRigWeight(0f);
		playerAttack.SetAnimFloat("Reverse", 1f);
	}

	public override void Setup()
	{
		player = owner.Player;	
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
		rigWeight = Mathf.Lerp(rigWeight, 0f, Time.deltaTime * 4f);
		player.SetBowAimRigWeight(rigWeight);
	}
}
