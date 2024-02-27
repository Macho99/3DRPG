using JetBrains.Annotations;
using System;
using UnityEngine;

[Serializable]
public enum ItemID
{
    // 무기
    Katana,

    // 방어구
    MetalArmor,

    // 소모품
    Apple,
}

[Serializable]
public enum ItemType
{
    Consum,
    Weapon,
    Armor,
    ETC // 기타 아이템
}

public abstract class SOItem : ScriptableObject
{
    public ItemID ID;
    public ItemType Type;
    public string Name;
    [TextArea(15, 20)]
    public string Description;
    [TextArea(15, 20)]
    public string Summary;
    public int Price;
    public Sprite Icon;
}