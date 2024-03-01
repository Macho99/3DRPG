using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	[SerializeField] ParticleSystem normalTrail;
	[SerializeField] ParticleSystem windTrail;
	[SerializeField] ParticleSystem fireTrail;
	private ParticleSystem curTrail;
	private LayerMask hitMask;
	private float despawnTime;
	private int hitCnt;

	public Vector3 Velocity { get; set; }

	Action<RaycastHit, Vector3, Vector3, int, Arrow> hitAction;
	Action<Arrow> updateAction;

	private void Awake()
	{
		hitMask = LayerMask.GetMask("Environment", "Monster", "Tree");
	}

	private void OnDisable()
	{
		normalTrail.gameObject.SetActive(false);
		windTrail.gameObject.SetActive(false);
		fireTrail.gameObject.SetActive(false);
	}

	public void Init(Vector3 velocity, Bow.ArrowState arrowState, 
		Action<Arrow> updateAction = null,
		Action<RaycastHit, Vector3, Vector3, int, Arrow> hitAction = null,
		int hitCnt = 0
		)
	{
		this.hitCnt = hitCnt;
		transform.forward = velocity;
		if(hitAction != null)
		{
			this.hitAction = hitAction;
		}
		else
		{
			this.hitAction = BasicHit;
		}
		this.updateAction = updateAction;

		despawnTime = Time.time + 10f;

		switch (arrowState) {
			case Bow.ArrowState.None:
				curTrail = null;
				break;
			case Bow.ArrowState.Normal:
				curTrail = normalTrail;
				break;
			case Bow.ArrowState.Wind:
				curTrail = windTrail;
				break;
			case Bow.ArrowState.Fire:
				curTrail = fireTrail;
				break;
		}


		curTrail.gameObject.SetActive(true);
		if (hitCnt == 0 || hitCnt == 1)
			curTrail.Play();

		_ = StartCoroutine(CoMove(velocity));
	}

	private IEnumerator CoMove(Vector3 vel)
	{
		//Vector3 gravity = Physics.gravity;
		Vector3 gravity = new Vector3(0f, -9.81f, 0f);
		Velocity = vel;
		while (true)
		{
			if (Time.time > despawnTime)
			{
				_ = StartCoroutine(CoOff());
				break;
			}

			Velocity += gravity * Time.deltaTime;
			if (Physics.Raycast(transform.position, Velocity, out RaycastHit hitInfo,
				this.Velocity.magnitude * Time.deltaTime, hitMask))
			{
				hitAction?.Invoke(hitInfo, transform.position, Velocity, hitCnt + 1, this);
				break;
			}
			transform.Translate(Velocity * Time.deltaTime, Space.World);
			transform.forward = Velocity;
			updateAction?.Invoke(this);

			yield return null;
		}
	}

	public void BasicHit(RaycastHit hitInfo, Vector3 pos, Vector3 velocity, int hitCnt, Arrow arrow)
	{
		transform.position = hitInfo.point;
		_ = StartCoroutine(CoOff());
	}

	private IEnumerator CoOff()
	{
		while (Time.time < despawnTime)
			yield return null;

		curTrail.Stop();
		curTrail.gameObject.SetActive(false);
		GameManager.Resource.Destroy(gameObject);
	}
}