using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BowIdle : StateBase<Bow.State, Bow>
{
	Player player;
	PlayerMove playerMove;
	PlayerAttack playerAttack;
	PlayerCamManager playerCamManager;

	bool aimLock = false;

	public BowIdle(Bow owner, StateMachine<Bow.State, Bow> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		player.WeaponIdle();
		playerAttack.OnAttack1Down.AddListener(Aim);
		playerAttack.OnQButtonDown.AddListener(FastAim);
	}

	public override void Exit()
	{
		playerAttack.OnAttack1Down.RemoveListener(Aim);
		playerAttack.OnQButtonDown.RemoveListener(FastAim);
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
		if (playerAttack.IsAnimName(1, "Entry") == false) return;

		if (owner.Reloaded == false)
		{
			stateMachine.ChangeState(Bow.State.Reload);
			return;
		}
		else if (playerAttack.Attack1Pressed == true)
		{
			Aim(player.CurState);
			return;
		}
	}

	public override void Update()
	{
		if(owner.Reloaded == true)
		{
			switch (player.CurState)
			{
				case Player.State.Idle:
				case Player.State.Walk:
				case Player.State.Run:
					owner.SetArrowHold(Bow.ArrowHoldMode.Hold);
					break;
				default:
					owner.SetArrowHold(Bow.ArrowHoldMode.LeftHand);
					break;
			}
		}

		if(aimLock == true)
		{
			switch (player.CurState)
			{
				case Player.State.Idle:
				case Player.State.Walk:
					break;
				default:
					playerMove.AimLock = false;
					playerCamManager.SetAimCam(false);
					aimLock = false;
					break;
			}
		}
	}

	private void Aim(Player.State state)
	{
		switch (state)
		{
			case Player.State.Idle:
			case Player.State.Walk:
				aimLock = true;
				stateMachine.ChangeState(Bow.State.StartAim);
				break;
		}
	}

	private void FastAim(Player.State state)
	{
		switch (state)
		{
			case Player.State.Idle:
			case Player.State.Walk:
				aimLock = true;
				stateMachine.ChangeState(Bow.State.FastAim);
				break;
		}
	}
}