using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class KatanaQuickDraw1 : KatanaQuickDrawBase
{
	public KatanaQuickDraw1(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack4", false)
	{
	}
}

public class KatanaQuickDraw2 : KatanaQuickDrawBase
{
	public KatanaQuickDraw2(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack5", false)
	{

	}
}
public class KatanaQuickDraw3 : KatanaQuickDrawBase
{
	public KatanaQuickDraw3(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack31", false)
	{
	}
}

public class KatanaQuickDraw4 : KatanaQuickDrawBase
{
	public KatanaQuickDraw4(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack6", true)
	{
	}
}


public class KatanaQuickDraw5 : KatanaQuickDrawBase
{
	public KatanaQuickDraw5(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack17", true)
	{
	}

	protected override void Attack(bool clockwise)
	{
		//base.Attack(clockwise);
		owner.PlayDeathfaultFeedback();
	}
}

public class KatanaQuickDraw6 : KatanaQuickDrawBase
{
	public KatanaQuickDraw6(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack7", true)
	{
	}
}