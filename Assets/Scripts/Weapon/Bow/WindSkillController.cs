using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSkillController : MonoBehaviour 
{
	[SerializeField] float followSpeed = 4f;
	[SerializeField] Vector3 followOffset = new Vector3(0f, 1.7f, 0f);
	[SerializeField] float rotationSpeed = 50f;
	[SerializeField] float radius = 0.5f;

	List<FloatingArrow> floatingArrowList = new();
	Player player;

	bool stop;

	private void Awake()
	{
		stop = false;
		player = FieldSFC.Player;
	}

	public void Init(int arrowNum)
	{
		stop = false;
		InitArrows(arrowNum);
		_ = StartCoroutine(CoUpdate());
	}

	private void InitArrows(int arrowNum)
	{
		while (floatingArrowList.Count > arrowNum)
		{
			int count = floatingArrowList.Count;
			FloatingArrow obj = floatingArrowList[count - 1];
			floatingArrowList.Remove(obj);
			GameManager.Resource.Destroy(obj.gameObject);
		}

		while(floatingArrowList.Count < arrowNum)
		{
			FloatingArrow obj = GameManager.Resource.Instantiate<FloatingArrow>("Prefab/FloatingArrow", 
				Vector3.zero, Quaternion.identity, transform);
			floatingArrowList.Add(obj);
		}

		if(arrowNum == 0)
		{
			Debug.LogError($"arrowNum은 0이어선 안됩니다");
			return;
		}

		int idx = 0;
		float deltaAngle = 360 / floatingArrowList.Count;
		foreach(FloatingArrow arrow in floatingArrowList)
		{
			Vector3 pos = new Vector3();
			pos.x = Mathf.Cos(deltaAngle * idx * Mathf.Deg2Rad) * radius;
			pos.z = Mathf.Sin(deltaAngle * idx * Mathf.Deg2Rad) * radius;
			arrow.transform.localPosition = pos;

			idx++;
			arrow.Init();
		}
	}

	public void Stop()
	{
		stop = true;
	}

	public void PrepareAttack()
	{
		foreach (FloatingArrow arrow in floatingArrowList)
		{
			arrow.PrepareAttack();
		}
	}

	public void Attack(Transform target)
	{
		foreach(FloatingArrow arrow in floatingArrowList)
		{
			arrow.Attack(target);
		}
	}

	private IEnumerator CoUpdate()
	{
		while(stop == false)
		{
			transform.position = Vector3.Lerp(transform.position,
				player.transform.position + followOffset, Time.deltaTime * followSpeed);
			transform.rotation = Quaternion.Euler(0f, rotationSpeed * Time.deltaTime, 0f) * transform.rotation;

			yield return null;
		}
	}
}