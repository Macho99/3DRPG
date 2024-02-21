using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerRun : StateBase<Player.State, Player>
{
	PlayerMove playerMove;
	PlayerAttack playerAttack; 

	public PlayerRun(Player owner, StateMachine<Player.State, Player> stateMachine) : base(owner, stateMachine)
	{

	}

	public override void Enter()
	{
		playerMove.MoveMultiplier = 2f;
		//playerAttack.SetUnarmed();
	}

	public override void Exit()
	{

	}

	public override void Setup()
	{
		playerAttack = owner.PlayerAttack;
		playerMove = owner.PlayerMove;
	}

	public override void Transition()
	{
		if (playerMove.MoveInput.sqrMagnitude < 0.1f)
		{
			stateMachine.ChangeState(Player.State.Idle);
			return;
		}
		else if (playerMove.SprintInput == false)
		{
			stateMachine.ChangeState(Player.State.Walk);
		}
	}

	public override void Update()
	{

	}
}