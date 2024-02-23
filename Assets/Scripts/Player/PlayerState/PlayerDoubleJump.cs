using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerDoubleJump : StateBase<Player.State, Player>
{
	PlayerMove playerMove;
	public PlayerDoubleJump(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		playerMove.Jump(true);
		owner.SetCamFollowSpeed(5f);
		playerMove.Jump(true);
		playerMove.SetAnimTrigger("DoubleJump");
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
		if(playerMove.IsAnimName(0, "Double_Jump_Loop") == true)
		{
			stateMachine.ChangeState(Player.State.DoubleOnAir);
		}
	}

	public override void Update()
	{

	}
}