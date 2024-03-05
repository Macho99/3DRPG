using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class EquipItem : Item
{
	protected EquipItem(string id, Type itemType) : base(id, itemType)
	{
	}
}