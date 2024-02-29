using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackProcess { BeforeAttack, Attacking, AfterAttack, End }
public abstract class Weapon : MonoBehaviour
{
	[SerializeField] RuntimeAnimatorController controller;

	protected LayerMask hitMask;
	protected LayerMask monsterMask;
	protected Player player;
	protected PlayerMove playerMove;
	protected PlayerAttack playerAttack;
	protected PlayerAnimEvent playerAnimEvent;
	protected PlayerCamManager playerCamManager;
	protected new MeshRenderer renderer;

	public LayerMask HitMask { get { return hitMask; } }
	public LayerMask MonsterMask { get { return monsterMask; } }
	public Player Player { get { return player; } }
	public PlayerMove PlayerMove { get { return playerMove; } }
	public PlayerAttack PlayerAttack { get { return playerAttack; } }
	public PlayerAnimEvent PlayerAnimEvent { get { return playerAnimEvent; } }
	public PlayerCamManager PlayerCamManager { get { return playerCamManager; } }

	protected virtual void Awake()
	{
		hitMask = LayerMask.GetMask("Monster", "Environment", "Tree");
		monsterMask = LayerMask.GetMask("Monster");
		renderer = GetComponentInChildren<MeshRenderer>();
		player = FieldSFC.Player.GetComponent<Player>();
		playerAttack = player.GetComponent<PlayerAttack>();
		playerMove = player.GetComponent<PlayerMove>();
		playerCamManager = player.GetComponent<PlayerCamManager>();
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

	public RuntimeAnimatorController GetAnimController() {
		return controller; 
	}
}
