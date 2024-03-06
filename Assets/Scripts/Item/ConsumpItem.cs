using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class ConsumpItem : MultipleItem
{
	public ConsumpItem(ItemData itemData, int amount) : base(itemData, amount)
	{
	}
}