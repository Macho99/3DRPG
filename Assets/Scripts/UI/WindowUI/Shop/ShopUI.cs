using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : WindowUI
{
    protected override void Awake()
    {
        base.Awake();
    }

        public void ReturnInputIgnore()
    {
        FieldSFC.Player.IgnoreInput(false);
    }
}
