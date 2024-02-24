using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class KatanaOnAirSwingBase : KatanaSwingBase
{
	protected KatanaOnAirSwingBase(Katana owner, StateMachine<Katana.State, Katana> stateMachine, 
		string triggerName, int maxAttackNum = 1)
		: base(owner, stateMachine, triggerName, Player.State.OnAirAttack, maxAttackNum)
	{
	}
}