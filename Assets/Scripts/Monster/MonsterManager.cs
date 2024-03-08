using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private static MonsterManager instance;
    public static MonsterManager Instance {  get { return instance; } }

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    [SerializeField] private BossSkillData bossSkillData;
    [SerializeField] private MonsterData monsterDataList;

    public AudioClip GetBossSkillSound(string skillName)
    {
        BossSkill bossSkill = bossSkillData.bossSkills.Find(skill => skill.skillName == skillName);

        if (bossSkill != null)
        {
            return bossSkill.sound;
        }

        return null;
    }

    public GameObject GetBossSkill(string skillName)
    {
        BossSkill bossSkill = bossSkillData.bossSkills.Find(skill => skill.skillName == skillName);

        if (bossSkill != null)
        {
            return bossSkill.skill;
        }

        return null;
    }

    public AudioClip GetMonsterSound(string soundName)
    {
        MonsterDataList monsterData = monsterDataList.monsters.Find(sound => sound.soundName == soundName);

        if (monsterData != null)
        {
            return monsterData.sound;
        }

        return null;
    }

    public GameObject GetMonsterHitEffect(string soundName)
    {
        MonsterDataList monsterData = monsterDataList.monsters.Find(effect => effect.soundName == soundName);

        if (monsterData != null)
        {
            return monsterData.hitEffect;
        }

        return null;
    }
}
