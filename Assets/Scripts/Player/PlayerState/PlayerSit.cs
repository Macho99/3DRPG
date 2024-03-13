using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSit : StateBase<Player.State, Player>
{
	PlayerAttack playerAttack;
	PlayerMove playerMove;
	PlayerLook playerLook;
	PlayerCamManager playerCamManager;

	public PlayerSit(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		FieldSFC.Instance.PlayFadeOut();
		GameManager.UI.HideSceneUI(true);
		playerCamManager.SetTitleCam(true);
		playerLook.Follow = false;
		playerAttack.SetAnimBool("Sit", true);
		playerMove.MoveMultiplier = 0f;
	}

	public override void Exit()
	{
		playerLook.Follow = true;
		GameManager.UI.HideSceneUI(false);
		playerCamManager.SetTitleCam(false);
		playerAttack.SetAnimBool("Sit", false);
		playerMove.MoveMultiplier = 1f;
	}

	public override void Setup()
	{
		playerLook = owner.PlayerLook;
		playerAttack = owner.PlayerAttack;
		playerMove = owner.PlayerMove;
		playerCamManager = owner.GetComponent<PlayerCamManager>();
	}

	public override void Transition()
	{
		if(playerMove.MoveInput.sqrMagnitude > 0.1f)
		{
			owner.PlayerAttack.SetAnimBool("Sit", false);
			_ = owner.StartCoroutine(CoExit());
		}
	}

	private IEnumerator CoExit()
	{
		yield return new WaitUntil(() => playerAttack.IsAnimName(0, "SitEnd") == true);
		yield return new WaitUntil(() => playerAttack.IsAnimName(0, "SitEnd") == false);
		stateMachine.ChangeState(Player.State.Idle);
	}

	public override void Update()
	{

	}
}