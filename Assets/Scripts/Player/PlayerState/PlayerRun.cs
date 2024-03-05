using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerRun : StateBase<Player.State, Player>
{
	PlayerMove playerMove;
	PlayerAnimEvent playerAnimEvent;

	public PlayerRun(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
	{

	}

	public override void Enter()
	{
		playerMove.MoveMultiplier = 2f;
		playerMove.OnJumpDown.AddListener(owner.Jump);
		playerMove.OnFalling.AddListener(owner.OnAir);
	}

	public override void Exit()
	{
		playerMove.OnJumpDown.RemoveListener(owner.Jump);
		playerMove.OnFalling.RemoveListener(owner.OnAir);
	}

	public override void Setup()
	{
		playerMove = owner.PlayerMove;
		playerAnimEvent = owner.PlayerAnimEvent;
	}

	public override void Transition()
	{
		if (playerMove.MoveInput.sqrMagnitude < 0.1f)
		{
			stateMachine.ChangeState(Player.State.Idle);
			return;
		}
		else if (playerMove.SprintInput == false || playerMove.SprintLock == true)
		{
			stateMachine.ChangeState(Player.State.Walk);
		}
	}

	public override void Update()
	{

	}
}