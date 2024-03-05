using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class WeaponItem : EquipItem
{
	protected WeaponItem(string id) : base(id, Type.Weapon)
	{
	}
}