using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DroptableDataList
{
    public MonsterRace race;
}

[CreateAssetMenu(fileName = "Droptable", menuName = "Monster Data/Droptable", order = 0)]
public class DroptableData : ScriptableObject
{
    public List<ItemData> OrcDroptable = new(); 
    public List<ItemData> SkeletonDroptable = new(); 
    public List<ItemData> TargetDummyDroptable = new(); 
    public List<ItemData> MimicDroptable = new();
    public List<ItemData> BossDroptable = new();

    public GameObject itemObj;
    public AudioClip pickupSound;
}
