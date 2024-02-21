using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Sword : Weapon
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

	private RaycastHit[] hits;
	private BoxCollider col;
	private PlayerAttack playerAttack;
	private PlayerAnimEvent playerAnimEvent;
	private bool IsAttacking;
	private FrameInfo prev;
	private FrameInfo cur;

	protected override void Awake()
	{
		base.Awake();
		hits = new RaycastHit[10];
		col = GetComponentInChildren<BoxCollider>(true);
		playerAttack = FieldSFC.Player.GetComponent<PlayerAttack>();
	}

	private void Start()
	{
		playerAnimEvent = playerAttack.AnimEvent;
	}

	public override void Attack()
	{
		playerAttack.SetAnimTrigger("Attack1");
		playerAnimEvent.OnAttackStart.AddListener(AttackStart);
		playerAnimEvent.OnAttackEnd.AddListener(AttackEnd);
	}

	private void AttackStart()
	{
		_ = StartCoroutine(CoAttack());
		playerAnimEvent.OnAttackStart.RemoveListener(AttackStart);
	}

	private void AttackEnd()
	{
		IsAttacking = false;
	}

	private IEnumerator CoAttack()
	{
		List<GameObject> hitList = new List<GameObject>();
		IsAttacking = true;
		TargetFollower trail = GameManager.Resource.Instantiate<TargetFollower>("Prefab/SwordTrail", true);
		trail.SetTarget(trailTrans);
		trail.transform.position = trailTrans.position;
		trail.GetComponent<TrailRenderer>().Clear();

		while (IsAttacking == true)
		{
			prev.Set(col.transform.position + col.transform.rotation * col.center, col.transform.rotation);
			yield return null;
			if(AttackCast(prev, hitList) == false)
			{
				playerAttack.SetAnimFloat("Reverse", -0.4f);
				trail.SetTarget(null);
				while (playerAttack.GetAnimNormalizedTime(0) > 0.05f)
				{
					yield return null;
				}
				playerAttack.SetAnimFloat("Reverse", 1f);
				playerAnimEvent.OnAttackEnd.RemoveListener(AttackEnd);
				IsAttacking = false;
				playerAttack.SetAnimTrigger("Exit");
				yield break;
			}
		}
		trail.SetTarget(null);
		playerAnimEvent.OnAttackEnd.RemoveListener(AttackEnd);
		yield return new WaitUntil(() => playerAttack.IsAnimWait(0));

		playerAttack.SetAnimTrigger("Exit");

		//foreach(GameObject obj in hitList)
		//{
		//	print(obj.name);
		//}
	}

	private bool AttackCast(FrameInfo prev, in List<GameObject> hitList)
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