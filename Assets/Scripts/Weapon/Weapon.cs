using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Weapon : MonoBehaviour
{
	protected LayerMask hitMask;
	protected LayerMask monsterMask;

	public Sprite itemSkillIcon;
	public float damage;

	protected virtual void Awake()
	{
		hitMask = LayerMask.GetMask("Monster", "Environment", "Tree");
		monsterMask = LayerMask.GetMask("Monster");
	}

	public abstract void Attack();
}
