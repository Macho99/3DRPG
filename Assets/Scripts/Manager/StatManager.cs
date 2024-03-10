using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class StatManager : MonoBehaviour
{
	[SerializeField] private int maxHP = 100;
	[SerializeField] private int curHP;
	[SerializeField] private int maxMP = 100;
	[SerializeField] private int curMP;
	[SerializeField] private float attackMultiplier = 1;
	[SerializeField] private int defence = 0;
	[SerializeField] private int money;

	public int MaxHP { get { return maxHP; } }
	public int CurHP {  get { return curHP; } }
	public int MaxMP { get { return maxMP; } }
	public int CurMP { get { return curMP; } }
	public float HPRatio { get { return (float)curHP / maxHP; } }
	public float MPRatio { get { return (float)CurMP / maxMP; } }

	public float AttackMultiplier { get {  return attackMultiplier; } }
	public int Defence { get { return defence; } }

	[HideInInspector] public UnityEvent OnPlayerDie = new();
	[HideInInspector] public UnityEvent<float> OnPlayerHPChange = new();
	[HideInInspector] public UnityEvent<float> OnPlayerMPChange = new();

	private void Awake()
	{
		curHP = maxHP;
	}

	public void AddMaxHP(int amount)
	{
		maxHP += amount;
		OnPlayerHPChange?.Invoke(HPRatio);
	}

	public void AddCurHP(int amount)
	{
		curHP += amount;
		if(curHP > maxHP)
			curHP = maxHP;
		OnPlayerHPChange?.Invoke(HPRatio);
	}

	public void SubCurHP(int amount)
	{
		curHP -= amount;
		OnPlayerHPChange?.Invoke(HPRatio);
		if (curHP <= 0)
		{
			curHP = 0;
			OnPlayerDie?.Invoke();
		}
	}

    public void AddMaxMP(int amount)
	{
		maxMP += amount;
		OnPlayerMPChange?.Invoke(MPRatio);
	}

	public void AddCurMP(int amount)
	{
		curMP += amount;
		if (curMP > maxMP)
			curMP = maxMP;
		OnPlayerMPChange?.Invoke(MPRatio);
	}

	public bool TrySubCurHP(int amount)
	{
		if (curMP < amount)
			return false;

		curMP -= amount;
		OnPlayerMPChange?.Invoke(MPRatio);
		return true;
	}
}