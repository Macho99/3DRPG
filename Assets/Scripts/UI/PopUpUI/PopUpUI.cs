using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpUI : BaseUI
{


	public override void CloseUI()
	{
		GameManager.UI.ClosePopUpUI();
	}
}
