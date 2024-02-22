using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAfterimage : MonoBehaviour
{
	[SerializeField] Transform skinFolder;
	[SerializeField] float refeshTime = 0.3f;
	[SerializeField] float duration = 2f;
	[SerializeField] Material material;

	private float parentOffset = 0.1f;
	private SkinnedMeshRenderer[] skinneds;

	private void Awake()
	{
		skinneds = GetComponentsInChildren<SkinnedMeshRenderer>();
	}

	public void Play()
	{
		_ = StartCoroutine(CoBakeMesh());
	}

	private IEnumerator CoBakeMesh()
	{
		float endTime = Time.time + duration;
		while(Time.time < endTime)
		{
			float curTime = Time.time;
			VFXAutoOff afterimage = GameManager.Resource.Instantiate<VFXAutoOff>(
				"Prefab/Afterimage", transform.position, transform.rotation, true);
			float curRatio = (endTime - curTime) / duration;
			afterimage.SetOffTime(endTime - Time.time + parentOffset);

			foreach (var skin in skinneds)
			{
				VFXAutoOff child = GameManager.Resource.Instantiate<VFXAutoOff>(
					"Prefab/AfterimageChild", true);
				child.SetOffTime(endTime - Time.time);
				child.transform.SetParent(afterimage.transform, false);

				MeshFilter childMf = child.GetComponent<MeshFilter>();
				MeshRenderer childMr = childMf.GetComponent<MeshRenderer>();

				Mesh mesh = new Mesh();
				skin.BakeMesh(mesh);
				childMf.mesh = mesh;

				childMr.material = material;
				StartCoroutine(CoFade(childMr.material, curRatio, 0.1f ,0.1f));
			}

			yield return new WaitForSeconds(refeshTime);
		}
	}

	private IEnumerator CoFade(Material mat, float startAlpha,float subPerRefresh, float refreshInterval)
	{
		float alphaMultiplier = startAlpha;
		while (true)
		{
			mat.SetFloat("_AlphaMultiplier", alphaMultiplier);
			alphaMultiplier -= subPerRefresh;
			if (alphaMultiplier < 0.01f)
				break;
			yield return new WaitForSeconds(refreshInterval);
		}
	}
}
