using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.LookDev;

public class PlayerAttack : MonoBehaviour
{
	[SerializeField] Transform weaponHolder;
	[SerializeField] float attackHoldTime = 0.3f;
	[SerializeField] MMF_Player attackFailFeedback;

	[HideInInspector] public UnityEvent<Player.State> OnAttack1Down;
	[HideInInspector] public UnityEvent<Player.State> OnAttack1Up;
	[HideInInspector] public UnityEvent<Player.State> OnAttack1Hold;
	[HideInInspector] public UnityEvent<Player.State> OnAttack2Down;
	[HideInInspector] public UnityEvent<Player.State> OnAttack2Up;
	[HideInInspector] public UnityEvent<Player.State> OnAttack2Hold;
	[HideInInspector] public UnityEvent<Player.State> OnQButtonDown;
	[HideInInspector] public UnityEvent<Player.State> OnQButtonUp;
	[HideInInspector] public UnityEvent<Player.State> OnEButtonDown;
	[HideInInspector] public UnityEvent<Player.State> OnEButtonUp;
	[HideInInspector] public UnityEvent<Player.State> OnRButtonDown;
	[HideInInspector] public UnityEvent<Player.State> OnRButtonUp;

	[HideInInspector] public UnityEvent<WeaponType> OnCurHoldWeaponTypeChange;

	private Coroutine Attack1HoldCoroutine;
	private Coroutine Attack2HoldCoroutine;

	public bool Attack1Pressed { get; private set; }
	public bool Attack2Pressed { get; private set; }
	public PlayerAnimEvent AnimEvent { get => animEvent; }

	private RuntimeAnimatorController initController;
	private Player player;
	private PlayerAnimEvent animEvent;
	private Weapon[] weapons;
	private WeaponType curHoldWeaponType = WeaponType.Melee;
	private Animator anim;

	private void Awake()
	{
		player = GetComponent<Player>();
		animEvent = GetComponent<PlayerAnimEvent>();
		anim = GetComponent<Animator>();
		initController = anim.runtimeAnimatorController;
		weapons = new Weapon[(int)WeaponType.Size];

		OnAttack1Down = new UnityEvent<Player.State>();
		OnAttack1Up = new UnityEvent<Player.State>();
		OnAttack1Hold = new UnityEvent<Player.State>();
		OnAttack2Down = new UnityEvent<Player.State>();
		OnAttack2Up = new UnityEvent<Player.State>();
		OnAttack2Hold = new UnityEvent<Player.State>();
		OnQButtonDown = new UnityEvent<Player.State>();
		OnQButtonUp = new UnityEvent<Player.State>();
		OnEButtonDown = new UnityEvent<Player.State>();
		OnEButtonUp = new UnityEvent<Player.State>();
		OnRButtonDown = new UnityEvent<Player.State>();
		OnRButtonUp = new UnityEvent<Player.State>();

		OnCurHoldWeaponTypeChange = new UnityEvent<WeaponType>();

		//if (weapons.Length > 0)
		//{
		//	curWeapon = weapons[0];
		//	anim.runtimeAnimatorController = curWeapon.GetAnimController();
		//}
	}

	public void RefreshWeapon()
	{
		WeaponItem InvMeleeItem = GameManager.Inven.GetWeaponSlot(WeaponType.Melee);
		WeaponItem InvRangedItem = GameManager.Inven.GetWeaponSlot(WeaponType.Ranged);

		int meleeIdx = (int) WeaponType.Melee;
		int rangedIdx = (int) WeaponType.Ranged;

		WeaponItem curMeleeItem = weapons[meleeIdx]?.WeaponItem;
		WeaponItem curRangedItem = weapons[rangedIdx]?.WeaponItem;
		if (curMeleeItem != InvMeleeItem)
		{
			ChangeWeapon(meleeIdx, InvMeleeItem);
		}

		if (curRangedItem != InvRangedItem)
		{
			ChangeWeapon(rangedIdx, InvRangedItem);
		}
	}

	private void ChangeWeapon(int idx, WeaponItem newWeaponItem)
	{
		if (weapons[idx] != null)
		{
			if (idx == (int)curHoldWeaponType)
			{
				weapons[idx].ForceInactive();
				SetAnimFloat("Armed", 0f);
			}

			GameManager.Resource.Destroy(weapons[idx].gameObject);
			weapons[idx] = null;
		}

		if(newWeaponItem != null)
		{
			weapons[idx] = GameManager.Resource.Instantiate(newWeaponItem.WeaponPrefab, weaponHolder, false);
			weapons[idx].transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
			weapons[idx].Init(newWeaponItem);
			if (idx != (int)curHoldWeaponType)
			{
				weapons[idx].ForceInactive();
				weapons[idx].gameObject.SetActive(false);
			}
			else
			{
				anim.runtimeAnimatorController = weapons[idx].GetAnimController();
			}
		}
		else if(idx == (int)curHoldWeaponType)
		{
			anim.runtimeAnimatorController = initController;
		}
	}

