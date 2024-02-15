using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollower : MonoBehaviour
{
	[SerializeField] public Transform target;
	[SerializeField] bool followRotation;

	private void Update()
	{
		if (target == null) return;

        transform.position = target.position;
		if(followRotation == true)
		{
			transform.rotation = target.rotation;
		}
	}

	public void SetTarget(Transform target)
	{
		print(target);
		this.target = target;
	}
}
