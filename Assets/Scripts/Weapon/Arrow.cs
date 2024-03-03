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

	public Vector3 Gravity { get; private set; }
	public Vector3 Velocity { get; set; }

	Action<RaycastHit, int, Arrow> hitAction;
	Action<Arrow> updateAction;

	private void Awake()
	{
		Gravity = new Vector3(0, -9.81f, 0f);
		hitMask = LayerMask.GetMask("Environment", "Monster", "Tree");
	}

	private void OnDisable()
	{
		normalTrail.gameObject.SetActive(false);
		windTrail.gameObject.SetActive(false);
		fireTrail.gameObject.SetActive(false);
	}

	public void Init(Vector3 velocity, Bow.ArrowProperty arrowState, 
		Action<Arrow> updateAction = null,
		Action<RaycastHit, int, Arrow> hitAction = null,
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
			this.hitAction = GroundHit;
		}
		this.updateAction = updateAction;

		despawnTime = Time.time + 10f;

		switch (arrowState) {
			case Bow.ArrowProperty.None:
				curTrail = null;
				break;
			case Bow.ArrowProperty.Ice:
				curTrail = normalTrail;
				break;
			case Bow.ArrowProperty.Wind:
				curTrail = windTrail;
				break;
			case Bow.ArrowProperty.Fire:
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
		Velocity = vel;
		while (true)
		{
			if (Time.time > despawnTime)
			{
				_ = StartCoroutine(CoOff());
				break;
			}

			Velocity += Gravity * Time.deltaTime;
			if (Physics.Raycast(transform.position, Velocity, out RaycastHit hitInfo,
				this.Velocity.magnitude * Time.deltaTime, hitMask))
			{
				hitAction?.Invoke(hitInfo, hitCnt + 1, this);
				break;
			}
			transform.Translate(Velocity * Time.deltaTime, Space.World);
			transform.forward = Velocity;
			updateAction?.Invoke(this);

			yield return null;
		}
	}

	public void GroundHit(RaycastHit hitInfo, int hitCnt, Arrow arrow)
	{
		transform.position = hitInfo.point;
		AutoOff();
	}

	public void AutoOff()
	{
		_ = StartCoroutine(CoOff());
	}

	private IEnumerator CoOff()
	{
		yield return new WaitForSeconds(0.3f);
		curTrail.Stop();

		while (Time.time < despawnTime)
			yield return null;

		curTrail.gameObject.SetActive(false);
		GameManager.Resource.Destroy(gameObject);
	}
}