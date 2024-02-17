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

	[HideInInspector] public UnityEvent<Player.State> OnAttack1Down;
	[HideInInspector] public UnityEvent<Player.State> OnAttack1Up;
	[HideInInspector] public UnityEvent<Player.State> OnAttack2Down;
	[HideInInspector] public UnityEvent<Player.State> OnAttack2Up;

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
		animEvent = GetComponentInChildren<PlayerAnimEvent>();
		anim = GetComponentInChildren<Animator>();
		weapons = weaponHolder.GetComponentsInChildren<Weapon>();

		OnAttack1Down = new UnityEvent<Player.State>();
		OnAttack1Up = new UnityEvent<Player.State>();
		OnAttack2Down = new UnityEvent<Player.State>();
		OnAttack2Up = new UnityEvent<Player.State>();

		if(weapons.Length > 0)
		{
			curWeapon = weapons[0];
		}
	}

	private void OnAttack1(InputValue value)
	{
		bool pressed = value.Get<float>() > 0.9f ? true : false;

		if (pressed)
			OnAttack1Down?.Invoke(player.CurState);
		else
			OnAttack1Up?.Invoke(player.CurState);
	}

	private void OnAttack2(InputValue value)
	{
		bool pressed = value.Get<float>() > 0.9f ? true : false;

		if (pressed)
			OnAttack2Down?.Invoke(player.CurState);
		else
			OnAttack2Up?.Invoke(player.CurState);
	}

	public float GetAnimNormalizedTime(int layer)
	{
		return anim.GetCurrentAnimatorStateInfo(layer).normalizedTime;
	}

	public bool IsAnimWait(int layer)
	{
		return anim.GetCurrentAnimatorStateInfo(layer).IsName("Wait");
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
}