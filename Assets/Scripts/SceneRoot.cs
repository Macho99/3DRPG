using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneRoot : MonoBehaviour
{
	[SerializeField] string sceneName;
	[SerializeField] bool setOffOnAwake = true;

	private void Awake()
	{
		GameManager.Scene.RegisterScene(sceneName, gameObject);
		if (setOffOnAwake == true)
			gameObject.SetActive(false);
	}
}
