using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HPConsumpItem : ConsumpItem
{
	public HPConsumpItem(ItemData itemData, int amount = 1) : base(itemData, amount)
	{
	}

	public override Item Clone()
	{
		return new HPConsumpItem(itemData);
	}

	public override void Use()
	{
		throw new NotImplementedException();
	}
}