using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ProBuilder.MeshOperations;

public class StatManager : MonoBehaviour
{
	[SerializeField] private int curHP;
	[SerializeField] private int curMP = 50;
	[SerializeField] private int maxHP = 100;
	[SerializeField] private int maxMP = 100; 
	[SerializeField] private int recoveryHP = 1;
	[SerializeField] private int recoveryMP = 1;
	//[SerializeField] private float attackMultiplier = 1;
	[SerializeField] private int defence = 0;
	[SerializeField] private int stunResistance = 0;
	[SerializeField] private int money = 0;

	public int MaxHP { get { return maxHP; } }
	public int CurHP {  get { return curHP; } }
	public int MaxMP { get { return maxMP; } }
	public int CurMP { get { return curMP; } }
	public int RecoveryHP { get { return recoveryHP; } }
	public int RecoveryMP { get { return recoveryMP; } }
	public float HPRatio { get { return (float)curHP / maxHP; } }
	public float MPRatio { get { return (float)curMP / maxMP; } }

	//public float AttackMultiplier { get {  return attackMultiplier; } }
	public int Defence { get { return defence; } }
    public int Money { get { return money; } }
	public int StunResistance { get { return stunResistance; } }

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

	public void SubMaxHP(int amount)
	{
		maxHP -= amount;
		if(curHP > maxHP)
		{
			curHP = maxHP;
		}
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
		if (curHP <= 0)
		{
			curHP = 0;
			OnPlayerDie?.Invoke();
		}
		OnPlayerHPChange?.Invoke(HPRatio);
	}

	public void AddMaxMP(int amount)
	{
		maxMP += amount;
		OnPlayerMPChange?.Invoke(MPRatio);
	}

	public void SubMaxMP(int amount)
	{
		maxMP -= amount;
		if (curMP > maxMP)
		{
			curMP = maxMP;
		}
		OnPlayerMPChange?.Invoke(MPRatio);
	}

	public void AddCurMP(int amount)
	{
		curMP += amount;
		if (curMP > maxMP)
			curMP = maxMP;
		OnPlayerMPChange?.Invoke(MPRatio);
	}

	public bool TrySubCurMP(int amount)
	{
		if (curMP < amount)
		{
			GameManager.UI.MakeAlarm("마나가 부족합니다!", $"필요 마나: {amount - curMP}");
			FieldSFC.Instance?.PlayMPLack();
			return false;
		}

		curMP -= amount;
		OnPlayerMPChange?.Invoke(MPRatio);
		return true;
	}

	public void AddMoney(int amount)
	{
        money += amount;
    }

    public void SubMoney(int amount)
    {
        if (money - amount < 0) { return; }
		else
		{
            money -= amount;
        }
    }
	public void EquipArmor(ArmorItem armorItem)
	{
		if(armorItem == null) { return; }

		ArmorStat armorStat = armorItem.ArmorStat;

		AddMaxHP(armorStat.maxHP);
		AddMaxMP(armorStat.maxMP);
		recoveryHP += armorStat.recoveryHP;
		recoveryMP += armorStat.recoveryMP;
		defence += armorStat.defence;
		stunResistance += armorStat.stunResistance;
	}

	public void UnequipArmor(ArmorItem armorItem)
	{
		if (armorItem == null) { return; }

		ArmorStat armorStat = armorItem.ArmorStat;

		SubMaxHP(armorStat.maxHP);
		SubMaxMP(armorStat.maxMP);
		recoveryHP -= armorStat.recoveryHP;
		recoveryMP -= armorStat.recoveryMP;
		defence -= armorStat.defence;
		stunResistance -= armorStat.stunResistance;
	}
}