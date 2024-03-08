using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Recovery Consump Item Data", 
	menuName = "Scriptable Object/Recovery Consump Item Data", order = int.MaxValue)]
public class RecoveryConsumpItemData : ItemData
{
	[SerializeField] RecoveryType recoveryType;
	[SerializeField] int recoveryAmount;
	[SerializeField] ParticleSystem paritclePrefab;

	private void Awake()
	{
		itemType = Item.Type.RecoveryConsump;
	}

	public RecoveryType RecoveryType { get { return recoveryType; } }
	public int RecoveryAmount { get { return recoveryAmount; } }
	public ParticleSystem ParticlePrefab { get { return paritclePrefab; } }
}