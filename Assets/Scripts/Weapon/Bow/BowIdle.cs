using System;
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
	public BowIdle(Bow owner, StateMachine<Bow.State, Bow> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		if(owner.Reloaded == false)
		{
			stateMachine.ChangeState(Bow.State.Reload);
			return;
		}

		playerAttack.OnAttack1Down.AddListener(Aim);
	}

	public override void Exit()
	{
	}

	public override void Setup()
	{
		player = owner.Player;
		playerMove = owner.PlayerMove;
		playerAttack = owner.PlayerAttack;
	}

	public override void Transition()
	{

	}

	public override void Update()
	{
		playerAttack.SetAnimFloat("IdleAdapter", 0f, 0.1f, Time.deltaTime);

		switch (player.CurState)
		{
			case Player.State.Idle:
			case Player.State.Walk:
			case Player.State.Run:
				owner.RenderArrowToHold();
				break;
			default:
				owner.RenderLeftHandArrow();
				break;
		}
	}

	private void Aim(Player.State state)
	{
		switch (state)
		{
			case Player.State.Idle:
			case Player.State.Walk:
				stateMachine.ChangeState(Bow.State.StartAim);
				break;
		}
	}
}