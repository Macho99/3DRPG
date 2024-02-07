using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollower : MonoBehaviour
{
	[SerializeField] Transform target;
	[SerializeField] bool followRotation;

	private void Update()
	{
		transform.position = target.position;
		if(followRotation == true)
		{
			transform.rotation = target.rotation;
		}
	}
}
