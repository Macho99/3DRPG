using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerJump : StateBase<Player.State, Player>
{
	PlayerMove playerMove;
	PlayerAnimEvent playerAnimEvent;
	public PlayerJump(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		playerMove.SetAnimTrigger("Jump");
		owner.SetCamFollowSpeed(5f);
		playerAnimEvent.OnJumpStart.AddListener(Jump);
	}

	public override void Exit()
	{
		playerAnimEvent.OnJumpStart.RemoveListener(Jump);
	}

	public override void Setup()
	{
		playerMove = owner.PlayerMove;
		playerAnimEvent = owner.PlayerAnimEvent;
	}

	public override void Transition()
	{
		if (playerMove.IsAnimName(0, "Jump_Loop"))
		{
			stateMachine.ChangeState(Player.State.OnAir);
		}
	}

	public override void Update()
	{

	}

	private void Jump()
	{
		playerMove.Jump();
	}
}