	private void WeaponSwap(WeaponType newWeaponType)
	{
		if (curHoldWeaponType == newWeaponType) return;
		switch (player.CurState)
		{
			case Player.State.Idle:
			case Player.State.Walk:
			case Player.State.Run:
				break;
			default:
				return;
		}

		int curIdx = (int) curHoldWeaponType;
		int newIdx = (int) newWeaponType;

		if (weapons[curIdx] != null)
		{
			anim.SetTrigger("BaseExit");
			weapons[curIdx].ChangeStateToIdle(false);
			weapons[curIdx].ForceInactive();
			weapons[curIdx].gameObject.SetActive(false);
		}

		if (weapons[newIdx] != null)
		{
			anim.runtimeAnimatorController = weapons[newIdx].GetAnimController();
			weapons[newIdx].gameObject.SetActive(true);
		}
		else
		{
			anim.runtimeAnimatorController = initController;
		}

		curHoldWeaponType = newWeaponType;
	}

	private void OnNum1Button(InputValue value)
	{
		if (value.isPressed == false) return;

		WeaponSwap(WeaponType.Melee);
	}

	private void OnNum2Button(InputValue value)
	{
		if (value.isPressed == false) return;

		WeaponSwap(WeaponType.Ranged);
	}

	private void OnNum3Button(InputValue value)
	{
		if (value.isPressed == false) return;

		GameManager.Inven.GetConsumpSlot(ConsumpSlotType.Slot1)?.Use();
	}
	private void OnNum4Button(InputValue value)
	{
		if (value.isPressed == false) return;

		GameManager.Inven.GetConsumpSlot(ConsumpSlotType.Slot2)?.Use();
	}


	private void OnAttack1(InputValue value)
	{
		Attack1Pressed = value.Get<float>() > 0.9f ? true : false;

		if (Attack1Pressed)
		{
			OnAttack1Down?.Invoke(player.CurState);
			Attack1HoldCoroutine = StartCoroutine(CoAttackHold(OnAttack1Hold));
		}
		else
		{
			if(Attack1HoldCoroutine != null)
				StopCoroutine(Attack1HoldCoroutine);
			OnAttack1Up?.Invoke(player.CurState);
		}
	}

	private void OnAttack2(InputValue value)
	{
		Attack2Pressed = value.Get<float>() > 0.9f ? true : false;

		if (Attack2Pressed)
		{
			OnAttack2Down?.Invoke(player.CurState);
			Attack2HoldCoroutine = StartCoroutine(CoAttackHold(OnAttack2Hold));
		}
		else
		{
			if (Attack2HoldCoroutine != null)
				StopCoroutine(Attack2HoldCoroutine);
			OnAttack2Up?.Invoke(player.CurState);
		}
	}

	private IEnumerator CoAttackHold(UnityEvent<Player.State> holdevent)
	{
		float exitTime = Time.time + attackHoldTime;
		while (Time.time < exitTime)
		{
			yield return null;
		}
		holdevent?.Invoke(player.CurState);
	}

	private void OnQButton(InputValue value)
	{
		bool pressed = value.Get<float>() > 0.9f ? true : false;

		if (pressed)
		{
			OnQButtonDown?.Invoke(player.CurState);
		}
		else
		{
			OnQButtonUp?.Invoke(player.CurState);
		}
	}
	private void OnEButton(InputValue value)
	{
		bool pressed = value.Get<float>() > 0.9f ? true : false;

		if (pressed)
		{
			OnEButtonDown?.Invoke(player.CurState);
		}
		else
		{
			OnEButtonUp?.Invoke(player.CurState);
		}
	}
	private void OnRButton(InputValue value)
	{
		bool pressed = value.Get<float>() > 0.9f ? true : false;

		if (pressed)
		{
			OnRButtonDown?.Invoke(player.CurState);
		}
		else
		{
			OnRButtonUp?.Invoke(player.CurState);
		}
	}

	public void ChangeStateToIdle(bool forceIdle = false)
	{
		weapons[(int)curHoldWeaponType]?.ChangeStateToIdle(forceIdle);
	}

	public float GetAnimNormalizedTime(int layer)
	{
		return anim.GetCurrentAnimatorStateInfo(layer).normalizedTime;
	}

	public bool IsAnimWait(int layer)
	{
		return anim.GetCurrentAnimatorStateInfo(layer).IsName("Wait");
	}

	public bool IsAnimName(int layer, string name)
	{
		return anim.GetCurrentAnimatorStateInfo(layer).IsName(name);
	}

	public void SetAnimTrigger(string str)
	{
		//print(str);
		anim.SetTrigger(str);
	}

	public void SetAnimFloat(string str, float value)
	{
		anim.SetFloat(str, value);
	}

	public void SetAnimFloat(string str, float value, float dampTime, float deltaTime)
	{
		anim.SetFloat(str, value, dampTime, deltaTime);
	}

	public void SetAnimBool(string str, bool value)
	{ 
		anim.SetBool(str, value);
	}

	public void SetAnimUpdateMode(AnimatorUpdateMode mode)
	{
		anim.updateMode = mode;
	}

	private void OnUnarm(InputValue value)
	{
		if(value.isPressed == true)
			SetUnarmed();
	}

	public void SetUnarmed()
	{
		weapons[(int) curHoldWeaponType]?.SetUnArmed();
	}

	public void PlayAttackFailFeedback()
	{
		attackFailFeedback?.PlayFeedbacks();
	}
}