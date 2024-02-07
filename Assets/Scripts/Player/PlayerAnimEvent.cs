using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimEvent : MonoBehaviour
{
	[HideInInspector] public UnityEvent OnAttackStart;

	private void Awake()
	{
		OnAttackStart = new UnityEvent();
	}

	private void AttackStart()
	{
		OnAttackStart?.Invoke();
	}
}