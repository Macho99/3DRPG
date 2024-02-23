using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerDoubleLand : StateBase<Player.State, Player>
{
	PlayerMove playerMove;
	public PlayerDoubleLand(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		playerMove.SetAnimTrigger("Land");
		playerMove.SetAnimTrigger("BaseExit");
		playerMove.MoveMultiplier = 0f;
	}

	public override void Exit()
	{
		playerMove.SetAnimFloat("Grruzam", 1f);
		playerMove.MoveMultiplier = 1f;
	}

	public override void Setup()
	{
		playerMove = owner.PlayerMove;
	}

	public override void Transition()
	{
		if(playerMove.IsAnimName(0, "Double_Jump_End") == true)
		{
			stateMachine.ChangeState(Player.State.Idle);
		}
	}

	public override void Update()
	{

	}
}