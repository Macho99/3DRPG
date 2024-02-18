using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class KatanaQuickDraw1 : KatanaQuickDrawBase
{
	public KatanaQuickDraw1(Katana owner, StateMachine<Katana.State, Katana> stateMachine) : base(owner, stateMachine, "Attack4")
	{
	}
}

public class KatanaQuickDraw2 : KatanaQuickDrawBase
{
	public KatanaQuickDraw2(Katana owner, StateMachine<Katana.State, Katana> stateMachine) : base(owner, stateMachine, "Attack5")
	{
	}
}

public class KatanaQuickDraw3 : KatanaQuickDrawBase
{
	public KatanaQuickDraw3(Katana owner, StateMachine<Katana.State, Katana> stateMachine) : base(owner, stateMachine, "Attack6")
	{
	}
}

public class KatanaQuickDraw4 : KatanaQuickDrawBase
{
	public KatanaQuickDraw4(Katana owner, StateMachine<Katana.State, Katana> stateMachine) : base(owner, stateMachine, "Attack7")
	{
	}
}