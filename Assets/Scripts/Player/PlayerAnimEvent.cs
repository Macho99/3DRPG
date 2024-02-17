using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimEvent : MonoBehaviour
{
	[HideInInspector] public UnityEvent OnAttackStart;
	[HideInInspector] public UnityEvent OnAttackEnd;
	[HideInInspector] public UnityEvent OnEquipChange;

	private void Awake()
	{
		OnAttackStart = new UnityEvent();
		OnAttackEnd = new UnityEvent();
	}

	private void AttackStart()
	{
		OnAttackStart?.Invoke();
	}

	private void AttackEnd()
	{
		OnAttackEnd?.Invoke();
	}

	private void EquipChange()
	{
		OnEquipChange?.Invoke();
	}
}