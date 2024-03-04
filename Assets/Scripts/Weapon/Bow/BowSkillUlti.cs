using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.Playables;

public class BowSkillUlti : StateBase<Bow.State, Bow>
{
	Player player;
	PlayerLook playerLook;
	Transform aimPoint;
	PlayerAttack playerAttack;
	PlayerMove playerMove;
	PlayerCamManager playerCamManager;
	DecalProjector decal;

	bool waitAnim;

	public BowSkillUlti(Bow owner, StateMachine<Bow.State, Bow> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		waitAnim = false;
		player.ChangeState(Player.State.MoveAttack);
		owner.SetArrowHold(Bow.ArrowHoldMode.LeftHand);
		playerMove.AimLockOffset = Vector3.zero;
		playerMove.AimLock = true;
		playerMove.MoveMultiplier = 1f;
		playerCamManager.SetBowUltiCastCam(true);
		//playerAttack.SetAnimTrigger("Hold1");
		playerAttack.OnAttack1Down.AddListener(SkillCast);
		playerAttack.OnAttack2Down.AddListener(SkillUndo);
		playerAttack.OnEButtonDown.AddListener(SkillUndo);
		aimPoint = playerLook.AimPoint;
		decal = GameManager.Resource.Instantiate<DecalProjector>("Prefab/BowUltiDecal",
			aimPoint.transform.position, Quaternion.Euler(90f, 0f, 0f), true);
	}

	public override void Exit()
	{
		if (decal != null)
			GameManager.Resource.Destroy(decal.gameObject);

		playerMove.AimLock = false;
		playerMove.MoveMultiplier = 1f;
		playerMove.AimLockOffset = new Vector3(0f, 45f, 0f);
		playerCamManager.SetAimCam(false);
		playerCamManager.SetBowUltiCastCam(false);
		playerAttack.OnAttack1Down.RemoveListener(SkillCast);
		playerAttack.OnAttack2Down.RemoveListener(SkillUndo);
		playerAttack.OnEButtonDown.RemoveListener(SkillUndo);
	}

	public override void Setup()
	{
		waitAnim = false;
		player = owner.Player;
		playerMove = owner.PlayerMove;
		playerLook = owner.PlayerLook;
		playerAttack = owner.PlayerAttack;
		playerCamManager = owner.PlayerCamManager;
	}

	public override void Transition()
	{

	}

	public override void Update()
	{
		if (waitAnim == true) return;

		Vector3 decalPosition = player.transform.position + player.transform.forward * 10f;
		decalPosition.y += 10f;
		decal.transform.position = decalPosition;
		decal.transform.right = player.transform.forward;
		decal.transform.rotation = decal.transform.rotation * Quaternion.Euler(90f, 0f, 0f);
	}

	private void SkillCast(Player.State state)
	{
		if(waitAnim == true) return;
		waitAnim = true;

		GameManager.Resource.Destroy(decal.gameObject);
		decal = null;
		_ = owner.StartCoroutine(CoSkillCast());
	}

	private IEnumerator CoSkillCast()
	{
		playerMove.MoveMultiplier = 0f;
		playerAttack.SetAnimTrigger("Attack2");
		yield return new WaitUntil(() => playerAttack.IsAnimName(0, "Attack2") == true || waitAnim == false);
		player.GetComponent<PlayableDirector>().Play();
		owner.UltiSkill();
		yield return new WaitUntil(() => playerAttack.GetAnimNormalizedTime(0) > 0.8f || waitAnim == false);
		if(waitAnim != false)
		{
			playerAttack.SetAnimTrigger("BaseExit");
		}

		stateMachine.ChangeState(Bow.State.Idle);
	}

	private void SkillUndo(Player.State state)
	{
		stateMachine.ChangeState(Bow.State.Idle);
	}
}
