using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Bow : Weapon
{
	[SerializeField] GameObject arrowToDraw;
	[SerializeField] GameObject arrowToShoot;
	[SerializeField] GameObject leftHandArrow;

	[Serializable]
	public enum State { Idle, Reload, StartAim, Aiming, UndoAim, Shot };

	[SerializeField] State curState;

	StateMachine<State, Bow> stateMachine;

	public bool Reloaded { get; set; }

	protected override void Awake()
	{
		base.Awake();
		stateMachine = new StateMachine<State, Bow>(this);
		stateMachine.AddState(State.Idle, new BowIdle(this, stateMachine));
		stateMachine.AddState(State.Reload, new BowReload(this, stateMachine));
		stateMachine.AddState(State.StartAim, new BowStartAim(this, stateMachine));
		stateMachine.AddState(State.Aiming, new BowAiming(this, stateMachine));
		stateMachine.AddState(State.UndoAim, new BowUndoAim(this, stateMachine));
		stateMachine.AddState(State.Shot, new BowShot(this, stateMachine));
	}

	protected override void Start()
	{
		base.Start();
		stateMachine.SetUp(State.Idle);
	}

	private void Update()
	{
		stateMachine.Update();
		curState = stateMachine.GetCurState();
	}

	public override void ChangeStateToIdle(bool forceIdle = false)
	{

	}

	public override void SetUnArmed()
	{

	}

	private void OnEnable()
	{
		player.SetNeckRigWeight(0.6f);
		playerMove.AimLockOffset = new Vector3(0f, 45f, 0f);
	}

	private void OnDisable()
	{
		player.SetNeckRigWeight(0f);
		playerMove.AimLockOffset = Vector3.zero;
	}

	public void SetOffArrow()
	{
		arrowToDraw.SetActive(false);
		arrowToShoot.SetActive(false);
		leftHandArrow.SetActive(false);
	}

	public void RenderArrowToDraw()
	{
		arrowToDraw.SetActive(true);
		arrowToShoot.SetActive(false);
		leftHandArrow.SetActive(false);
	}

	public void RenderArrowToHold()
	{
		arrowToDraw.SetActive(false);
		arrowToShoot.SetActive(true);
		leftHandArrow.SetActive(false);
	}

	public void RenderLeftHandArrow()
	{
		arrowToDraw.SetActive(false);
		arrowToShoot.SetActive(false);
		leftHandArrow.SetActive(true);
	}
}
