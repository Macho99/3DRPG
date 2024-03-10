using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using static UnityEngine.Rendering.DebugUI.Table;
using Random = UnityEngine.Random;

public class Bow : Weapon
{
	[SerializeField] TargetFollower leftHandWeaponSlotFollower;
	[SerializeField] TargetFollower spine3Follower;
	[SerializeField] TargetFollower rightHandFollower;
	[SerializeField] SkinnedMeshRenderer bowMesh;
	[SerializeField] GameObject arrowToDraw;
	[SerializeField] MeshRenderer arrowToShoot;
	[SerializeField] GameObject leftHandArrow;
	[SerializeField] ParticleSystem iceParticle;
	[SerializeField] ParticleSystem windParticle;
	[SerializeField] ParticleSystem fireParticle;
	[SerializeField] ParticleSystem fastShotVFX;
	[SerializeField] ParticleSystem enhanceIceVFX;
	[SerializeField] Material fastShotArrowMat;
	[SerializeField] float arrowSpeed = 60f;
	public float arrowRigTime = 0.5f;
	public float arrowRigLerpSpeed = 6f;

	[Serializable]
	public enum State { Inactive, Idle, Reload, StartAim, Aiming, UndoAim, Shot, FastAim, FastShot, 
		FireRain, Ulti, };
	[Serializable]
	public enum ArrowProperty { Ice, Wind, Fire }
	public enum ArrowHoldMode { None, Draw, Hold, LeftHand };

	[SerializeField] ArrowProperty curArrowProperty;
	[SerializeField] State curState;

	ParticleSystem curAimParticle;
	StateMachine<State, Bow> stateMachine;
	ArrowHoldMode curArrowHold;
	LayerMask enemyMask;
	Collider[] traceCols = new Collider[10];
	BowUI bowUI;

	Action<RaycastHit, int, Arrow> hitAction;
	Action<Arrow> updateAction;
	Material initMat;

	[SerializeField] float[] qCooltimes;
	[SerializeField] float[] eCooltimes;
	[SerializeField] float ultiSkillCooltime = 180f;
	[SerializeField] int[] qManas;
	[SerializeField] int[] eManas;
	[SerializeField] int ultiMana;


	WindSkillController windController;

	float[] qUseableTimes = new float[3];
	float[] eUseableTimes = new float[3];
	float ultiSkillUseableTime = 0f;

	bool aimLock;

	public bool AimLock { get { return aimLock; } set {
			aimLock = value;
			if(aimLock == true)
			{
				HideAimPoint(false);
			}
			else
			{
				HideAimPoint(true);
			}
		} }
	public ArrowProperty CurArrowProperty { get { return curArrowProperty; } }
	public int FastShotNum { get; set; } = 0;
	public bool Reloaded { get; set; }
	public float ArrowSpeed { get { return arrowSpeed; } }

	protected override void Awake()
	{
		base.Awake();
		curArrowHold = ArrowHoldMode.None;
		initMat = arrowToShoot.material;

		enemyMask = LayerMask.GetMask("Monster");

		stateMachine = new StateMachine<State, Bow>(this);
		stateMachine.AddState(State.Inactive, new BowInactive(this, stateMachine));
		stateMachine.AddState(State.Idle, new BowIdle(this, stateMachine));
		stateMachine.AddState(State.Reload, new BowReload(this, stateMachine));
		stateMachine.AddState(State.StartAim, new BowStartAim(this, stateMachine));
		stateMachine.AddState(State.Aiming, new BowAiming(this, stateMachine));
		stateMachine.AddState(State.UndoAim, new BowUndoAim(this, stateMachine));
		stateMachine.AddState(State.Shot, new BowShot(this, stateMachine));
		stateMachine.AddState(State.FastAim, new BowFastAim(this, stateMachine));
		stateMachine.AddState(State.FastShot, new BowFastShot(this, stateMachine));
		stateMachine.AddState(State.FireRain, new BowSkillFireRain(this, stateMachine));
		stateMachine.AddState(State.Ulti, new BowSkillUlti(this, stateMachine));
	}

