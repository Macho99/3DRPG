using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerDie : StateBase<Player.State, Player>
{
	PlayerAttack playerAttack;
	PlayerMove playerMove;

	public PlayerDie(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		owner.SetAnimRootMotion(true);
		owner.IgnoreInput(true);
		playerAttack.SetAnimBool("Die", true);
		playerMove.MoveMultiplier = 0f;
	}

	public override void Exit()
	{
		owner.SetAnimRootMotion(false);
		owner.IgnoreInput(false);
		playerAttack.SetAnimBool("Die", false);
		playerMove.MoveMultiplier = 1f;
	}

	public override void Setup()
	{
		playerAttack = owner.PlayerAttack;
		playerMove = owner.PlayerMove;
	}

	public override void Transition()
	{

	}

	public override void Update()
	{

	}
}