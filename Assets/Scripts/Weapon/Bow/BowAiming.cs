using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BowAiming : StateBase<Bow.State, Bow>
{
	PlayerAttack playerAttack;
	PlayerMove playerMove;
	public BowAiming(Bow owner, StateMachine<Bow.State, Bow> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		playerAttack.SetAnimFloat("Reverse", 0f);
		playerAttack.OnAttack1Up.AddListener(Shot);
		playerAttack.OnAttack2Down.AddListener(UndoAim);
		playerMove.AimLock = true;
		//playerAttack.on
	}

	public override void Exit()
	{
		playerAttack.SetAnimFloat("Reverse", 1f);
		playerAttack.OnAttack1Up.RemoveListener(Shot);
		playerAttack.OnAttack2Down.RemoveListener(UndoAim);
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

	private void Shot(Player.State state)
	{
		stateMachine.ChangeState(Bow.State.Shot);
	}
}