	public override void Init(WeaponItem weaponItem)
	{
		base.Init(weaponItem);
		leftHandWeaponSlotFollower.SetTarget(player.GetTransform(Player.FollowTransform.LeftHandWeaponSlot));
		spine3Follower.SetTarget(player.GetTransform(Player.FollowTransform.Spine3));
		rightHandFollower.SetTarget(player.GetTransform(Player.FollowTransform.RightHand));
	}

	protected override void Start()
	{
		base.Start();
		ArrowSelect(ArrowProperty.Ice);
		stateMachine.SetUp(State.Inactive);
	}

	private void Update()
	{
		playerAttack.SetAnimFloat("IdleAdapter", 0f, 0.1f, Time.deltaTime);
		stateMachine.Update();
		curState = stateMachine.GetCurState();
	}

	public override void ChangeStateToIdle(bool forceIdle = false)
	{
		player.SetBowAimRigWeight(0f, 0f, 0f);
		playerAttack.SetAnimTrigger("Init");
		stateMachine.ChangeState(State.Idle);
		return;
	}

	private void OnEnable()
	{
		bowUI = GameManager.UI.ShowSceneUI<BowUI>("UI/SceneUI/BowUI");
		bowUI.UIUpdate(curArrowProperty, GetCooltimeStruct());
		player.SetNeckRigWeight(0.6f);
		playerMove.AimLockOffset = new Vector3(0f, 45f, 0f);
	}

	private void OnDisable()
	{
		bowUI.CloseUI();
		bowUI = null; 
		if(windController != null)
		{
			windController.Stop();
			GameManager.Resource.Destroy(windController.gameObject);
			windController = null;
		}
		player.SetNeckRigWeight(0f);
		playerMove.AimLockOffset = Vector3.zero;
	}

	public void SetArrowHold(ArrowHoldMode mode)
	{
		if (mode == curArrowHold) return;

		arrowToDraw.SetActive(false);
		arrowToShoot.gameObject.SetActive(false);
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
				arrowToShoot.gameObject.SetActive(true);
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

		arrow.Init(velocity, curArrowProperty, updateAction, hitAction);
	}

	public Arrow ManualShot()
	{
		Arrow arrow = GameManager.Resource.Instantiate<Arrow>("Prefab/Arrow", GetArrowShootPos(), Quaternion.identity, true);
		return arrow;
	}

	public void MonsterHit(RaycastHit hitInfo, int hitCnt, Arrow arrow)
	{
		int layer = hitInfo.collider.gameObject.layer;
		if(IsMonsterLayer(layer) == true)
		{
			MonsterAttack(hitInfo.collider.gameObject, FinalDamage);
            windController?.Attack(hitInfo.transform);
			arrow.transform.parent = hitInfo.collider.transform;
			arrow.AutoOff();
		}
		else
		{
			arrow.GroundHit(hitInfo, hitCnt, arrow);
		}
	}

	private Vector3 GetBounceVelocity(RaycastHit hitInfo, Arrow arrow)
	{
		Vector3 hitPoint = hitInfo.point;
		Vector3 normal = hitInfo.normal;
		Vector3 velocity = -arrow.Velocity;

		Vector3 pos = arrow.transform.position;
		pos -= hitPoint;
		Quaternion toNormalRotation = Quaternion.FromToRotation(pos, normal);
		pos = toNormalRotation * toNormalRotation * pos;
		pos += hitPoint;
		velocity = toNormalRotation * toNormalRotation * velocity;

		return velocity;
	}

	private void SplitBounceHit(RaycastHit hitInfo, int hitCnt, Arrow arrow)
	{
		if (IsMonsterLayer(hitInfo.collider.gameObject.layer))
		{
			MonsterHit(hitInfo, hitCnt, arrow);
			//return;
		}
		Vector3 velocity = GetBounceVelocity(hitInfo, arrow);

		for (int i = 0; i < 4; i++)
		{
			Arrow newArrow = GameManager.Resource.Instantiate<Arrow>("Prefab/Arrow", 
				arrow.transform.position, Quaternion.identity, true);
			velocity += Random.insideUnitSphere * 3f;
			newArrow.Init(velocity * 0.7f, ArrowProperty.Ice, null, BounceHit, hitCnt);
		}
		arrow.Init(velocity * 0.7f, ArrowProperty.Ice, null, BounceHit, hitCnt);
	}

