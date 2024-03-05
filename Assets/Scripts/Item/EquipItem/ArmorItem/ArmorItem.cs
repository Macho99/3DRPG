using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class ArmorItem : EquipItem
{
	protected ArmorItem(string id) : base(id, Type.Armor)
	{
	}
}