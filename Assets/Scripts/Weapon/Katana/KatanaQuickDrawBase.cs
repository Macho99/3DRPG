using Autodesk.Fbx;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.VFX;

public abstract class KatanaQuickDrawBase : StateBase<Katana.State, Katana>
{
	protected PlayerAttack playerAttack;
	protected PlayerAnimEvent playerAnimEvent;
	protected Player player;
	protected string vfxPath;
	bool dummyActive;
	string triggerName;
	bool endQuickDraw;
	float scale;
	float radius;

	public KatanaQuickDrawBase(Katana owner, StateMachine<Katana.State, Katana> stateMachine
		, string triggerName, bool endQuickDraw, string vfxPath = "Prefab/SlashVFX", 
		float scale = 2f, float radius = 2f) : base(owner, stateMachine)
	{ 
		this.triggerName = triggerName;
		this.endQuickDraw = endQuickDraw;
		this.vfxPath = vfxPath;
		this.scale = scale;
		this.radius = radius;
	}

	public override void Enter()
	{
		dummyActive = true;
		player.ChangeState(Player.State.StandAttack);
		playerAttack.SetAnimTrigger(triggerName);
		playerAnimEvent.OnEquipChange.AddListener(EquipChange);
		playerAnimEvent.OnClockWiseAttack.AddListener(ClockWiseAttack);
		playerAnimEvent.OnCounterClockWiseAttack.AddListener(CounterClockWiseAttack);

		if(endQuickDraw == true)
		{
			playerAnimEvent.OnAttackEnd.AddListener(AttackEnd);
		}
	}

	public override void Exit()
	{
		playerAnimEvent.OnEquipChange.RemoveListener(EquipChange);
		playerAnimEvent.OnClockWiseAttack.RemoveListener(ClockWiseAttack);
		playerAnimEvent.OnCounterClockWiseAttack.RemoveListener(CounterClockWiseAttack);
		if(endQuickDraw == true)
		{
			playerAnimEvent.OnAttackEnd.RemoveListener(AttackEnd);
		}
	}

	public override void Update()
	{

	}

	public override void Setup()
	{
		player = owner.Player;
		playerAttack = owner.PlayerAttack;
		playerAnimEvent = owner.PlayerAnimEvent;
	}

	public override void Transition()
	{
	}

	private void EquipChange()
	{
		dummyActive = !dummyActive;
		owner.SetDummyRender(dummyActive);
		if(dummyActive == true)
		{
			stateMachine.ChangeState(Katana.State.QuickDrawIdle);
		}
	}

	private void ClockWiseAttack()
	{
		Attack(true);
	}

	private void CounterClockWiseAttack()
	{
		Attack(false);
	}

	protected virtual void VFXSetting(GameObject vfx) { }

	protected virtual void Attack(bool clockwise)
	{
		GameObject vfx = GameManager.Resource.Instantiate<GameObject>(vfxPath, true);
		VFXSetting(vfx);
		vfx.transform.position = player.transform.position + player.transform.forward + Vector3.up;

		Quaternion quaternion = player.transform.rotation;
		quaternion *= Quaternion.Euler(0f, Random.Range(-45f, 45f), 0f);
		Quaternion zAxisRotation;
		float rotAngle = Random.Range(0f, 30f);
		if(clockwise == false)
		{
			rotAngle = -rotAngle;
			rotAngle += 180f;
		}

		zAxisRotation = Quaternion.Euler(0f, 0f, rotAngle);
		vfx.transform.rotation = quaternion * zAxisRotation;
		vfx.transform.localScale = Vector3.one * scale;

		Vector3 position = owner.transform.position + owner.transform.forward * radius;
		owner.SphereCastAttack(position, radius, owner.FinalDamage);
	}

	protected virtual void AttackEnd()
	{
		stateMachine.ChangeState(Katana.State.Idle);
		playerAttack.SetAnimTrigger("BaseExit");
	}
}