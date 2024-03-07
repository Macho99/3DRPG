using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	[Serializable]
	public enum FollowTransform { 
		KatanaHolder, SwordCase, SwordDummy, 
		LeftHandWeaponSlot, Spine3, RightHand, 
		Size
	}

	[Serializable]
	struct SkinMapping
	{
		public ArmorType armorType;
		public GameObject gameObject;
		[HideInInspector] public SkinnedMeshRenderer initialSkin;
	}

	[Serializable]
	struct TransformMapping
	{
		public FollowTransform followTransform;
		public Transform transform;
	}

	[Header("Enum 순서에 맞게 할당하세요!")]
	[SerializeField] SkinMapping[] skins;
	[Header("Enum 순서에 맞게 할당하세요!")]
	[SerializeField] TransformMapping[] followTransforms;
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
	private PlayerInput playerInput;
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
	public float StunEndTime { get; private set; }

	[SerializeField] private int curHP;
	[SerializeField] private int maxHp = 100;
	public int CurHp { get { return curHP; } set { curHP = value; } }
	public int MaxHp { get { return maxHp; } set { maxHp = value; } }

	[SerializeField] private float stunDuration;
	[SerializeField] private Vector3 stunDir;
	[SerializeField] private int damage;

	private void Awake()
	{
		curHP = maxHp;
		anim = GetComponent<Animator>();
		playerLook = GetComponent<PlayerLook>();
		playerMove = GetComponent<PlayerMove>();
		playerAttack = GetComponent<PlayerAttack>();
		playerAnimEvent = GetComponent<PlayerAnimEvent>();
		playerInput = GetComponent<PlayerInput>();

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

		stateMachine.AddState(State.Stun, new PlayerStun(this, stateMachine));
		stateMachine.AddState(State.Die, new PlayerDie(this, stateMachine));

		BindSkin();
	}

	private void BindSkin()
	{
		for(int i=0;i<skins.Length;i++)
		{
			SkinMapping skinMapping = skins[i];
			skinMapping.initialSkin = skinMapping.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
		}
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

	public void SetBowAimRigWeight(float spine, float leftShoulder, float rightHand)
	{
		//print(spine);
		spineRig.weight = spine;
		leftShoulderRig.weight = leftShoulder;
		rightHandRig.weight = rightHand;
	}

	public void IgnoreInput(bool value)
	{
		playerInput.enabled = !value;
		playerLook.enabled = !value;
	}

	private void OnTestButton(InputValue value)
	{
		bool pressed = value.Get<float>() > 0.9f;
		if (pressed == true)
		{
			TakeDamage(damage, true, stunDuration, stunDir);
		}
	}

	public void TakeDamage(int damage, bool hitFeedback = true)
	{
		TakeDamage(damage, hitFeedback, 0f, Vector3.zero);
	}

	public void TakeDamage(int damage, bool hitFeedback, float stunDuration, Vector3 knockback)
	{
		if (curState == State.Die) return;

		curHP -= damage;
		if(curHP <= 0)
		{
			curHP = 0;
			stateMachine.ChangeState(State.Die);
			return;
		}

		if(stunDuration > 0.01f) 
		{
			if(knockback.sqrMagnitude > 0.1f)
			{
				knockback.y = 0f;
				transform.forward = -knockback;
			}
			Stun(stunDuration);
		}
		else if(hitFeedback == true)
		{
			Hit();
		}
	}

	private void Stun(float duration)
	{
		playerAttack.ChangeStateToIdle();
		StunEndTime = Time.time + duration;
		stateMachine.ChangeState(Player.State.Stun);
	}

	private void Hit()
	{
		playerAttack.SetAnimTrigger("Hit");
		FieldSFC.Instance?.PlayHit();
	}

	public void SetArmor(ArmorItem armorItem)
	{
		ArmorType armorType = armorItem.ArmorType;
		skins[(int)armorType].gameObject.transform.
			Find(armorItem.ArmorSkinName).gameObject.SetActive(true);

		SkinnedMeshRenderer initalSkin = skins[(int)armorType].initialSkin;

		if(initalSkin != null)
			initalSkin.gameObject.SetActive(false);
	}

	public void InitArmor(ArmorType armorType)
	{
		string armorName = GameManager.Inven.GetArmorSlot(armorType).ArmorSkinName;
		skins[(int)armorType].gameObject.transform.
			Find(armorName).gameObject.SetActive(false);
		SkinnedMeshRenderer initalSkin = skins[(int)armorType].initialSkin;

		if (initalSkin != null)
			initalSkin.gameObject.SetActive(true);
	}

	public Transform GetTransform(FollowTransform followType)
	{
		return followTransforms[(int)followType].transform;
	}


	public void RefreshWeapon()
	{
		playerAttack.RefreshWeapon();
	}
}