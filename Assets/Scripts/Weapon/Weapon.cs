using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AttackProcess { BeforeAttack, Attacking, AfterAttack, End }
public abstract class Weapon : MonoBehaviour
{
	[SerializeField] RuntimeAnimatorController controller;

	protected WeaponItem weaponItem;
	protected LayerMask hitMask;
	protected LayerMask monsterMask;
	protected LayerMask groundMask;
	protected Player player;
	protected PlayerMove playerMove;
	protected PlayerAttack playerAttack;
	protected PlayerAnimEvent playerAnimEvent;
	protected PlayerLook playerLook;
	protected PlayerCamManager playerCamManager;
	protected new MeshRenderer renderer;

	private Collider[] sphereCols = new Collider[10];

	public int BaseDamage { get; protected set; }
	public float DamageMultiplier { get; set; } = 1f;
	public int FinalDamage { get { return (int)(BaseDamage * DamageMultiplier); } }
	public WeaponItem WeaponItem { get { return weaponItem; } }
	public LayerMask HitMask { get { return hitMask; } }
	public LayerMask MonsterMask { get { return monsterMask; } }
	public LayerMask GroundMask { get { return groundMask; } }
	public Player Player { get { return player; } }
	public PlayerMove PlayerMove { get { return playerMove; } }
	public PlayerLook PlayerLook { get { return playerLook; } }
	public PlayerAttack PlayerAttack { get { return playerAttack; } }
	public PlayerAnimEvent PlayerAnimEvent { get { return playerAnimEvent; } }
	public PlayerCamManager PlayerCamManager { get { return playerCamManager; } }

	public UnityEvent OnMonsterAttack = new();

	protected virtual void Awake()
	{
		hitMask = LayerMask.GetMask("Monster", "Environment", "Tree");
		monsterMask = LayerMask.GetMask("Monster");
		groundMask = LayerMask.GetMask("Environment", "Tree");
		renderer = GetComponentInChildren<MeshRenderer>();
		player = FieldSFC.Player.GetComponent<Player>();
		playerAttack = player.GetComponent<PlayerAttack>();
		playerMove = player.GetComponent<PlayerMove>();
		playerLook = player.GetComponent<PlayerLook>();
		playerCamManager = player.GetComponent<PlayerCamManager>();
	}

	protected virtual void Start()
	{
		playerAnimEvent = playerAttack.AnimEvent;
	}

	public abstract void SetUnArmed();
	public abstract void ForceInactive();

	public virtual void Init(WeaponItem weaponItem)
	{
		this.weaponItem = weaponItem;
		BaseDamage = weaponItem.Damage;
	}

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

	public RuntimeAnimatorController GetAnimController() 
	{
		return controller; 
	}

	public bool IsMonsterLayer(int layer)
	{
		if((monsterMask.value & (1 << layer)) != 0)
		{
			return true;
		}
		return false;
	}

	public void SphereCastAttack(Vector3 center, float radius, int damage)
	{
		int hit = Physics.OverlapSphereNonAlloc(center, radius, sphereCols, monsterMask);

		if(hit >= sphereCols.Length)
		{
			hit = sphereCols.Length - 1;
		}

		for (int i = 0; i < hit; i++)
		{
			Collider col = sphereCols[i];
			MonsterAttack(col.gameObject, damage);
		}
	}

	public void MonsterAttack(GameObject gameObject, int damage)
	{
		if (gameObject.TryGetComponent(out Monster monster))
		{
			monster.TakeDamage(damage);
			OnMonsterAttack?.Invoke();
		}
		else if (gameObject.TryGetComponent(out DeathKnight deathKnight))
		{
			deathKnight.TakeDamage(damage);
			OnMonsterAttack?.Invoke();
		}
	}
}