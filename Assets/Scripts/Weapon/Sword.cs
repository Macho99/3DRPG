using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Sword : Weapon
{

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

	private RaycastHit[] hits;
	private BoxCollider col;
	private LayerMask monsterMask;
	private PlayerAttack playerAttack;
	private PlayerAnimEvent playerAnimEvent;
	private bool IsAttacking;
	private FrameInfo prev;
	private FrameInfo cur;

	private void Awake()
	{
		hits = new RaycastHit[10];
		col = GetComponentInChildren<BoxCollider>(true);
		monsterMask = LayerMask.GetMask("Tree");
		playerAttack = FieldSFC.Player.GetComponent<PlayerAttack>();
	}

	private void Start()
	{
		playerAnimEvent = playerAttack.AnimEvent;
	}

	public override void Attack()
	{
		playerAttack.SetAnimTrigger("Attack1");
		playerAnimEvent.OnAttackStart.AddListener(StartAttack);
	}

	private void StartAttack()
	{
		_ = StartCoroutine(CoAttack());
		playerAnimEvent.OnAttackStart.RemoveListener(StartAttack);
	}

	private IEnumerator CoAttack()
	{
		List<GameObject> hitList = new List<GameObject>();
		IsAttacking = true;
		while (true)
		{
			prev.Set(col.transform.position + col.transform.rotation * col.center, col.transform.rotation);
			yield return null;
			AttackCast(prev, hitList);
			if(playerAttack.IsAnimWait(1) == true)
			{
				break;
			}
		}
		IsAttacking = false;
		playerAttack.SetAnimTrigger("Exit");

		foreach(GameObject obj in hitList)
		{
			print(obj.name);
		}
	}

	private void AttackCast(FrameInfo prev, in List<GameObject> hitList)
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

		/*RaycastHit[] curHits = Physics.BoxCastAll(
			cur.position,
			col.size * 0.5f,
			prev.position - cur.position,
			cur.rotation,
			moveDist,
			monsterMask
			);*/
		/*foreach (RaycastHit hit in hits)
		{
			if (hitList.Contains(hit.collider.gameObject) == false)
			{
				hitList.Add(hit.collider.gameObject);
				print(hit.collider.gameObject);
			}
		}*/

		int hitNum = Physics.BoxCastNonAlloc(
			cur.position,
			col.size * 0.5f,
			(prev.position - cur.position).normalized,
			hits,
			cur.rotation,
			moveDist,
			monsterMask
			);

		for(int i = hitNum - 1; i >= 0; i--)
		{
			RaycastHit hit = hits[i];
			if (hitList.Contains(hit.collider.gameObject) == false)
			{
				hitList.Add(hit.collider.gameObject);
				print(hit.collider.gameObject);
			}
		}
	}
}