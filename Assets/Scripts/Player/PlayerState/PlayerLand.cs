using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerLand : StateBase<Player.State, Player>
{
	PlayerMove playerMove;
	public PlayerLand(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		playerMove.SetAnimTrigger("Land");
		playerMove.SetAnimTrigger("BaseExit");
	}

	public override void Exit()
	{
		owner.SetCamFollowSpeed(50f, 1f);
		playerMove.SetAnimFloat("IdleAdapter", 1f);
		playerMove.MoveMultiplier = 1f;
	}

	public override void Setup()
	{
		playerMove = owner.PlayerMove;
	}

	public override void Transition()
	{
		if(playerMove.IsAnimName(0, "Jump_End"))
		{
			stateMachine.ChangeState(Player.State.Idle);
		}
	}

	public override void Update()
	{

	}
}
