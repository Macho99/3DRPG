using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimEvent : MonoBehaviour
{
	[HideInInspector] public UnityEvent OnAttackStart;
	[HideInInspector] public UnityEvent OnAttackEnd;
	[HideInInspector] public UnityEvent OnClockWiseAttack;
	[HideInInspector] public UnityEvent OnCounterClockWiseAttack;
	[HideInInspector] public UnityEvent OnEquipChange;
	[HideInInspector] public UnityEvent OnJumpStart;

	private void Awake()
	{
		OnAttackStart = new UnityEvent();
		OnAttackEnd = new UnityEvent();
		OnClockWiseAttack = new UnityEvent();
		OnCounterClockWiseAttack = new UnityEvent();
		OnEquipChange = new UnityEvent();
		OnJumpStart = new UnityEvent();
	}

	private void AttackStart()
	{
		OnAttackStart?.Invoke();
	}

	private void AttackEnd()
	{
		OnAttackEnd?.Invoke();
	}

	private void ClockWiseAttack()
	{
		OnClockWiseAttack?.Invoke();
	}

	private void CounterClockWiseAttack()
	{
		OnCounterClockWiseAttack?.Invoke();
	}

	private void EquipChange()
	{
		OnEquipChange?.Invoke();
	}

	private void JumpStart()
	{
		OnJumpStart?.Invoke();
	}
}