using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
	[SerializeField] State curState;
	[SerializeField] Rig neckRig;
	[SerializeField] Rig spineRig;
	[SerializeField] Rig leftShoulderRig;
	[SerializeField] Rig rightHandRig;

	[Serializable]
	public enum State { Idle, Walk, Run, Dodge, Jump, OnAir, Land,
		JumpTest,
		DoubleJump, DoubleOnAir, DoubleLand, BreakFall,
		StandAttack, OnAirAttack, MoveAttack,
		Stun, Die};

	private Animator anim;
	private MMFollowTarget camRootFollower;
	private PlayerLook playerLook;
	private PlayerMove playerMove;
	private PlayerAttack playerAttack;
	private PlayerAnimEvent playerAnimEvent;
	private StateMachine<State, Player> stateMachine;
	private bool camFollowFixed;

	[HideInInspector] public UnityEvent OnWeaponIdle;
	[HideInInspector] public UnityEvent OnDodgeAttackStart;

	public bool DoubleJumped { get; set; }
	public Transform MoveRoot { get => playerMove.MoveRoot; }
	public State CurState { get { return curState; } }
	public PlayerLook PlayerLook { get {  return playerLook; } }
	public PlayerMove PlayerMove { get {  return playerMove; } }
	public PlayerAttack PlayerAttack { get {  return playerAttack; } }
	public PlayerAnimEvent PlayerAnimEvent { get { return playerAnimEvent; } }

	private void Awake()
	{
		anim = GetComponent<Animator>();
		playerLook = GetComponent<PlayerLook>();
		playerMove = GetComponent<PlayerMove>();
		playerAttack = GetComponent<PlayerAttack>();
		playerAnimEvent = GetComponent<PlayerAnimEvent>();

		camRootFollower = playerLook.CamRoot.GetComponent<MMFollowTarget>();
		OnWeaponIdle = new UnityEvent();
		OnDodgeAttackStart = new UnityEvent();

		stateMachine = new StateMachine<State, Player>(this);
		stateMachine.AddState(State.Idle, new PlayerIdle(this, stateMachine));
		stateMachine.AddState(State.Walk, new PlayerWalk(this, stateMachine));
		stateMachine.AddState(State.Run, new PlayerRun(this, stateMachine));
		stateMachine.AddState(State.Dodge, new PlayerDodge(this, stateMachine));
		stateMachine.AddState(State.Jump, new PlayerJump(this, stateMachine));
		stateMachine.AddState(State.OnAir, new PlayerOnAir(this, stateMachine));
		stateMachine.AddState(State.Land, new PlayerLand(this, stateMachine));
		stateMachine.AddState(State.DoubleJump, new PlayerDoubleJump(this, stateMachine));
		stateMachine.AddState(State.DoubleOnAir, new PlayerDoubleOnAir(this, stateMachine));
		stateMachine.AddState(State.DoubleLand, new PlayerDoubleLand(this, stateMachine));
		stateMachine.AddState(State.BreakFall, new PlayerBreakFall(this, stateMachine));

		stateMachine.AddState(State.StandAttack, new PlayerAttackStand(this, stateMachine));
		stateMachine.AddState(State.MoveAttack, new PlayerAttackMove(this, stateMachine));
		stateMachine.AddState(State.OnAirAttack, new PlayerAttackOnAir(this, stateMachine));

		stateMachine.AddState(State.JumpTest, new PlayerJumpTest(this, stateMachine));
	}

	private void Start()
	{
		stateMachine.SetUp(State.Idle);
	}

	private void Update()
	{
		stateMachine.Update();
		curState = stateMachine.GetCurState();
	}

	public void ChangeState(State newState)
	{
		stateMachine.ChangeState(newState);
	}

	public void WeaponIdle()
	{
		OnWeaponIdle?.Invoke();
	}

	public void SetAnimRootMotion(bool value, bool camSetting = true)
	{
		anim.applyRootMotion = value;
		if (camSetting == false) return;

		if(value == true)
		{
			SetCamFollowSpeed(5f);
		}
		else
		{
			SetCamFollowSpeed(50f, 1f);
		}
	}

	public void SetCamFollowSpeed(float speed)
	{
		camFollowFixed = true;
		camRootFollower.FollowPositionSpeed = 5f;
	}
	
	public void SetCamFollowSpeed(float speed, float lerpSpeed)
	{
		camFollowFixed = false;
		_ = StartCoroutine(CoFollowSpeedSet(speed, lerpSpeed));
	}

	private IEnumerator CoFollowSpeedSet(float target, float lerpSpeed)
	{
		while (camFollowFixed == false)
		{
			camRootFollower.FollowPositionSpeed = 
				Mathf.Lerp(camRootFollower.FollowPositionSpeed, target, lerpSpeed * Time.deltaTime);

			if(camRootFollower.FollowPositionSpeed > target - 10f)
			{
				camRootFollower.FollowPositionSpeed = target;
				break;
			}
			yield return null;
		}
	}

	public void Dodge()
	{
		playerAttack.ChangeStateToIdle(true);
		stateMachine.ChangeState(Player.State.Dodge);
	}

	public void Jump()
	{
		stateMachine.ChangeState(Player.State.Jump);
	}

	public void OnAir()
	{
		playerMove.SetAnimTrigger("Fall");
		stateMachine.ChangeState(Player.State.DoubleOnAir);
	}

	public void DodgeAttackStart()
	{
		OnDodgeAttackStart?.Invoke();
	}

	public void SetNeckRigWeight(float weight)
	{
		//if(neckRigCoroutine != null)
		//	StopCoroutine(neckRigCoroutine);

		//neckRigCoroutine = StartCoroutine(CoSetNeckRigWeight(weight, lerpSpeed));
		neckRig.weight = weight;
	}

	//public IEnumerator CoSetNeckRigWeight(float weight, float lerpSpeed)
	//{
	//	while (true)
	//	{
	//		neckRig.weight = Mathf.Lerp(neckRig.weight, weight, Time.deltaTime * lerpSpeed);
	//		if(Mathf.Abs(weight - neckRig.weight) < 0.1f)
	//		{
	//			neckRig.weight = weight;
	//			break;
	//		}
	//		yield return null;
	//	}
	//}

	public float GetBowAimRigWeight()
	{
		return spineRig.weight;
	}

	public void SetBowAimRigWeight(float weight)
	{
		spineRig.weight = weight;
		leftShoulderRig.weight = weight;
		rightHandRig.weight = weight;
	}
}