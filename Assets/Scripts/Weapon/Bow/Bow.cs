using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using Random = UnityEngine.Random;

public class Bow : Weapon
{
	[SerializeField] SkinnedMeshRenderer bowMesh;
	[SerializeField] GameObject arrowToDraw;
	[SerializeField] GameObject arrowToShoot;
	[SerializeField] GameObject leftHandArrow;
	[SerializeField] ParticleSystem normalParticle;
	[SerializeField] ParticleSystem windParticle;
	[SerializeField] ParticleSystem fireParticle;
	[SerializeField] float arrowSpeed = 60f;
	public float arrowRigTime = 0.5f;
	public float arrowRigLerpSpeed = 6f;

	[Serializable]
	public enum State { Idle, Reload, StartAim, Aiming, UndoAim, Shot, FastAim, FastShot };
	[Serializable]
	public enum ArrowState { None, Normal, Wind, Fire }
	public enum ArrowHoldMode { None, Draw, Hold, LeftHand };

	[SerializeField] ArrowState curArrowState;
	[SerializeField] State curState;

	ParticleSystem curAimParticle;
	StateMachine<State, Bow> stateMachine;
	ArrowHoldMode curArrowHold;
	LayerMask enemyMask;
	Collider[] cols = new Collider[10];

	Action<RaycastHit, Vector3, Vector3, int, Arrow> hitAction;
	Action<Arrow> updateAction;

	public int FastShotNum { get; set; } = 0;
	public bool Reloaded { get; set; }
	public float ArrowSpeed { get { return arrowSpeed; } }

	protected override void Awake()
	{
		base.Awake();
		ArrowSelect(ArrowState.Normal);
		curArrowHold = ArrowHoldMode.None;

		enemyMask = LayerMask.GetMask("Monster");

		stateMachine = new StateMachine<State, Bow>(this);
		stateMachine.AddState(State.Idle, new BowIdle(this, stateMachine));
		stateMachine.AddState(State.Reload, new BowReload(this, stateMachine));
		stateMachine.AddState(State.StartAim, new BowStartAim(this, stateMachine));
		stateMachine.AddState(State.Aiming, new BowAiming(this, stateMachine));
		stateMachine.AddState(State.UndoAim, new BowUndoAim(this, stateMachine));
		stateMachine.AddState(State.Shot, new BowShot(this, stateMachine));
		stateMachine.AddState(State.FastAim, new BowFastAim(this, stateMachine));
		stateMachine.AddState(State.FastShot, new BowFastShot(this, stateMachine));
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

	public void Shot()
	{
		Vector3 arrowPos = GetArrowShootPos();
		Vector3 aimPos = playerLook.AimPoint.position;
		Vector3 velocity = (aimPos - arrowPos).normalized * ArrowSpeed;
		Arrow arrow = GameManager.Resource.Instantiate<Arrow>("Prefab/Arrow", arrowPos, Quaternion.identity, true);

		arrow.Init(velocity, curArrowState, updateAction, hitAction);
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
				newArrow.Init(velocity * 0.7f, ArrowState.Normal, null, BounceHit, hitCnt);
			}
			arrow.Init(velocity * 0.7f, ArrowState.Normal, null, BounceHit, hitCnt);
		}
		else
		{
			if (hitCnt > 5)
			{
				arrow.Init(velocity, ArrowState.Normal);
			}
			else
			{
				arrow.Init(velocity, ArrowState.Normal, null, BounceHit, hitCnt);
			}
		}
	}

	public void Explosion(RaycastHit hitInfo, Vector3 pos, Vector3 velocity, int hitCnt, Arrow arrow)
	{
		GameObject obj = GameManager.Resource.Instantiate<GameObject>("Prefab/FireArrowExplosion", 
			hitInfo.point, Quaternion.identity, true);
		obj.transform.localScale = Vector3.one * 2f;
		arrow.BasicHit(hitInfo, pos, velocity, hitCnt, arrow);
	}

	public void TraceMonster(Arrow arrow)
	{
		int monsterNum = Physics.OverlapSphereNonAlloc(arrow.transform.position, 5f, cols, enemyMask);
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

		Vector3 monsterDir = (minCol.transform.position - arrow.transform.position).normalized;
		Vector3 vel = arrow.Velocity;
		arrow.Velocity = Vector3.Lerp(vel, monsterDir * vel.magnitude, Time.deltaTime * 20f);
	}

	public void PlayAimVFX(bool value)
	{
		if(value == true)
		{
			curAimParticle?.Play();
		}
		else
		{
			curAimParticle?.Stop();
		}
	}

	public override void SetUnArmed()
	{
		ArrowSelect arrowSelect = GameManager.UI.ShowPopUpUI<ArrowSelect>("UI/PopUpUI/ArrowSelect");
		arrowSelect.Init(ArrowSelect);
	}

	private void ArrowSelect(ArrowState arrowState)
	{
		if (arrowState == ArrowState.None) return;

		curArrowState = arrowState;
		updateAction = null;
		hitAction = null;

		switch(arrowState)
		{
			case ArrowState.Normal:
				curAimParticle = null;
				hitAction = BounceHit;
				break;
			case ArrowState.Fire:
				curAimParticle = fireParticle;
				hitAction = Explosion;
				break;
			case ArrowState.Wind:
				curAimParticle = windParticle;
				updateAction = TraceMonster;
				break;
			default:
				print($"올바르지 않은 ArrowState : {arrowState}");
				break;
		}
	}
}