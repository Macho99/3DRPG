using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerBreakFall : StateBase<Player.State, Player>
{
	PlayerMove playerMove;
	PlayerAnimEvent playerAnimEvent;
	public PlayerBreakFall(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		owner.SetAnimRootMotion(true);
		playerMove.SetAnimTrigger("BreakFall");
		playerAnimEvent.OnLandStart.AddListener(LandStart);
		playerAnimEvent.OnLandEnd.AddListener(LandEnd);
	}

	public override void Exit()
	{
		playerMove.SetAnimFloat("IdleAdapter", 1f);
		playerMove.MoveMultiplier = 1f;
		playerAnimEvent.OnLandStart.RemoveListener(LandStart);
		playerAnimEvent.OnLandEnd.RemoveListener(LandEnd);
	}

	public override void Setup()
	{
		playerMove = owner.PlayerMove;
		playerAnimEvent = owner.PlayerAnimEvent;
	}

	public override void Transition()
	{
	}

	public override void Update()
	{

	}
	private void LandStart()
	{
		playerMove.MoveMultiplier = 0f;
	}

	private void LandEnd()
	{
		owner.SetAnimRootMotion(false);
		stateMachine.ChangeState(Player.State.Idle);
	}
}