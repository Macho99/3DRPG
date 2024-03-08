using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    //private static MonsterManager instance;
    //public static MonsterManager Instance {  get { return instance; } }

    //private void Awake()
    //{
    //    if (instance != null)
    //    {
    //        DestroyImmediate(this);
    //        return;
    //    }

    //    instance = this;
    //    DontDestroyOnLoad(this);
    //}


    [SerializeField] private BossSkillData bossSkillData;
    [SerializeField] private MonsterData monsterDataList;

    private void Awake()
    {
        bossSkillData = GameManager.Resource.Load<BossSkillData>("MonsterData/BossSkill");
        monsterDataList = GameManager.Resource.Load<MonsterData>("MonsterData/MonsterData");
    }

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

    public AudioClip GetBossSound(string soundName)
    {
        BossSFX bossSFX = bossSkillData.bossSfxes.Find(sound => sound.soundName == soundName);

        if (bossSFX != null)
        {
            return bossSFX.sound;
        }

        return null;
    }

    public GameObject GetBossEffect(string soundName)
    {
        BossSFX bossSFX = bossSkillData.bossSfxes.Find(effect => effect.soundName == soundName);

        if (bossSFX != null)
        {
            return bossSFX.effect;
        }

        return null;
    }

    public AudioClip GetMonsterSound(MonsterRace race, string soundName)
    {
        MonsterDataList monsterData;

        switch (race)
        {
            case MonsterRace.Orc:
                monsterData = monsterDataList.Orcs.Find(sound => sound.soundName == soundName);
                break;
            case MonsterRace.Skeleton:
                monsterData = monsterDataList.Skeletons.Find(sound => sound.soundName == soundName);
                break;
            case MonsterRace.TargetDummy:
                monsterData = monsterDataList.TargetDummies.Find(sound => sound.soundName == soundName);
                break;
            case MonsterRace.Mimic:
                monsterData = monsterDataList.Mimics.Find(sound => sound.soundName == soundName);
                break;
            default:
                return null;
        }
        //MonsterDataList monsterData = monsterDataList.monsters.Find(sound => sound.soundName == soundName);

        if (monsterData != null)
        {
            return monsterData.sound;
        }

        return null;
    }

    public GameObject GetMonsterEffect(MonsterRace race, string soundName)
    {
        MonsterDataList monsterData;

        switch (race)
        {
            case MonsterRace.Orc:
                monsterData = monsterDataList.Orcs.Find(effect => effect.soundName == soundName);
                break;
            case MonsterRace.Skeleton:
                monsterData = monsterDataList.Skeletons.Find(sound => sound.soundName == soundName);
                break;
            case MonsterRace.TargetDummy:
                monsterData = monsterDataList.TargetDummies.Find(sound => sound.soundName == soundName);
                break;
            case MonsterRace.Mimic:
                monsterData = monsterDataList.Mimics.Find(sound => sound.soundName == soundName);
                break;
            default:
                return null;
        }

        if (monsterData != null)
        {
            return monsterData.effect;
        }

        return null;
    }
}
