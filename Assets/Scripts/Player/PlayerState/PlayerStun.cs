using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerStun : StateBase<Player.State, Player>
{
	PlayerAttack playerAttack;
	PlayerMove playerMove;

	bool waitAnim;

	public PlayerStun(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		owner.SetAnimRootMotion(true);
		waitAnim = false;
		playerAttack.SetAnimTrigger("StunStart");
		playerMove.MoveMultiplier = 0f;
		FieldSFC.Instance?.PlayStun();
		
	}

	public override void Exit()
	{
		owner.SetAnimRootMotion(false);
		playerMove.MoveMultiplier = 1f;
	}

	public override void Setup()
	{
		playerMove = owner.PlayerMove;
		playerAttack = owner.PlayerAttack;
	}

	public override void Transition()
	{
		if(waitAnim == true) { return; }

		if(Time.time > owner.StunEndTime)
		{
			waitAnim = true;
			_ = owner.StartCoroutine(CoStunEnd());
		}
	}

	private IEnumerator CoStunEnd()
	{
		playerAttack.SetAnimTrigger("StunEnd");
		yield return new WaitUntil(() => playerAttack.IsAnimName(0, "StunEnd") == true);
		yield return new WaitUntil(() => playerAttack.IsAnimName(0, "StunEnd") == false);
		stateMachine.ChangeState(Player.State.Idle);
	}

	public override void Update()
	{

	}
}