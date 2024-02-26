using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PlayerMove : MonoBehaviour
{
	[SerializeField] Transform moveRoot;
	[SerializeField] float moveSpeed = 3f;
	[SerializeField] float jumpForce = 4f;
	[SerializeField] float doubleJumpForce = 6f;
	[SerializeField] float airAttackJumpForce = 3f;
	[SerializeField] float slideSpeed = 5f;
	[SerializeField] float slideAngle = 20f;
	[SerializeField] float moveLerpSpeed = 50f;
	[SerializeField] float rotationLerpSpeed = 5f;
	[SerializeField] float gravity = -25f;
	[SerializeField] float fallCheckDist = 2f;

	private Vector3 curMoveVec;
	private bool isGround;

	public Transform MoveRoot { get { return moveRoot; } }
	public bool IsGround
	{
		get { return isGround; }
		private set
		{
			isGround = value;
		}
	}

	[HideInInspector] public UnityEvent OnJumpDown = new UnityEvent();
	[HideInInspector] public UnityEvent OnDodgeDown = new UnityEvent();
	[HideInInspector] public UnityEvent OnFalling = new UnityEvent();

	public float ScaledGravity { get => gravity * GravityScale; }
	public bool JumpInput { get; private set; }
	public bool DodgeInput { get; private set; }
	public Vector2 MoveInput { get; private set; }
	public bool SprintInput { get; private set; }
	public float GravityScale { get; set; } = 1f;
	public float MoveMultiplier { private get; set; } = 1f;
	public float VelY { get { return velY; } }
	public Vector3 MoveForward { get { return moveRoot.forward; } }

	Animator anim;
	Transform animTrans;
	CharacterController controller;
	Transform characterTrans;
	[SerializeField] float velY;
	bool colResult;
	RaycastHit hitInfo;
	LayerMask environmentMask;

	private void Awake()
	{
		velY = 0f;
		controller = GetComponentInChildren<CharacterController>();
		anim = GetComponentInChildren<Animator>();
		animTrans = anim.transform;
		environmentMask = LayerMask.GetMask("Environment", "Tree", "Monster");
		isGround = true;
	}

	private void Update()
	{
		Move();
		GroundCheck();
		Slide();
	}

	private void Slide()
	{
		//바닥과 충돌중이고, 경사면이고, 플레이어 인풋이 없을때
		if (colResult == true && Vector3.Angle(hitInfo.normal, Vector3.up) > slideAngle)
		{
			if (MoveInput.sqrMagnitude < 0.1f)
			{
				Vector3 slideDirection = hitInfo.normal;
				slideDirection.y = 0f;
				controller.Move(slideDirection * slideSpeed * Time.deltaTime);
			}
		}
	}

	private void GroundCheck()
	{
		if (velY > 0.1f) return;

		colResult = Physics.SphereCast(transform.position + controller.center, controller.radius,
			Vector3.down, out hitInfo, controller.center.y + 0.1f - controller.radius, environmentMask);

		if(isGround == true && colResult == false)
		{
			IsGround = colResult;
			_ = StartCoroutine(CoFallCheck());
		}
		IsGround = colResult;
	}

	private IEnumerator CoFallCheck()
	{
		while(IsGround == false)
		{
			if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 100f, environmentMask) == true)
			{
				if ((hitInfo.point - transform.position).sqrMagnitude < fallCheckDist * fallCheckDist)
				{
					yield return null;
					continue;
				}
			}
			OnFalling?.Invoke();
			break;
		}
	}

	private void Move()
	{
		if (true == IsGround)
			velY = -2f;
		else
		{
			velY += ScaledGravity * Time.deltaTime;
		}

		Vector3 targetMoveVec = new Vector3();
		targetMoveVec += MoveInput.x * moveSpeed * moveRoot.right;
		targetMoveVec += MoveInput.y * moveSpeed * moveRoot.forward;
		Vector2 animSpeedVec = MoveInput;

		//if (IsSprint)
		//{
		//	targetMoveVec *= 2f;
		//	animSpeed *= 2f;
		//}

		targetMoveVec *= MoveMultiplier;

		curMoveVec = Vector3.Lerp(curMoveVec, targetMoveVec, Time.deltaTime * moveLerpSpeed);

		//에임 고정 아닐 때
		if (true)
		{
			float animSpeed = animSpeedVec.sqrMagnitude * MoveMultiplier;
			anim.SetFloat("Speed", animSpeed, 0.1f, Time.deltaTime);
			anim.SetFloat("SpeedY", animSpeed, 0.1f, Time.deltaTime);

			if (MoveInput.sqrMagnitude * MoveMultiplier > 0.1f)
			{
				anim.transform.rotation = Quaternion.Lerp(
					anim.transform.rotation,
					Quaternion.LookRotation(new Vector3(curMoveVec.x, 0f, curMoveVec.z)),
					Time.deltaTime * rotationLerpSpeed);
			}
		}
		//else
		//{

		//}
		curMoveVec.y = velY;

		controller.Move(curMoveVec * Time.deltaTime);
	}

	public void ManualMove(Vector3 moveVec, float time)
	{
		controller.Move(moveVec * time);
	}

	private void OnMove(InputValue value)
	{
		MoveInput = value.Get<Vector2>();
	}

	private void OnJump(InputValue value)
	{
		JumpInput = value.Get<float>() > 0.9f;
		if(JumpInput == true)
		{
			OnJumpDown?.Invoke();
		}
	}

	private void OnDodge(InputValue value)
	{
		DodgeInput = value.Get<float>() > 0.9f;
		if(DodgeInput == true)
		{
			OnDodgeDown?.Invoke();
		}
	}

	private void OnSprint(InputValue value)
	{
		SprintInput = value.Get<float>() > 0.9f;
	}

	private void OnDrawGizmos()
	{
		if (colResult == false) return;

		Gizmos.color = Color.yellow;

		Vector3 colPoint = hitInfo.point;
		colPoint.y += controller.radius;

		Gizmos.DrawWireSphere(colPoint, controller.radius);
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

	public void Jump(bool doubleJump = false)
	{
		if(doubleJump == false)
		{
			velY = jumpForce;
			IsGround = false;
		}
		else
		{
			velY = doubleJumpForce;
		}
	}

	public void OnAirAttackJump()
	{
		velY = airAttackJumpForce;
	}

	public float CalcLandTime()
	{
		Vector3 offset = Vector3.up * 0.1f;
		if (Physics.SphereCast(transform.position + offset, controller.radius,
			Vector3.down, out hitInfo, 100f, environmentMask) == false)
		{
			return 10f;
		}

		float dist = Vector3.Distance(hitInfo.point, transform.position + offset);
		float gd2 = 2 * -ScaledGravity * dist;
		float sqrt = Mathf.Sqrt(velY * velY + gd2);
		float result = (velY + sqrt) / -ScaledGravity;
		//print(result);
		return result;
	}
}