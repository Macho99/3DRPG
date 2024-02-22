using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.LookDev;

public class PlayerAttack : MonoBehaviour
{
	[SerializeField] Transform weaponHolder;
	[SerializeField] GameObject swordDummy;
	[SerializeField] float attackHoldTime = 0.3f;
	[SerializeField] MMF_Player attackFailFeedback;

	[HideInInspector] public UnityEvent<Player.State> OnAttack1Down;
	[HideInInspector] public UnityEvent<Player.State> OnAttack1Up;
	[HideInInspector] public UnityEvent<Player.State> OnAttack1Hold;
	[HideInInspector] public UnityEvent<Player.State> OnAttack2Down;
	[HideInInspector] public UnityEvent<Player.State> OnAttack2Up;
	[HideInInspector] public UnityEvent<Player.State> OnAttack2Hold;

	private Coroutine Attack1HoldCoroutine;
	private Coroutine Attack2HoldCoroutine;

	public bool Attack1Pressed { get; private set; }
	public bool Attack2Pressed { get; private set; }
	public GameObject SwordDummy { get => swordDummy; }
	public PlayerAnimEvent AnimEvent { get => animEvent; }

	private Player player;
	private PlayerAnimEvent animEvent;
	private Weapon[] weapons;
	private Weapon curWeapon;
	private Animator anim;

	private void Awake()
	{
		player = GetComponent<Player>();
		animEvent = GetComponent<PlayerAnimEvent>();
		anim = GetComponent<Animator>();
		weapons = weaponHolder.GetComponentsInChildren<Weapon>();

		OnAttack1Down = new UnityEvent<Player.State>();
		OnAttack1Up = new UnityEvent<Player.State>();
		OnAttack1Hold = new UnityEvent<Player.State>();
		OnAttack2Down = new UnityEvent<Player.State>();
		OnAttack2Up = new UnityEvent<Player.State>();
		OnAttack2Hold = new UnityEvent<Player.State>();

		if(weapons.Length > 0)
		{
			curWeapon = weapons[0];
		}
	}

	private void OnAttack1(InputValue value)
	{
		Attack1Pressed = value.Get<float>() > 0.9f ? true : false;

		if (Attack1Pressed)
		{
			OnAttack1Down?.Invoke(player.CurState);
			Attack1HoldCoroutine = StartCoroutine(CoAttackHold(OnAttack1Hold));
		}
		else
		{
			if(Attack1HoldCoroutine != null)
				StopCoroutine(Attack1HoldCoroutine);
			OnAttack1Up?.Invoke(player.CurState);
		}
	}

	private void OnAttack2(InputValue value)
	{
		Attack2Pressed = value.Get<float>() > 0.9f ? true : false;

		if (Attack2Pressed)
		{
			OnAttack2Down?.Invoke(player.CurState);
			Attack2HoldCoroutine = StartCoroutine(CoAttackHold(OnAttack2Hold));
		}
		else
		{
			if (Attack2HoldCoroutine != null)
				StopCoroutine(Attack2HoldCoroutine);
			OnAttack2Up?.Invoke(player.CurState);
		}
	}

	private IEnumerator CoAttackHold(UnityEvent<Player.State> holdevent)
	{
		float exitTime = Time.time + attackHoldTime;
		while (Time.time < exitTime)
		{
			yield return null;
		}
		holdevent?.Invoke(player.CurState);
	}

	public void ChangeStateToIdle(bool forceIdle = false)
	{
		curWeapon.ChangeStateToIdle(forceIdle);
	}

	public float GetAnimNormalizedTime(int layer)
	{
		return anim.GetCurrentAnimatorStateInfo(layer).normalizedTime;
	}

	public bool IsAnimWait(int layer)
	{
		return anim.GetCurrentAnimatorStateInfo(layer).IsName("Wait");
	}

	public bool IsAnimName(int layer, string name)
	{
		return anim.GetCurrentAnimatorStateInfo(layer).IsName(name);
	}

	public void SetAnimTrigger(string str)
	{
		anim.SetTrigger(str);
	}

	public void SetAnimFloat(string str, float value)
	{
		anim.SetFloat(str, value);
	}

	public void SetAnimFloat(string str, float value, float dampTime, float deltaTime)
	{
		anim.SetFloat(str, value, dampTime, deltaTime);
	}

	public void SetUnarmed()
	{
		curWeapon.SetUnArmed();
	}

	public void PlayAttackFailFeedback()
	{
		attackFailFeedback?.PlayFeedbacks();
	}
}