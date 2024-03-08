using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OtherItem : MultipleItem
{
	public OtherItem(ItemData itemData, int amount = 1) : base(itemData, amount)
	{
	}

	public override Item Clone()
	{
		return new OtherItem(itemData);
	}

	public override void Use() { }
}