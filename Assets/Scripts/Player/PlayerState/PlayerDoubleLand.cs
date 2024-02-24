using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerDoubleLand : StateBase<Player.State, Player>
{
	PlayerMove playerMove;
	PlayerAnimEvent playerAnimEvent;

	bool landEnd;

	public PlayerDoubleLand(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		owner.SetCamFollowSpeed(50f, 1f);
		landEnd = false;
		playerMove.SetAnimTrigger("Land");
		playerMove.SetAnimTrigger("BaseExit");
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
		if (landEnd == false) return;
		if(playerMove.IsAnimName(0, "Double_Jump_End") == true)
		{
			stateMachine.ChangeState(Player.State.Idle);
		}
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
		landEnd = true;
	}
}