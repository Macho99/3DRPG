using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Bow;
using Random = UnityEngine.Random;

public class BowUltiElem : MonoBehaviour
{
	[SerializeField] ParticleSystem iceVFX;
	[SerializeField] ParticleSystem windVFX;
	[SerializeField] ParticleSystem fireVFX;
	[SerializeField] List<Material> dissolveMatList;
	[SerializeField] List<Material> originMatList;
	[SerializeField] float duration = 10f;
	[SerializeField] float dissolveSpeed = 5f;

	ParticleSystem curVFX;
	float endTime;
	float threshold;
	new MeshRenderer renderer;
	Bow bow;
	Bow.ArrowProperty property;
	Action<RaycastHit, int, Arrow> hitAction;
	Action<Arrow> updateAction;

	private void Awake()
	{
		renderer = GetComponentInChildren<MeshRenderer>();
	}

	public void Init(Bow bow, Bow.ArrowProperty property)
	{
		endTime = Time.time + duration;
		renderer.gameObject.SetActive(false);
		this.bow = bow;
		this.property = property; 

		updateAction = null;
		hitAction = bow.MonsterHit;
		iceVFX.gameObject.SetActive(false);
		fireVFX.gameObject.SetActive(false);
		windVFX.gameObject.SetActive(false);

		switch (property)
		{
			case ArrowProperty.Ice:
				hitAction = bow.BounceHit;
				curVFX = iceVFX;
				break;
			case ArrowProperty.Fire:
				hitAction = bow.Explosion;
				curVFX = fireVFX;
				break;
			case ArrowProperty.Wind:
				updateAction = bow.TraceMonster;
				curVFX = windVFX;
				break;
			default:
				print($"올바르지 않은 ArrowProperty : {property}");
				break;
		}
		_ = StartCoroutine(CoInit());
	}

	private IEnumerator CoInit()
	{
		yield return new WaitForSeconds(Random.Range(0f, 1f));
		curVFX.gameObject.SetActive(true);
		yield return new WaitForSeconds(Random.Range(0f, 1f));
		_ = StartCoroutine(CoDissolve());
	}

	private IEnumerator CoDissolve()
	{
		if (Time.time > endTime - 1f)
		{
			yield return new WaitForSeconds(1f);
			GameManager.Resource.Destroy(gameObject);
			yield break;
		}
		renderer.transform.localPosition = new Vector3(0f, 0f, -0.6f);
		threshold = 0f;
		renderer.SetMaterials(dissolveMatList);
		renderer.gameObject.SetActive(true);

		while (threshold < 2.5f)
		{
			threshold += Time.deltaTime * dissolveSpeed;
			foreach (Material mat in renderer.materials)
			{
				mat.SetFloat("_threshold", threshold);
			}
			yield return null;
		}
		renderer.SetMaterials(originMatList);

		_ = StartCoroutine(CoLoadArrow());
	}

	private IEnumerator CoLoadArrow()
	{
		while (true)
		{
			Vector3 pos = renderer.transform.localPosition;
			pos.z += Time.deltaTime * (0.65f + pos.z) * 3f;
			renderer.transform.localPosition = pos;

			if(pos.z > -0.3f)
			{
				break;
			}

			yield return null;
		}
		_ = StartCoroutine(CoShot());
	}

	private IEnumerator CoShot()
	{
		renderer.gameObject.SetActive(false);
		Arrow arrow = GameManager.Resource.Instantiate<Arrow>("Prefab/Arrow", 
			transform.position, transform.rotation, true);
		Vector3 velocity = transform.forward * 20f * Random.Range(0.7f, 1.3f);
		velocity += Random.insideUnitSphere * 0.2f;
		arrow.Init(velocity, property, updateAction, hitAction);
		yield return new WaitForSeconds(0.5f);

		_ = StartCoroutine(CoDissolve());
	}
}
