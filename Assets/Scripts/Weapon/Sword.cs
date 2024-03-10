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

	private BoxCollider col;
	private GameObject[] hitList;
	private int hitListCnt;
	private RaycastHit[] hits;
	private FrameInfo prev;
	private FrameInfo cur;
	private RaycastHitComparer comparer = new RaycastHitComparer();

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

		Vector3 castStartPos = prev.position + (prev.position - cur.position) * 0.2f;

		ExtDebug.DrawBoxCastBox(
			castStartPos,
			col.size * 0.5f,
			cur.rotation,
			cur.position - prev.position,
			moveDist,
			Color.red
			);

		Debug.DrawLine(castStartPos, cur.position);

		int hitNum = Physics.BoxCastNonAlloc(
			castStartPos,
			col.size * 0.5f,
			(cur.position - prev.position).normalized,
			hits,
			cur.rotation,
			moveDist,
			hitMask
			);

		prev = cur;
		comparer.Init(castStartPos);
		Array.Sort(hits, 0, hitNum, comparer);

		//print("Start");
		for(int i = 0; i < hitNum; i++)
		{
			RaycastHit hit = hits[i];

			//print(Vector3.Distance(hit.point, castStartPos));

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
						playerAttack.PlayAttackFailFeedback();
						return false;
					}
					else
						continue;
				}
				hitList[hitListCnt] = hit.collider.gameObject;
				hitListCnt++;

				// TODO: 검 데미지 임시부여 (데미지 수치 변경 필요)
				MonsterAttack(hit.collider.gameObject, FinalDamage);
			}
		}
		//print("End;");
		return true;
	}
}

public class RaycastHitComparer : IComparer<RaycastHit>
{
	private Vector3 castStartPos;

	public void Init(Vector3 castStartPos)
	{
		this.castStartPos = castStartPos;
	}

	public int Compare(RaycastHit a, RaycastHit b)
	{
		float distA = Vector3.SqrMagnitude(a.point - castStartPos);
		float distB = Vector3.SqrMagnitude(b.point - castStartPos);

		return distA.CompareTo(distB);
	}
}