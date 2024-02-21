using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSFC : MonoBehaviour
{
	private static FieldSFC instance;
	private static GameObject player;
	public static GameObject Player
	{
		get
		{
			if (player == null)
				player = GameObject.FindGameObjectWithTag("Player");
			return player;
		}
	}

	public static FieldSFC Instance
	{
		get { return instance; }
	}

	private void Awake()
	{
		if(instance != null)
		{
			Destroy(gameObject);
			return;
		}
		instance = this;
	}

	private void OnDestroy()
	{
		if(instance == this)
		{
			instance = null;
			player = null;
		}
	}
}
