using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class MultipleItem : Item
{
	int amount;
	public int Amount { get { return amount; } }
	protected MultipleItem(ItemData itemData, int amount) : base(itemData)
	{
		this.amount = amount;
	}
	public void AddAmount (int value)
	{
		amount += value;
	}

	public bool SubAmount(int value)
	{
		if(amount < value)
		{
			return false;
		}

		amount -= value;
		return true;
	}
}