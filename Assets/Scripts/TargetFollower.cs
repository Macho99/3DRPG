using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollower : MonoBehaviour
{
	[SerializeField] public Transform target;
	[SerializeField] bool followRotation = true;
	[SerializeField] bool followScale = false;
	[SerializeField] Vector3 positionOffset;

	private void Update()
	{
		if (target == null) return;

        transform.position = target.position + positionOffset;

		if(followRotation == true)
			transform.rotation = target.rotation;

		if (followScale == true)
			transform.localScale = target.localScale;
	}

	public void SetTarget(Transform target)
	{
		this.target = target;
	}
}
