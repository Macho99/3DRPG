using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
	[SerializeField] Transform weaponHolder;
	
	public PlayerAnimEvent AnimEvent { get => animEvent; }

	private PlayerAnimEvent animEvent;
	private Weapon[] weapons;
	private Weapon curWeapon;
	private Animator anim;

	private void Awake()
	{
		animEvent = GetComponentInChildren<PlayerAnimEvent>();
		anim = GetComponentInChildren<Animator>();
		weapons = weaponHolder.GetComponentsInChildren<Weapon>();
		if(weapons.Length > 0)
		{
			curWeapon = weapons[0];
		}
	}

	private void OnAttack1(InputValue value)
	{
		if (value.isPressed == true)
		{
			curWeapon.Attack();
		}
	}

	public bool IsAnimWait(int layer)
	{
		return anim.GetCurrentAnimatorStateInfo(layer).IsName("Wait");
	}

	public void SetAnimTrigger(string str)
	{
		anim.SetTrigger(str);
	}
}