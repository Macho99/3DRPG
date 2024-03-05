using System.Collections.Generic;
using UnityEngine;

public class BowUndoAim : StateBase<Bow.State, Bow>
{
	Player player;
	PlayerAttack playerAttack;
	PlayerCamManager playerCamManager;

	float rigWeight;
	float arrowRigWeight;

	public BowUndoAim(Bow owner, StateMachine<Bow.State, Bow> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		rigWeight = player.GetBowAimRigWeight();
		arrowRigWeight = rigWeight;
		playerAttack.SetAnimFloat("Reverse", -2f);
	}

	public override void Exit()
	{
		player.SetBowAimRigWeight(0f, 0f, 0f);
		playerAttack.SetAnimFloat("Reverse", 1f);
		owner.SetBowWeight(0f);
	}

	public override void Setup()
	{
		player = owner.Player;	
		playerAttack = owner.PlayerAttack;
		playerCamManager = owner.PlayerCamManager;
	}

	public override void Transition()
	{
		if(playerAttack.IsAnimName(1, "UpperHold1") == true)
		{
			if (playerAttack.GetAnimNormalizedTime(1) < 0.01f)
			{
				playerAttack.SetAnimTrigger("UpperExit");
				stateMachine.ChangeState(Bow.State.Idle);
			}
		}
	}

	public override void Update()
	{
		rigWeight = Mathf.Lerp(rigWeight, 0f, Time.deltaTime * 4f);
		if(rigWeight < 0.7f)
		{
			arrowRigWeight = Mathf.Lerp(arrowRigWeight, 0f, Time.deltaTime * 10f);
		}
		owner.SetBowWeight(arrowRigWeight);
		player.SetBowAimRigWeight(rigWeight, rigWeight, arrowRigWeight);
	}
}
