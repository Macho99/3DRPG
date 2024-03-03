using System.Collections.Generic;
using UnityEngine;

public class BowStartAim : StateBase<Bow.State, Bow>
{
	Player player;
	PlayerMove playerMove;
	PlayerAttack playerAttack;
	PlayerCamManager playerCamManager;

	float rigWeight;
	float arrowRigWeight;

	public BowStartAim(Bow owner, StateMachine<Bow.State, Bow> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		owner.PlayAimVFX(true);
		owner.WindControllerPrepareAttack();
		rigWeight = 0f;
		arrowRigWeight = 0f;
		playerCamManager.SetAimCam(true);
		player.ChangeState(Player.State.MoveAttack);
		playerAttack.SetAnimTrigger("UpperHold1");
		playerAttack.OnAttack1Up.AddListener(UndoAim);
		playerAttack.OnAttack2Down.AddListener(UndoAim);
		playerMove.AimLock = true;
	}

	public override void Exit()
	{
		owner.PlayAimVFX(false);
		player.SetBowAimRigWeight(1f, 1f, 1f);
		playerAttack.OnAttack1Up.RemoveListener(UndoAim);
		playerAttack.OnAttack2Down.RemoveListener(UndoAim);
		owner.SetBowWeight(1f);
	}

	public override void Setup()
	{
		player = owner.Player;
		playerMove = owner.PlayerMove;
		playerAttack = owner.PlayerAttack;
		playerCamManager = owner.PlayerCamManager;
	}

	public override void Transition()
	{
		if(playerAttack.IsAnimName(1, "UpperHold1") == true)
		{
			if (playerAttack.GetAnimNormalizedTime(1) > 0.99f)
			{
				stateMachine.ChangeState(Bow.State.Aiming);
			}
		}
	}

	public override void Update()
	{
		rigWeight = Mathf.Lerp(rigWeight, 1f, Time.deltaTime * 4f);
		if (playerAttack.IsAnimName(1, "UpperHold1") == true)
		{
			if (playerAttack.GetAnimNormalizedTime(1) > owner.arrowRigTime)
			{
				arrowRigWeight = Mathf.Lerp(arrowRigWeight, 1f, Time.deltaTime * owner.arrowRigLerpSpeed);
			}
		}
		owner.SetBowWeight(arrowRigWeight);
		player.SetBowAimRigWeight(rigWeight, rigWeight, arrowRigWeight);
	}

	private void UndoAim(Player.State state)
	{
		stateMachine.ChangeState(Bow.State.UndoAim);
	}
}