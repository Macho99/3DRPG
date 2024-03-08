using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BossSkill
{
    public string skillName;
    public GameObject skill;
    public AudioClip sound;
}

[Serializable]
public class BossSFX
{
    public string soundName;
    public GameObject effect;
    public AudioClip sound;
}

[CreateAssetMenu(fileName = "Boss Skill", menuName = "Monster Data/Boss Skill", order = 0)] 
public class BossSkillData : ScriptableObject
{
    public List<BossSkill> bossSkills = new List<BossSkill>();
    public List<BossSFX> bossSfxes = new List<BossSFX>();
}
