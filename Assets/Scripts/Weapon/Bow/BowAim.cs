using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BowAim : StateBase<Bow.State, Bow>
{
	PlayerMove playerMove;
	PlayerAttack playerAttack;
	public BowAim(Bow owner, StateMachine<Bow.State, Bow> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		playerAttack.SetAnimTrigger("UpperHold1");
		playerAttack.OnAttack1Up.AddListener(UndoAim);
		//playerAttack.on
	}

	public override void Exit()
	{
		playerAttack.OnAttack1Up.RemoveListener(UndoAim);
	}

	public override void Setup()
	{
		playerMove = owner.PlayerMove;
		playerAttack = owner.PlayerAttack;
	}

	public override void Transition()
	{
		
	}

	public override void Update()
	{

	}

	private void UndoAim(Player.State state)
	{
		stateMachine.ChangeState(Bow.State.UndoAim);
	}
}