using System;
using System.Collections;
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
	bool waitAnim;

	public BowFastAim(Bow owner, StateMachine<Bow.State, Bow> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		waitAnim = false;
		shotReady = false;
		owner.WindControllerPrepareAttack();
		playerAttack.OnAttack1Down.AddListener(FastShot);
		playerAnimEvent.OnEquipChange.AddListener(ShotReady);

		if (owner.FastShotNum == 0)
		{
			owner.PlayFastShotVFX();
			owner.SetFastShotArrowMat(true);
			playerAttack.SetAnimTrigger("UpperHold2");
			playerCamManager.SetAimCam(true);
			player.ChangeState(Player.State.MoveAttack);
			playerMove.AimLock = true;
			player.SetBowAimRigWeight(1f, 1f, 1f);
		}
		else if(owner.FastShotNum > 4)
		{
			_ = owner.StartCoroutine(CoExit());
			return;
		}
	}

	private IEnumerator CoExit()
	{
		waitAnim = true;
		owner.SetFastShotArrowMat(false);
		yield return new WaitUntil(() => playerAttack.IsAnimName(1, "UpperHold2") == true);
		playerAttack.SetAnimTrigger("UpperExit");
		owner.FastShotNum = 0;
		stateMachine.ChangeState(Bow.State.Idle);
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
		if (waitAnim == true) return;

		if (playerAttack.Attack1Pressed == true)
		{
			FastShot(player.CurState);
		}
	}

	public override void Update()
	{
		if (waitAnim == true) return;

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
		if (waitAnim == true)return;

		if(shotReady == true)
		{
			stateMachine.ChangeState(Bow.State.FastShot);
		}
	}
}