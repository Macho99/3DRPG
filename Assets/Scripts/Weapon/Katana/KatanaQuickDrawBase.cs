using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.VFX;

public abstract class KatanaQuickDrawBase : StateBase<Katana.State, Katana>
{
	PlayerAttack playerAttack;
	PlayerAnimEvent playerAnimEvent;
	Player player;
	bool dummyActive;
	string triggerName;

	public KatanaQuickDrawBase(Katana owner, StateMachine<Katana.State, Katana> stateMachine, string triggerName) : base(owner, stateMachine)
	{
		this.triggerName = triggerName;
	}

	public override void Enter()
	{
		dummyActive = true;
		player.ChangeState(Player.State.StandAttack);
		playerAttack.SetAnimTrigger(triggerName);
		playerAnimEvent.OnEquipChange.AddListener(EquipChange);
		playerAnimEvent.OnClockWiseAttack.AddListener(ClockWiseAttack);
		playerAnimEvent.OnCounterClockWiseAttack.AddListener(CounterClockWiseAttack);
	}

	public override void Exit()
	{
		playerAnimEvent.OnEquipChange.RemoveListener(EquipChange);
		playerAnimEvent.OnClockWiseAttack.RemoveListener(ClockWiseAttack);
		playerAnimEvent.OnCounterClockWiseAttack.RemoveListener(CounterClockWiseAttack);
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

	private void Attack(bool clockwise)
	{
		VisualEffect effect = GameManager.Resource.Instantiate<VisualEffect>("Prefab/SlashVFX", true);
		effect.transform.position = player.transform.position + player.transform.forward + Vector3.up;

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
		effect.transform.rotation = quaternion * zAxisRotation;
		effect.transform.localScale = Vector3.one * 2f;
	}
}