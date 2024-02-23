using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerJump : StateBase<Player.State, Player>
{
	PlayerMove playerMove;
	PlayerAnimEvent PlayerAnimEvent;
	public PlayerJump(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		playerMove.SetAnimFloat("Jump", 1f);
		playerMove.GravityMultiplier = 0f;
		owner.SetCamFollowSpeed(5f);
		owner.CurJumpState = Player.JumpState.Jump;
		PlayerAnimEvent.OnJumpStart.AddListener(Jump);
	}

	public override void Exit()
	{

	}

	public override void Setup()
	{
		playerMove = owner.PlayerMove;
		PlayerAnimEvent = owner.PlayerAnimEvent;
	}

	public override void Transition()
	{
		if (playerMove.IsAnimWait(0))
		{
			playerMove.GravityMultiplier = 1f;
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