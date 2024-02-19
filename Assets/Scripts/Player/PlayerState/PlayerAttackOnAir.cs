using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerAttackOnAir : StateBase<Player.State, Player>
{
	PlayerMove playerMove;
	PlayerAttack playerAttack;
	public PlayerAttackOnAir(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		owner.SetAnimRootMotion(true);
		playerMove.MoveMultiplier = 0f;
		playerMove.GravityMultiplier = 0f;
		owner.OnWeaponIdle.AddListener(ChangeToIdle);
	}

	public override void Exit()
	{
		owner.SetAnimRootMotion(false);
		playerMove.MoveMultiplier = 1f;
		playerMove.GravityMultiplier = 1f;
		owner.OnWeaponIdle.RemoveListener(ChangeToIdle);
	}

	public override void Setup()
	{
		playerMove = owner.PlayerMove;
		playerAttack = owner.PlayerAttack;
	}

	public override void Transition()
	{

	}

	public override void Update()
	{

	}

	private void ChangeToIdle()
	{
		stateMachine.ChangeState(Player.State.Idle);
	}
}
