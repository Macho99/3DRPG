using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class KatanaJumpCombo01 : KatanaOnAirSwingBase
{
	public KatanaJumpCombo01(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack18")
	{
	}

	protected override bool CheckTransition()
	{
		return false;
	}
}