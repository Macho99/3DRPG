using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class FloatingArrow : MonoBehaviour
{
	//[SerializeField] float flipDuration = 2f;
	[SerializeField] float dissolveSpeed = 5f;
	[SerializeField] float flightArrowSpeed = 15f;
	[SerializeField] List<Material> dissolveMatList;
	[SerializeField] List<Material> originMatList;

	FlightArrow flightArrow;
	ParticleSystem flightParticle;

	Quaternion upOffset = Quaternion.Euler(90f, 0f, 0f);
	Player player;
	Transform aimPoint;

	Transform target;
	new MeshRenderer renderer;
	MeshFilter meshFilter;
	float threshold = 0f;

	bool prepareAttack = false;
	bool attackLock = false;

	private void Awake()
	{
		renderer = GetComponentInChildren<MeshRenderer>();
		meshFilter = GetComponentInChildren<MeshFilter>();
		player = FieldSFC.Player;
		aimPoint = player.PlayerLook.AimPoint;
	}

	private void OnDisable()
	{
		if(flightArrow != null)
		{
			GameManager.Resource.Destroy(flightArrow.gameObject);
		}
		renderer.enabled = true;
		transform.rotation = Quaternion.identity;
	}

	public void Init()
	{
		_ = StartCoroutine(CoDissolve());
	}

	public void FlightInit()
	{
		renderer.SetMaterials(originMatList);
	}

	public void PrepareAttack()
	{
		prepareAttack = true;
	}

	private IEnumerator CoDissolve()
	{
		threshold = 0f;
		transform.rotation = Quaternion.identity;
		renderer.SetMaterials(dissolveMatList);
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

		_ = StartCoroutine(CoIdle());
	}

	private IEnumerator CoIdle()
	{
		attackLock = false;
		float rotationSpeed = Random.Range(-50f, -200f);
		while (prepareAttack == false)
		{
			transform.rotation = Quaternion.Euler(0f, rotationSpeed * Time.deltaTime, 0f) * transform.rotation;
			yield return null;
		}
		_ = StartCoroutine(CoAlign());
	}

	//private IEnumerator CoFlip()
	//{
	//	float elapsed = 0f;
	//	float deltaRotation = 360f / flipDuration;
	//	while (elapsed < flipDuration - 0.05f)
	//	{
	//		elapsed += Time.deltaTime;
	//		transform.rotation = Quaternion.Euler(0f, 0f, Time.deltaTime * deltaRotation) * transform.rotation;
	//		yield return null;
	//	}
	//	transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
	//	_ = StartCoroutine(CoIdle());
	//}

	private IEnumerator CoAlign()
	{
		//Quaternion randomOffset = Quaternion.Euler(Random.Range(-45f, 0f), 0f, Random.Range(-45f, 45f));
		while (true)
		{
			Quaternion quaternion = Quaternion.LookRotation(aimPoint.position - player.transform.position, Vector3.up);
			transform.rotation = Quaternion.Lerp(transform.rotation, quaternion * upOffset, Time.deltaTime * 5f);
			yield return null;
		}
	}

	public void Attack(Transform target)
	{
		if (attackLock == true) return;

		attackLock = true;
		prepareAttack = false;
		StopAllCoroutines();
		this.target = target;
		_ = StartCoroutine(CoAttack());
	}

	private IEnumerator CoAttack()
	{
		float waitTime = Time.time + Random.Range(0f, 0.5f);

		while(Time.time < waitTime)
		{
			Quaternion quaternion = Quaternion.LookRotation(aimPoint.position - player.transform.position, Vector3.up);
			transform.rotation = Quaternion.Lerp(transform.rotation, quaternion * upOffset, Time.deltaTime * 5f);
			yield return null;
		}

		renderer.enabled = false;
		flightArrow = GameManager.Resource.Instantiate<FlightArrow>("Prefab/FlightArrow", 
			transform.position, transform.rotation, true);
		flightArrow.ParticlePlay();

		Vector3 curVel = transform.up;
		Vector3 xOffset = transform.right;
		xOffset *= Random.Range(-0.5f, 0.5f);
		Vector3 yOffset = -transform.forward;
		yOffset *= Random.Range(0f, 0.5f);
		curVel += xOffset + yOffset;

		float speed = flightArrowSpeed + Random.Range(-5f, 5f);
		curVel *= speed;

		float halfSqrMag = (transform.position - target.position).sqrMagnitude * (0.4f * 0.4f);
		while (true)
		{
			flightArrow.transform.position += curVel * Time.deltaTime;
			flightArrow.transform.up = curVel;

			if ((transform.position - flightArrow.transform.position).sqrMagnitude > halfSqrMag)
			{
				break;
			}

			yield return null;
		}

		float sqrMag;
		do
		{
			Vector3 targetVelocity = (target.position - flightArrow.transform.position).normalized * speed;
			curVel = Vector3.Lerp(curVel, targetVelocity, Time.deltaTime * 5f);
			flightArrow.transform.position = flightArrow.transform.position + curVel * Time.deltaTime;
			flightArrow.transform.up = curVel;
			sqrMag = (target.position - flightArrow.transform.position).sqrMagnitude;
			yield return null;
		} while (sqrMag > 0.1f);

		GameManager.Resource.Destroy(flightArrow.gameObject);
		flightArrow = null;

		renderer.enabled = true;
		_ = StartCoroutine(CoDissolve());
	}
}