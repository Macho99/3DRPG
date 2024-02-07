using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PlayerMove : MonoBehaviour
{
	[SerializeField] Transform moveRoot;
	[SerializeField] float moveSpeed = 3f;
	[SerializeField] float jumpForce = 10f;
	[SerializeField] float slideSpeed = 5f;
	[SerializeField] float slideAngle = 20f;
	[SerializeField] float moveLerpSpeed = 50f;
	[SerializeField] float rotationLerpSpeed = 5f;

	private Vector3 curMoveVec;
	private bool isGround;
	public bool IsGround
	{
		get { return isGround; }
		private set
		{
			if (isGround == false && value == true)
			{
				anim.SetBool("IsJump", false);
			}
			else if (isGround == true && value == false)
			{

			}

			isGround = value;
		}
	}
	public bool IsSprint { get; private set; }

	Animator anim;
	CharacterController controller;
	Vector2 moveInput;
	float velY;
	bool colResult;
	RaycastHit hitInfo;
	LayerMask environmentMask;

	private void Awake()
	{
		velY = 0f;
		controller = GetComponent<CharacterController>();
		anim = GetComponentInChildren<Animator>();
		environmentMask = LayerMask.GetMask("Environment");
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
			if (moveInput.sqrMagnitude < 0.1f)
			{
				Vector3 slideDirection = hitInfo.normal;
				slideDirection.y = 0f;
				controller.Move(slideDirection * slideSpeed * Time.deltaTime);
			}
		}
	}

	private void GroundCheck()
	{
		colResult = Physics.SphereCast(transform.position + controller.center, controller.radius,
			Vector3.down, out hitInfo, controller.center.y + 0.1f - controller.radius, environmentMask);

		IsGround = colResult;
	}

	private void Move()
	{
		if (true == IsGround)
			velY = -2f;
		else
		{
			velY += Physics.gravity.y * Time.deltaTime;
		}

		Vector3 targetMoveVec = new Vector3();
		targetMoveVec += moveInput.x * moveSpeed * moveRoot.right;
		targetMoveVec += moveInput.y * moveSpeed * moveRoot.forward;
		float animSpeed = moveInput.sqrMagnitude;

		if (IsSprint)
		{
			targetMoveVec *= 2f;
			animSpeed *= 2f;
		}

		curMoveVec = Vector3.Lerp(curMoveVec, targetMoveVec, Time.deltaTime * moveLerpSpeed);

		if (moveInput.sqrMagnitude > 0.1f)
		{
			anim.transform.rotation = Quaternion.Lerp(
				anim.transform.rotation,
				Quaternion.LookRotation(new Vector3(curMoveVec.x, 0f, curMoveVec.z)), 
				Time.deltaTime * rotationLerpSpeed);
		}

		anim.SetFloat("Speed", animSpeed, 0.1f, Time.deltaTime);

		curMoveVec.y = velY;

		controller.Move(curMoveVec * Time.deltaTime);
	}

	private void OnMove(InputValue value)
	{
		moveInput = value.Get<Vector2>();
	}

	private void OnJump(InputValue value)
	{
		if (false == IsGround) { return; }

		IsGround = false;
		velY = jumpForce;
		anim.SetBool("IsJump", true);
	}

	private void OnSprint(InputValue value)
	{
		IsSprint = value.Get<float>() > 0.9f ? true : false;
	}

	private void OnDrawGizmos()
	{
		if (colResult == false) return;

		Gizmos.color = Color.yellow;

		Vector3 colPoint = hitInfo.point;
		colPoint.y += controller.radius;

		Gizmos.DrawWireSphere(colPoint, controller.radius);
	}
}