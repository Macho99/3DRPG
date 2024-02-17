using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Sword : Weapon
{
	[SerializeField] Transform trailTrans;

	private struct FrameInfo
	{
		public Vector3 position;
		public Quaternion rotation;

		public void Set(Vector3 pos, Quaternion rot)
		{
			this.position = pos;
			this.rotation = rot;
		}
	}

	List<GameObject> hitList;
	private RaycastHit[] hits;
	private BoxCollider col;
	private FrameInfo prev;
	private FrameInfo cur;

	protected override void Awake()
	{
		base.Awake();
		hitList = new List<GameObject>(20);
		hits = new RaycastHit[10];
		col = GetComponentInChildren<BoxCollider>(true);
	}

	public TargetFollower BeginAttack()
	{
		TargetFollower trail = GameManager.Resource.Instantiate<TargetFollower>("Prefab/SwordTrail", true);
		trail.SetTarget(trailTrans);
		trail.transform.position = trailTrans.position;
		trail.GetComponent<TrailRenderer>().Clear();
		prev.Set(col.transform.position + col.transform.rotation * col.center, col.transform.rotation);
		return trail;
	}

	public bool Attack()
	{
		cur.Set(col.transform.position + col.transform.rotation * col.center, col.transform.rotation);
		float moveDist = Vector3.Distance(prev.position, cur.position);

		ExtDebug.DrawBoxCastBox(
			cur.position,
			col.size * 0.5f,
			cur.rotation,
			prev.position - cur.position,
			moveDist,
			Color.red
			);

		int hitNum = Physics.BoxCastNonAlloc(
			cur.position,
			col.size * 0.5f,
			(prev.position - cur.position).normalized,
			hits,
			cur.rotation,
			moveDist,
			hitMask
			);

		prev = cur;

		for(int i = hitNum - 1; i >= 0; i--)
		{
			RaycastHit hit = hits[i];
			if (hitList.Contains(hit.collider.gameObject) == false)
			{
				if ((monsterMask.value & (1 << hit.collider.gameObject.layer)) == 0)
				{
					return false;
				}
				hitList.Add(hit.collider.gameObject);
				print(hit.collider.gameObject);
			}
		}
		return true;
	}
}