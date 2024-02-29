using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bow : Weapon
{
	[SerializeField] SkinnedMeshRenderer bowMesh;
	[SerializeField] GameObject arrowToDraw;
	[SerializeField] GameObject arrowToShoot;
	[SerializeField] GameObject leftHandArrow;
	[SerializeField] ParticleSystem iceParticle;
	[SerializeField] float arrowSpeed = 60f;
	public float arrowRigTime = 0.5f;
	public float arrowRigLerpSpeed = 6f;

	[Serializable]
	public enum State { Idle, Reload, StartAim, Aiming, UndoAim, Shot };
	public enum ArrowHoldMode { None, Draw, Hold, LeftHand };

	[SerializeField] State curState;

	ParticleSystem curParticle;
	StateMachine<State, Bow> stateMachine;
	ArrowHoldMode curArrowHold;
	LayerMask enemyMask;
	Collider[] cols = new Collider[10];

	public bool Reloaded { get; set; }
	public float ArrowSpeed { get { return arrowSpeed; } }

	protected override void Awake()
	{
		base.Awake();
		curArrowHold = ArrowHoldMode.None;
		curParticle = iceParticle;

		enemyMask = LayerMask.GetMask("Monster");

		stateMachine = new StateMachine<State, Bow>(this);
		stateMachine.AddState(State.Idle, new BowIdle(this, stateMachine));
		stateMachine.AddState(State.Reload, new BowReload(this, stateMachine));
		stateMachine.AddState(State.StartAim, new BowStartAim(this, stateMachine));
		stateMachine.AddState(State.Aiming, new BowAiming(this, stateMachine));
		stateMachine.AddState(State.UndoAim, new BowUndoAim(this, stateMachine));
		stateMachine.AddState(State.Shot, new BowShot(this, stateMachine));
	}

	protected override void Start()
	{
		base.Start();
		stateMachine.SetUp(State.Idle);
	}

	private void Update()
	{
		playerAttack.SetAnimFloat("IdleAdapter", 0f, 0.1f, Time.deltaTime);
		stateMachine.Update();
		curState = stateMachine.GetCurState();
	}

	public override void ChangeStateToIdle(bool forceIdle = false)
	{

	}

	public override void SetUnArmed()
	{

	}

	private void OnEnable()
	{
		player.SetNeckRigWeight(0.6f);
		playerMove.AimLockOffset = new Vector3(0f, 45f, 0f);
	}

	private void OnDisable()
	{
		player.SetNeckRigWeight(0f);
		playerMove.AimLockOffset = Vector3.zero;
	}

	public void SetArrowHold(ArrowHoldMode mode)
	{
		if (mode == curArrowHold) return;

		arrowToDraw.SetActive(false);
		arrowToShoot.SetActive(false);
		leftHandArrow.SetActive(false);

		curArrowHold = mode;
		switch (mode)
		{
			case ArrowHoldMode.None:
				break;
			case ArrowHoldMode.LeftHand:
				leftHandArrow.SetActive(true);
				break;
			case ArrowHoldMode.Draw:
				arrowToDraw.SetActive(true);
				break;
			case ArrowHoldMode.Hold:
				arrowToShoot.SetActive(true);
				break;
		}
	}

	public void SetBowWeight(float weight)
	{
		if(weight < 0f)
		{
			print($"weight 는 0~1 이어야함 : {weight}");
			weight = 0f;
		}
		else if(weight > 1f)
		{
			print($"weight 는 0~1 이어야함 : {weight}");
			weight = 1f;
		}
		weight *= 0.7f;
		bowMesh.SetBlendShapeWeight(0, weight * 100);
	}

	public Vector3 GetArrowShootPos()
	{
		return arrowToShoot.transform.position;
	}

	public void BounceHit(RaycastHit hitInfo, Vector3 pos, Vector3 velocity, int hitCnt, Arrow arrow)
	{
		Vector3 hitPoint = hitInfo.point;
		Vector3 normal = hitInfo.normal;
		velocity = -velocity;

		pos -= hitPoint;
		Quaternion toNormalRotation = Quaternion.FromToRotation(pos, normal);
		pos = toNormalRotation * toNormalRotation * pos;
		pos += hitPoint;
		velocity = toNormalRotation * toNormalRotation * velocity;

		if (hitCnt == 1)
		{
			for (int i = 0; i < 5; i++)
			{
				Arrow newArrow = GameManager.Resource.Instantiate<Arrow>("Prefab/Arrow", pos, Quaternion.identity, true);
				velocity += Random.insideUnitSphere * 3f;
				newArrow.Init(velocity, hitCnt, BounceHit);
			}
			arrow.Init(velocity, hitCnt, BounceHit);
		}
		else
		{
			if (hitCnt > 5)
			{
				arrow.Init(velocity, hitCnt);
			}
			else
			{
				arrow.Init(velocity, hitCnt, BounceHit);
			}
		}
	}

	public void TraceMonster(Arrow arrow)
	{
		int monsterNum = Physics.OverlapSphereNonAlloc(arrow.transform.position, 10f, cols, enemyMask);
		float minSqrDist = 99999f;
		Collider minCol = null;
		for(int i = 0; i < monsterNum; i++)
		{
			Collider col = cols[i];
			float sqrDist = (arrow.transform.position - col.transform.position).sqrMagnitude;
			if(sqrDist < minSqrDist)
			{
				minSqrDist = sqrDist;
				minCol = col;
			}
		}
		if (minCol == null) return;
		Vector3 monsterPos = minCol.transform.position;
		Vector3 monsterDir = monsterPos - arrow.transform.position;
		Vector3 vel = arrow.Velocity;

		Vector3 targetDir = vel + monsterDir * 0.1f;

		Quaternion rotation = Quaternion.FromToRotation(vel, targetDir);
		arrow.Velocity = rotation * vel;
	}

	public void PlayAimVFX(bool value)
	{
		if(value == true)
		{
			curParticle?.Play();
		}
		else
		{
			curParticle?.Stop();
		}
	}
}