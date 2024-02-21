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

	private SkinnedMeshRenderer[] skinneds;

	private void Awake()
	{
		skinneds = GetComponentsInChildren<SkinnedMeshRenderer>();
	}

	private void OnEvasion(InputValue value)
	{
		if(value.isPressed == true)
		{
			StartCoroutine(CoBakeMesh());
		}
	}

	private IEnumerator CoBakeMesh()
	{
		float endTime = Time.time + duration;
		while(Time.time < endTime)
		{
			GameObject afterimage = new GameObject("Afterimage");
			afterimage.AddComponent<VFXAutoOff>();
			afterimage.transform.SetPositionAndRotation(transform.position, transform.rotation);

			foreach(var skin in skinneds)
			{
				GameObject child = new GameObject("Child");
				child.transform.SetParent(afterimage.transform, false);

				MeshFilter mf = child.AddComponent<MeshFilter>();
				MeshRenderer mr = child.AddComponent<MeshRenderer>();

				Mesh mesh = new Mesh();
				skin.BakeMesh(mesh);
				mf.mesh = mesh;

				mr.material = material;
				StartCoroutine(CoFade(mr.material, 0.1f ,0.1f));
			}

			yield return new WaitForSeconds(refeshTime);
		}
	}

	private IEnumerator CoFade(Material mat, float subPerRefresh, float refreshInterval)
	{
		float alphaMultiplier = 1f;
		while (true)
		{
			alphaMultiplier -= subPerRefresh;
			mat.SetFloat("_AlphaMultiplier", alphaMultiplier);
			if (alphaMultiplier < 0.01f)
				break;
			yield return new WaitForSeconds(refreshInterval);
		}
	}
}
