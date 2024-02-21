using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerJump : StateBase<Player.State, Player>
{
	PlayerMove playerMove;
	public PlayerJump(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
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
		playerMove = owner.PlayerMove;
	}

	public override void Transition()
	{

	}

	public override void Update()
	{

	}
}