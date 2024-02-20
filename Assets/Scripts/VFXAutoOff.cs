using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXAutoOff : MonoBehaviour
{
	[SerializeField] float OffTime = 1f;

	private float curTime = 0f;

	private void OnEnable()
	{
		_ = StartCoroutine(CoOff());
	}

	private IEnumerator CoOff()
	{
		while(true)
		{
			if(curTime > OffTime)
			{
				GameManager.Resource.Destroy(gameObject);
			}
			curTime += Time.deltaTime;
			yield return null;
		}
	}

	private void OnDisable()
	{
		curTime = 0f;
		transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
		transform.localScale = Vector3.one;
	}
}