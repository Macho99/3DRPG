using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerIdle : StateBase<Player.State, Player>
{
	public PlayerIdle(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{

	}

	public override void Exit()
	{

	}

	public override void Setup()
	{

	}

	public override void Transition()
	{
		if(owner.PlayerMove.MoveInput.sqrMagnitude > 0.1f)
		{
			stateMachine.ChangeState(Player.State.Walk);
		}
	}

	public override void Update()
	{

	}
}
