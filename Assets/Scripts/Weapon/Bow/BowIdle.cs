﻿using System;
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
		playerAttack.OnEButtonDown.AddListener(ESkill);
	}

	public override void Exit()
	{
		playerAttack.OnAttack1Down.RemoveListener(Aim);
		playerAttack.OnQButtonDown.RemoveListener(FastAim);
		playerAttack.OnEButtonDown.RemoveListener(ESkill);
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
		RigWeightSetting();

		if (owner.Reloaded == true)
		{
			switch (player.CurState)
			{
				case Player.State.Idle:
				case Player.State.Walk:
				case Player.State.Run:
				case Player.State.MoveAttack:
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
				case Player.State.MoveAttack:
					break;
				default:
					playerMove.AimLock = false;
					playerCamManager.SetAimCam(false);
					aimLock = false;
					break;
			}
		}
	}

	private void RigWeightSetting()
	{
		float weight = Mathf.Lerp(player.GetBowAimRigWeight(), 0f, Time.deltaTime * 3f);
		player.SetBowAimRigWeight(weight, weight, weight);
	}

	private bool CheckAvailableState(Player.State state)
	{
		if (playerAttack.IsAnimName(1, "Entry") == false) return false;

		switch (state)
		{
			case Player.State.Idle:
			case Player.State.Walk:
				return true;
		}

		return false;
	}

	private void Aim(Player.State state)
	{
		if (CheckAvailableState(state) == false) return;

		aimLock = true;
		stateMachine.ChangeState(Bow.State.StartAim);
	}

	private void FastAim(Player.State state)
	{
		if (CheckAvailableState(state) == false) return;

		aimLock = true;
		stateMachine.ChangeState(Bow.State.FastAim);
	}

	private void ESkill(Player.State state)
	{
		if (CheckAvailableState(state) == false) return;

		switch (owner.CurArrowProperty) {
			case Bow.ArrowProperty.Ice:
				owner.IceSkill();
				break;
			case Bow.ArrowProperty.Fire:
				aimLock = true;
				stateMachine.ChangeState(Bow.State.FireRain);
				break;
			case Bow.ArrowProperty.Wind:
				owner.WindSkill();
				break;
		}
	}
}