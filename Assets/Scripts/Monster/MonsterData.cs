using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterDataList
{
    public string soundName;
    public GameObject hitEffect;
    public AudioClip sound;
}

[CreateAssetMenu(fileName = "Monster Data", menuName = "Monster Data/Monster", order = 0)]
public class MonsterData : ScriptableObject
{
    public List<MonsterDataList> monsters = new List<MonsterDataList>();
}
