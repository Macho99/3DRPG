using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterAction : MonoBehaviour
{
	private int curHp;

	[HideInInspector] public UnityEvent<float> OnHpChanged;

	public int CurHp { get => curHp; }
}