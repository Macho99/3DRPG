using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeFeedback : MonoBehaviour
{
	[SerializeField] float[] roughnessArr;      //거칠기 정도
	[SerializeField] float[] magnitudeArr;      //움직임 범위
	[SerializeField] float[] zValueArr;			//z축 값
	[SerializeField] float xMultiplier = 1f;
	[SerializeField] float yMultiplier = 0.5f;
	[SerializeField] Transform CamFollower;

	GameObject particle;
	Transform chargeShakeCam;
	float zValue;
	float roughness;
	float magnitude;
	float tick;

	int chargeLevel = 1;

	bool playing;

	private void Awake()
	{
		chargeShakeCam = CamFollower.GetChild(0);
		SetChargeLevel(chargeLevel);
	}

	public void Play()
	{
		Transform playerTrans = FieldSFC.Player.transform;
		particle = GameManager.Resource.Instantiate<GameObject>("Prefab/ChargeParticle", 
			playerTrans.position, playerTrans.rotation, true);
		playing = true;
		CamFollower.gameObject.SetActive(true);
		chargeShakeCam.transform.localPosition = Vector3.zero;
		zValue = 0f;
		chargeShakeCam.transform.localRotation = Quaternion.identity;
		tick = Random.Range(-10f, 10f);
		_ = StartCoroutine(CoPlay());
	}

	public void Stop()
	{
		GameManager.Resource.Destroy(particle);
		particle = null;
		playing = false;
		CamFollower.gameObject.SetActive(false);
	}

	private IEnumerator CoPlay()
	{
		while(playing == true)
		{
			chargeShakeCam.transform.localRotation = Quaternion.identity;
			float curZ = chargeShakeCam.transform.localPosition.z;
			float lerpZ = Mathf.Lerp(curZ, zValue, Time.deltaTime * 5f);

			tick += Time.deltaTime * roughness;
			chargeShakeCam.transform.localPosition = new Vector3(
				(Mathf.PerlinNoise(tick, 0) - .5f) * xMultiplier * magnitude,
				(Mathf.PerlinNoise(0, tick) - .5f) * yMultiplier * magnitude,
				lerpZ);
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		chargeShakeCam.localPosition = Vector3.zero;
	}

	public void SetChargeLevel(int level)
	{
		if(level < 1 || level > 3)
		{
			print($"차지레벨은 1~3이어야 합니다 : {level}");
			return;
		}
		chargeLevel = level;
		roughness = roughnessArr[chargeLevel - 1];
		magnitude = magnitudeArr[chargeLevel - 1];
		zValue = zValueArr[chargeLevel - 1];
	}
}
