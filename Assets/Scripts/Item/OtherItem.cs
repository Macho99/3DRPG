using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OtherItem : MultipleItem
{
	public OtherItem(ItemData itemData) : base(itemData)
	{
	}

	public override Item Clone()
	{
		return new OtherItem(itemData);
	}

	public override void Use() { }
}