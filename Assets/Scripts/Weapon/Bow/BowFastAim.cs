using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BowFastAim : StateBase<Bow.State, Bow>
{
	Player player;
	PlayerMove playerMove;
	PlayerAttack playerAttack;
	PlayerAnimEvent playerAnimEvent;
	PlayerCamManager playerCamManager;

	bool shotReady;

	public BowFastAim(Bow owner, StateMachine<Bow.State, Bow> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		shotReady = false;
		playerAttack.OnAttack1Down.AddListener(FastShot);
		playerAnimEvent.OnEquipChange.AddListener(ShotReady);

		if (owner.FastShotNum == 0)
		{
			playerAttack.SetAnimTrigger("UpperHold2");
			playerCamManager.SetAimCam(true);
			player.ChangeState(Player.State.MoveAttack);
			playerMove.AimLock = true;
			player.SetBowAimRigWeight(1f, 1f, 1f);
			return;
		}
		else if(owner.FastShotNum > 5) {
			playerAttack.SetAnimTrigger("UpperExit");
			player.SetBowAimRigWeight(0f, 0f, 0f);
			stateMachine.ChangeState(Bow.State.Idle);
			return;
		}
	}

	public override void Exit()
	{
		playerAttack.OnAttack1Down.RemoveListener(FastShot);
		playerAnimEvent.OnEquipChange.RemoveListener(ShotReady);
	}

	public override void Setup()
	{
		player = owner.Player;
		playerMove = owner.PlayerMove;
		playerAttack = owner.PlayerAttack;
		playerAnimEvent = owner.PlayerAnimEvent;
		playerCamManager = owner.PlayerCamManager;
	}

	public override void Transition()
	{

	}

	public override void Update()
	{
		if(playerAttack.IsAnimName(1, "UpperHold2") == true)
		{
			owner.SetBowWeight(Mathf.Clamp(playerAttack.GetAnimNormalizedTime(1), 0f, 1f));
		}
	}

	private void ShotReady()
	{
		shotReady = true;
	}

	private void FastShot(Player.State state)
	{
		if(shotReady == true)
		{
			stateMachine.ChangeState(Bow.State.FastShot);
		}
	}
}