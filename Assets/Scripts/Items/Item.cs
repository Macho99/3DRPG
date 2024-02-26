using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public Sprite itemIcon;
    public string itemName;
    public string itemExplain;
    public string itemStatus;
}

[Serializable]
public class Armor : Item
{

}

[Serializable]
public class Consum : Item
{

}