	public void BounceHit(RaycastHit hitInfo, int hitCnt, Arrow arrow)
	{
		if (IsMonsterLayer(hitInfo.collider.gameObject.layer))
		{
			MonsterHit(hitInfo, hitCnt, arrow);
			return;
		}

		Vector3 velocity = GetBounceVelocity(hitInfo, arrow);

		if (hitCnt > 5)
		{
			arrow.Init(velocity, ArrowProperty.Ice, null, MonsterHit);
		}
		else
		{
			arrow.Init(velocity, ArrowProperty.Ice, null, BounceHit, hitCnt);
		}
	}

	public void Explosion(RaycastHit hitInfo, int hitCnt, Arrow arrow)
	{
		GameObject obj = GameManager.Resource.Instantiate<GameObject>("Prefab/FireArrowExplosion", 
			hitInfo.point, Quaternion.identity, true);
		obj.transform.localScale = Vector3.one * 2f;
		SphereCastAttack(hitInfo.point, 1.5f, FinalDamage);
		arrow.GroundHit(hitInfo, hitCnt, arrow);
	}

	public void TraceMonster(Arrow arrow)
	{
		int monsterNum = Physics.OverlapSphereNonAlloc(arrow.transform.position, 5f, traceCols, enemyMask);
		float minSqrDist = 99999f;
		Collider minCol = null;
		for(int i = 0; i < monsterNum; i++)
		{
			Collider col = traceCols[i];
			float sqrDist = (arrow.transform.position - col.transform.position).sqrMagnitude;
			if(sqrDist < minSqrDist)
			{
				minSqrDist = sqrDist;
				minCol = col;
			}
		}
		if (minCol == null) return;

		Vector3 monsterDir = (minCol.transform.position + Vector3.up - arrow.transform.position).normalized;
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

	private void ArrowSelect(ArrowProperty property)
	{
		curArrowProperty = property;
		updateAction = null;
		hitAction = MonsterHit;

		switch(property)
		{
			case ArrowProperty.Ice:
				curAimParticle = iceParticle;
				hitAction = BounceHit;
				break;
			case ArrowProperty.Fire:
				curAimParticle = fireParticle;
				hitAction = Explosion;
				break;
			case ArrowProperty.Wind:
				curAimParticle = windParticle;
				updateAction = TraceMonster;
				break;
			default:
				print($"올바르지 않은 ArrowState : {property}");
				break;
		}

		bowUI.UIUpdate(property, GetCooltimeStruct());
	}

	private BowUI.SkillCooltimeStruct GetCooltimeStruct()
	{
		BowUI.SkillCooltimeStruct cooltime = new();
		int curIdx = (int)curArrowProperty;
		cooltime.qCooltime = qCooltimes[curIdx];
		cooltime.qLefttime = qUseableTimes[curIdx] - Time.time;
		cooltime.eCooltime = eCooltimes[curIdx];
		cooltime.eLefttime = eUseableTimes[curIdx] - Time.time;
		cooltime.rCooltime = ultiSkillCooltime;
		cooltime.rLefttime = ultiSkillUseableTime - Time.time;
		return cooltime;
	}

	public void SetFastShotArrowMat(bool value)
	{
		if(value == true)
		{
			arrowToShoot.material = fastShotArrowMat;
		}
		else
		{
			arrowToShoot.material = initMat;
		}
	}
	
	public void PlayFastShotVFX()
	{
		fastShotVFX.Play();
	}

	public void QSkill()
	{
		int curIdx = (int)curArrowProperty;
		if(Time.time > qUseableTimes[curIdx])
		{
			if (GameManager.Stat.TrySubCurMP(qManas[curIdx]) == true)
			{
				qUseableTimes[curIdx] = Time.time + qCooltimes[curIdx];
				bowUI.UIUpdate(curArrowProperty, GetCooltimeStruct());
				AimLock = true;
				stateMachine.ChangeState(Bow.State.FastAim);
			}
		}
	}

	public void IceESkill()
	{
		int curIdx = (int)curArrowProperty;
		if (Time.time > eUseableTimes[curIdx])
		{
			if (GameManager.Stat.TrySubCurMP(eManas[curIdx]) == true)
			{
				eUseableTimes[curIdx] = Time.time + eCooltimes[curIdx];
				bowUI.UIUpdate(curArrowProperty, GetCooltimeStruct());
				_ = StartCoroutine(CoIceSkill());
			}
		}
	}
	public void WindESkill()
	{
		int curIdx = (int)curArrowProperty;
		if (Time.time > eUseableTimes[curIdx])
		{
			if (GameManager.Stat.TrySubCurMP(eManas[curIdx]) == true)
			{
				eUseableTimes[curIdx] = Time.time + eCooltimes[curIdx];
				bowUI.UIUpdate(curArrowProperty, GetCooltimeStruct());
				_ = StartCoroutine(CoWindSkill());
			}
		}
	}

	public bool FireESkill()
	{
		int curIdx = (int)curArrowProperty;
		if (Time.time > eUseableTimes[curIdx])
		{
			if (GameManager.Stat.TrySubCurMP(eManas[curIdx]) == true)
			{
				eUseableTimes[curIdx] = Time.time + eCooltimes[curIdx];
				bowUI.UIUpdate(curArrowProperty, GetCooltimeStruct());
				return true;
			}
		}
		return false;
	}

	private IEnumerator CoIceSkill()
	{
		float endTime = Time.time + 10f;
		hitAction = SplitBounceHit;
		enhanceIceVFX.Play();

		while ((curArrowProperty == ArrowProperty.Ice) && (Time.time < endTime))
		{
			yield return null;
		}

		if(curArrowProperty == ArrowProperty.Ice)
		{
			hitAction = BounceHit;
		}
		enhanceIceVFX.Stop();
	}


	private IEnumerator CoWindSkill()
	{
		float endTime = Time.time + eCooltimes[(int) ArrowProperty.Wind] * 0.5f;
		windController = GameManager.Resource.Instantiate<WindSkillController>(
			"Prefab/WindSkillController", transform.position, Quaternion.identity, true);
		windController.Init(5, (int) (BaseDamage * 0.3f));

		while((curArrowProperty == ArrowProperty.Wind) && (Time.time < endTime))
		{
			yield return null;
		}

		windController.Stop();
		GameManager.Resource.Destroy(windController.gameObject);
		windController = null;
	}

	public void WindControllerPrepareAttack()
	{
		windController?.PrepareAttack();
	}

	public bool UltiSkill()
	{
		if (Time.time > ultiSkillUseableTime)
		{
			if(GameManager.Stat.TrySubCurMP(ultiMana) == true)
			{
				ultiSkillUseableTime = Time.time + ultiSkillCooltime;
				bowUI.UIUpdate(curArrowProperty, GetCooltimeStruct());
				SetUltiRow(0, 11);
				SetUltiRow(1, 8);
				SetUltiRow(2, 5);
				return true;
			}
		}
		return false;
	}

	private void SetUltiRow(int rowNum, int col)
	{
		float yOffset = 2f;
		float xInterval = 0.7f;
		for (int i = 0; i < col; i++)
		{
			int xPos = (i - col / 2);
			Vector3 position = transform.position;
			position -= transform.right * (xPos * xInterval)
				+ transform.right * Random.Range(-0.1f, 0.1f);
			position.y += yOffset + 0.5f * rowNum;
			position += transform.forward * Random.Range(-0.15f, 0.1f);
			position += transform.forward * xPos * xPos * 0.05f;
			Quaternion rotation = transform.rotation * 
				Quaternion.Euler(
				rowNum * 7f + Random.Range(-2f, 2f),
				xPos * 3f + Random.Range(-2f, 2f), 
				0f);

			BowUltiElem ultiElem = GameManager.Resource.Instantiate<BowUltiElem>("Prefab/BowUltiElem",
				position, rotation, true);
			ultiElem.Init(this, (ArrowProperty)Random.Range(0, 3));
		}
	}
	
	public override void ForceInactive()
	{
		AimLock = false;
		playerCamManager.SetAimCam(false);
		playerMove.AimLock = false;
		stateMachine.ChangeState(State.Inactive);
	}

	public void SetAimPointSize(float weight)
	{
		bowUI.SetOutCircleScale(weight);
	}

	public void HideAimPoint(bool value)
	{
		bowUI.AimPointHide(value);
	}

}