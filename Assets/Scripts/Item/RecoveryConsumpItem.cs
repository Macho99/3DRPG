using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[Serializable]
public enum RecoveryType { HP, MP }

public class RecoveryConsumpItem : ConsumpItem
{
	RecoveryConsumpItemData recoveryConsumpItemData;

	public RecoveryConsumpItem(ItemData itemData, int amount = 1) : base(itemData, amount)
	{
		recoveryConsumpItemData = itemData as RecoveryConsumpItemData;
		if (recoveryConsumpItemData == null)
		{
			Debug.LogError($"{ID}의 Type이 잘못 설정되어있습니다");
			return;
		}
	}

	public RecoveryType RecoveryType { get { return recoveryConsumpItemData.RecoveryType; } }
	public int RecoveryAmount { get { return recoveryConsumpItemData.RecoveryAmount; } }
	public ParticleSystem ParticlePrefab { get { return recoveryConsumpItemData.ParticlePrefab; } }

	public override Item Clone()
	{
		return new RecoveryConsumpItem(itemData);
	}

	public override void Use()
	{
		GameManager.Resource.Instantiate(ParticlePrefab, FieldSFC.Player.transform.position,
			Quaternion.identity, FieldSFC.Player.transform, true);
		switch (this.RecoveryType)
		{
			case RecoveryType.HP:
				GameManager.Stat.AddCurHP(RecoveryAmount);
				break;
			case RecoveryType.MP:
				GameManager.Stat.AddCurMP(RecoveryAmount);
				break;
		}
		GameManager.Inven.SubItem(this, 1);
	}
}