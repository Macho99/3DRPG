using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackProcess { BeforeAttack, Attacking, AfterAttack, End }
public abstract class Weapon : MonoBehaviour
{
	protected LayerMask hitMask;
	protected LayerMask monsterMask;
	protected PlayerAttack playerAttack;
	protected PlayerAnimEvent playerAnimEvent;
	protected new MeshRenderer renderer;

	public LayerMask HitMask { get { return hitMask; } }
	public LayerMask MonsterMask { get { return monsterMask; } }
	public PlayerAttack PlayerAttack { get { return playerAttack; } }
	public PlayerAnimEvent PlayerAnimEvent { get { return playerAnimEvent; } }

	protected virtual void Awake()
	{
		hitMask = LayerMask.GetMask("Monster", "Environment", "Tree");
		monsterMask = LayerMask.GetMask("Monster");
		renderer = GetComponentInChildren<MeshRenderer>();
		playerAttack = FieldSFC.Player.GetComponent<PlayerAttack>();
	}

	protected virtual void Start()
	{
		playerAnimEvent = playerAttack.AnimEvent;
	}

	public abstract void SetUnArmed();
}
