using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class InvenUI : PopUpUI
{
	protected override void Awake()
	{
		base.Awake();
		buttons["CloseButton"].onClick.AddListener(CloseUI);
	}
}