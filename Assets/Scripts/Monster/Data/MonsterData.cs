using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterDataList
{
    public string soundName;
    public GameObject effect;
    public AudioClip sound;
}

[CreateAssetMenu(fileName = "Monster Data", menuName = "Monster Data/Monster", order = 0)]
public class MonsterData : ScriptableObject
{
    //public List<MonsterDataList> monsters = new List<MonsterDataList>();
    public List<MonsterDataList> Orcs = new List<MonsterDataList>();
    public List<MonsterDataList> Skeletons = new List<MonsterDataList>();
    public List<MonsterDataList> TargetDummies = new List<MonsterDataList>();
    public List<MonsterDataList> Mimics = new List<MonsterDataList>();
}
