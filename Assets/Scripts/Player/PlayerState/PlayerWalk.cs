using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerWalk : StateBase<Player.State, Player>
{
	PlayerMove playerMove;
	public PlayerWalk(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
	{

	}

	public override void Enter()
	{
		playerMove.MoveMultiplier = 1f;
	}

	public override void Exit()
	{

	}

	public override void Setup()
	{
		playerMove = owner.PlayerMove;
	}

	public override void Transition()
	{
		if(playerMove.MoveInput.sqrMagnitude < 0.1f)
		{
			stateMachine.ChangeState(Player.State.Idle);
			return;
		}
		else if(playerMove.SprintInput == true)
		{
			stateMachine.ChangeState(Player.State.Run);
		}
	}

	public override void Update()
	{

	}
}
