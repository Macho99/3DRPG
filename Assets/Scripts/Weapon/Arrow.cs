using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	[SerializeField] ParticleSystem trailParticle;
	[SerializeField] ParticleSystem hitParticlePrefab;
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

	public void Init(Vector3 velocity, int hitCnt = 0, 
		Action<RaycastHit, Vector3, Vector3, int, Arrow> hitAction = null, 
		Action<Arrow> updateAction = null)
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

		if (hitCnt == 0 || hitCnt == 1)
			trailParticle.Play();

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
				break;

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

	private void BasicHit(RaycastHit hitInfo, Vector3 pos, Vector3 velocity, int hitCnt, Arrow arrow)
	{
		transform.position = hitInfo.point;
		_ = StartCoroutine(CoOff());
	}

	private IEnumerator CoOff()
	{
		while (Time.time < despawnTime)
			yield return null;

		trailParticle.Stop();
		GameManager.Resource.Destroy(gameObject);
	}
}