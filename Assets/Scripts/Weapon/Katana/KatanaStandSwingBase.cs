using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class KatanaStandSwingBase : KatanaSwingBase
{
	public KatanaStandSwingBase(Katana owner, StateMachine<Katana.State, Katana> stateMachine, string triggerName)
		: base(owner, stateMachine, triggerName, Player.State.StandAttack)
	{
	}
}