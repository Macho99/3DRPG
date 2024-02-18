using System;
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

	private GameObject[] hitList;
	private int hitListCnt;
	private RaycastHit[] hits;
	private BoxCollider col;
	private FrameInfo prev;
	private FrameInfo cur;

	protected override void Awake()
	{
		base.Awake();
		hitList = new GameObject[20];
		hitListCnt = 0;
		hits = new RaycastHit[10];
		col = GetComponentInChildren<BoxCollider>(true);
	}

	public TargetFollower BeginAttack()
	{
		hitListCnt = 0;
		TargetFollower trail = GameManager.Resource.Instantiate<TargetFollower>("Prefab/SwordTrail", true);
		trail.SetTarget(trailTrans);
		trail.transform.position = trailTrans.position;
		prev.Set(col.transform.position + col.transform.rotation * col.center, col.transform.rotation);
		return trail;
	}

	public bool Attack()
	{
		cur.Set(col.transform.position + col.transform.rotation * col.center, col.transform.rotation);
		float moveDist = Vector3.Distance(prev.position, cur.position);
		moveDist *= 1.2f;

		ExtDebug.DrawBoxCastBox(
			cur.position,
			col.size * 0.5f,
			cur.rotation,
			prev.position - cur.position,
			moveDist,
			Color.red
			);

		Debug.DrawLine(prev.position, cur.position);

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

			bool findResult = false;
			for(int cnt = 0; cnt < hitListCnt; cnt++)
			{
				if(hitList[cnt] == hit.collider.gameObject)
				{
					findResult = true;
					break;
				}
			}

			if (findResult == false)
			{
				if ((monsterMask.value & (1 << hit.collider.gameObject.layer)) == 0)
				{
					if (hit.point.y - player.transform.position.y > 0.5f)
					{
						//print(hit.point.y - player.transform.position.y);
						return false;
					}
					else
						continue;
				}
				hitList[hitListCnt] = hit.collider.gameObject;
				hitListCnt++;
				print(hit.collider.gameObject);
			}
		}
		return true;
	}
}