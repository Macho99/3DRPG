using System.Collections.Generic;
using UnityEngine;

public class BowShot : StateBase<Bow.State, Bow>
{
	Player player;
	PlayerLook playerLook;
	PlayerAttack playerAttack;
	PlayerAnimEvent playerAnimEvent;
	PlayerCamManager playerCamManager;
	public BowShot(Bow owner, StateMachine<Bow.State, Bow> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		playerAttack.SetAnimTrigger("Upper1");
		owner.Reloaded = false;
		playerAnimEvent.OnEquipChange.AddListener(ChangeToReload);
		player.SetBowAimRigWeight(0f, 0f, 0f);
		owner.SetArrowHold(Bow.ArrowHoldMode.None);
		owner.SetBowWeight(0f);

		owner.Shot();
	}

	public override void Exit()
	{
		playerAnimEvent.OnEquipChange.RemoveListener(ChangeToReload);
	}

	public override void Setup()
	{
		player = owner.Player;
		playerLook = player.GetComponent<PlayerLook>();
		playerAttack = owner.PlayerAttack;
		playerAnimEvent = owner.PlayerAnimEvent;
		playerCamManager = owner.PlayerCamManager;
	}

	public override void Transition()
	{
		if(playerAttack.IsAnimWait(1) == true)
		{
			playerAttack.SetAnimTrigger("UpperExit");
			stateMachine.ChangeState(Bow.State.Idle);
		}
	}

	public override void Update()
	{

	}

	private void ChangeToReload()
	{
		stateMachine.ChangeState(Bow.State.Reload);
	}
}
