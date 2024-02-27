using JetBrains.Annotations;
using System;
using UnityEngine;

[Serializable]
public enum ItemID
{
    // ����
    Katana,

    // ��
    MetalArmor,

    // �Ҹ�ǰ
    Apple,
}

[Serializable]
public enum ItemType
{
    Consum,
    Weapon,
    Armor,
    ETC // ��Ÿ ������
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