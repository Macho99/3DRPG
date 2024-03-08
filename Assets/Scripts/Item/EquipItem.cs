using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class EquipItem : Item
{
	public EquipItem(ItemData itemData) : base(itemData)
	{
	}

	public override void Use()
	{
		throw new NotImplementedException();
	}
}