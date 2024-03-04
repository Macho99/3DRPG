using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class BowSkillFireRain : StateBase<Bow.State, Bow>
{
	const float maxDist = 70f;
	const float arriveTime = 3f;
	Player player;
	PlayerLook playerLook;
	Transform aimPoint;
	PlayerAttack playerAttack;
	PlayerMove playerMove;
	PlayerCamManager playerCamManager;
	DecalProjector decal;
	LayerMask groundMask;

	bool waitAnim;

	public BowSkillFireRain(Bow owner, StateMachine<Bow.State, Bow> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		waitAnim = false;	
		player.ChangeState(Player.State.MoveAttack);
		playerMove.AimLockOffset = Vector3.zero;
		playerMove.AimLock = true;
		playerMove.MoveMultiplier = 0f;
		playerCamManager.SetFireRainCastCam(true);
		playerAttack.SetAnimTrigger("Hold1");
		playerAttack.OnAttack1Down.AddListener(SkillCast);
		playerAttack.OnAttack2Down.AddListener(SkillUndo);
		playerAttack.OnEButtonDown.AddListener(SkillUndo);
		aimPoint = playerLook.AimPoint;
		decal = GameManager.Resource.Instantiate<DecalProjector>("Prefab/FireRainDecal", 
			aimPoint.transform.position, Quaternion.Euler(90f, 0f, 0f), true);
		_ = owner.StartCoroutine(CoAddjustBowWeight());
	}

	private IEnumerator CoAddjustBowWeight()
	{
		yield return new WaitUntil(() => playerAttack.IsAnimName(0, "Hold1"));
		while (playerAttack.IsAnimName(0, "Hold1") && waitAnim == false)
		{
			owner.SetBowWeight(playerAttack.GetAnimNormalizedTime(0));
			yield return null;
		}
	}

	public override void Exit()
	{
		if (decal != null)
			GameManager.Resource.Destroy(decal.gameObject);

		playerMove.MoveMultiplier = 1f;
		playerMove.AimLockOffset = new Vector3(0f, 45f, 0f);
		playerCamManager.SetFireRainCastCam(false);
		playerAttack.OnAttack1Down.RemoveListener(SkillCast);
		playerAttack.OnAttack2Down.RemoveListener(SkillUndo);
		playerAttack.OnEButtonDown.RemoveListener(SkillUndo);
	}

	public override void Setup()
	{
		player = owner.Player;
		playerMove = owner.PlayerMove;
		playerLook = owner.PlayerLook;
		playerAttack = owner.PlayerAttack;
		playerCamManager = owner.PlayerCamManager;
		groundMask = LayerMask.GetMask("Environment");
	}

	public override void Transition()
	{

	}

	public override void Update()
	{
		if (waitAnim == true) return;

		if((aimPoint.position - owner.transform.position).sqrMagnitude < maxDist * maxDist)
		{
			Vector3 decalPos = aimPoint.position;
			decalPos.y += 10f;
			decal.transform.position = decalPos;
		}

		Quaternion quaternion = Quaternion.Euler(0f, 20f * Time.deltaTime, 0f);
		decal.transform.rotation = quaternion * decal.transform.rotation;
	}

	private void SkillCast(Player.State state)
	{
		if (waitAnim == true) return;

		waitAnim = true;
		_ = owner.StartCoroutine(CoSkillCast());
	}

	private IEnumerator CoSkillCast()
	{
		playerAttack.SetAnimTrigger("Attack1"); 

		Shot();

		GameManager.Resource.Destroy(decal.gameObject, arriveTime);
		decal = null;
		owner.SetBowWeight(0f);
		owner.Reloaded = false;
		owner.SetArrowHold(Bow.ArrowHoldMode.None);


		yield return new WaitUntil(() => playerAttack.IsAnimWait(0));
		playerAttack.SetAnimTrigger("BaseExit");
		stateMachine.ChangeState(Bow.State.Idle);
	}

	private void Shot()
	{
		if (Physics.Raycast(decal.transform.position, Vector3.down, out RaycastHit hitInfo, 50f, groundMask) == false)
		{
			Debug.LogError("데칼 아래쪽에 지형이 없음");
			return;
		}

		for(int i = 0; i < 8; i++)
		{
			Arrow arrow = owner.ManualShot();

			Vector3 targetPos = hitInfo.point;
			Vector3 offset = Random.insideUnitSphere;
			offset.y = 0f;
			offset *= 3f;
			targetPos += offset;

			Vector3 velocity = (targetPos - arrow.transform.position) / arriveTime;
			velocity.y = (targetPos.y - arrow.transform.position.y) / arriveTime
				+ (arriveTime * -arrow.Gravity.y) * 0.5f;

			arrow.Init(velocity, Bow.ArrowProperty.Fire, null, owner.Explosion);
		}
	}

	private void SkillUndo(Player.State state)
	{
		if (waitAnim == true) return;

		waitAnim = true;
		_ = owner.StartCoroutine(CoReturnToIdle());
	}

	private IEnumerator CoReturnToIdle()
	{
		yield return new WaitUntil(() => playerAttack.IsAnimName(0, "Hold1Idle") == true);
		GameManager.Resource.Destroy(decal.gameObject);
		decal = null;
		playerAttack.SetAnimTrigger("BaseExit");
		stateMachine.ChangeState(Bow.State.Idle);
	}
}