using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackProcess { BeforeAttack, Attacking, AfterAttack, End }
public abstract class Weapon : MonoBehaviour
{
	protected LayerMask hitMask;
	protected LayerMask monsterMask;
	protected Player player;
	protected PlayerAttack playerAttack;
	protected PlayerAnimEvent playerAnimEvent;
	protected new MeshRenderer renderer;

	public LayerMask HitMask { get { return hitMask; } }
	public LayerMask MonsterMask { get { return monsterMask; } }
	public Player Player { get { return player; } }
	public PlayerAttack PlayerAttack { get { return playerAttack; } }
	public PlayerAnimEvent PlayerAnimEvent { get { return playerAnimEvent; } }

	protected virtual void Awake()
	{
		hitMask = LayerMask.GetMask("Monster", "Environment", "Tree");
		monsterMask = LayerMask.GetMask("Monster");
		renderer = GetComponentInChildren<MeshRenderer>();
		player = FieldSFC.Player.GetComponent<Player>();
		playerAttack = player.GetComponent<PlayerAttack>();
	}

	protected virtual void Start()
	{
		playerAnimEvent = playerAttack.AnimEvent;
	}

	public abstract void SetUnArmed();

	public void ChangePlayerState(Player.State state)
	{
		switch (state)
		{
			case Player.State.StandAttack:
			case Player.State.MoveAttack:
			case Player.State.OnAirAttack:
				player.ChangeState(state);
				break;
			default:
				print($"{state}는 Weapon이 변경할 수 없는 상태입니다");
				break;
		}
	}

	public abstract void ChangeStateToIdle(bool forceIdle = false);
}
