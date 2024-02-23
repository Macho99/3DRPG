using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerIdle : StateBase<Player.State, Player>
{
	PlayerMove playerMove;

	public PlayerIdle(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		playerMove.OnDodgeDown.AddListener(owner.Dodge);
		playerMove.OnJumpDown.AddListener(owner.Jump);
		playerMove.OnFalling.AddListener(owner.OnAir);
	}

	public override void Exit()
	{
		playerMove.OnDodgeDown.RemoveListener(owner.Dodge);
		playerMove.OnJumpDown.RemoveListener(owner.Jump);
		playerMove.OnFalling.RemoveListener(owner.OnAir);
	}

	public override void Setup()
	{
		playerMove = owner.PlayerMove;
	}

	public override void Transition()
	{
		if(playerMove.MoveInput.sqrMagnitude > 0.1f)
		{
			stateMachine.ChangeState(Player.State.Walk);
		}
	}

	public override void Update()
	{

	}
}
