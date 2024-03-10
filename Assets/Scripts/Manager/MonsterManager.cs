using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int curHP;

    public int MaxHP { get { return maxHP; } }
    public int CurHP { get { return curHP; } }
    public float HPRatio { get { return (float)curHP / maxHP; } }

    [HideInInspector] public UnityEvent OnBossDie = new();
    [HideInInspector] public UnityEvent<float> OnBossHPChange = new();

    //public void AddCurHP(int amount)
    //{
    //    curHP += amount;
    //    if (curHP > maxHP)
    //        curHP = maxHP;
    //    OnBossHPChange?.Invoke(HPRatio);
    //}

    public void SubCurHP(int amount)
    {
        curHP -= amount;
        OnBossHPChange?.Invoke(HPRatio);
        if (curHP <= 0)
        {
            curHP = 0;
            OnBossDie?.Invoke();
        }
    }

    [SerializeField] private BossSkillData bossSkillData;
    [SerializeField] private MonsterData monsterDataList;
    [SerializeField] private DroptableData droptableDataList;

    private void Awake()
    {
        bossSkillData = GameManager.Resource.Load<BossSkillData>("MonsterData/BossSkill");
        monsterDataList = GameManager.Resource.Load<MonsterData>("MonsterData/MonsterData");
        droptableDataList = GameManager.Resource.Load<DroptableData>("MonsterData/Droptable");
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

    public string GetDropItemId(MonsterRace race)
    {
        List<ItemData> dropTable;
        string itemName;

        switch (race)
        {
            case MonsterRace.Orc:
                dropTable = droptableDataList.OrcDroptable;
                break;
            case MonsterRace.Skeleton:
                dropTable = droptableDataList.SkeletonDroptable;
                break;
            case MonsterRace.TargetDummy:
                dropTable = droptableDataList.TargetDummyDroptable;
                break;
            case MonsterRace.Mimic:
                dropTable = droptableDataList.MimicDroptable;
                break;
            case MonsterRace.Boss:
                dropTable = droptableDataList.BossDroptable;
                break;
            default:
                return null;
        }

        if (dropTable != null)
        {
            itemName = GetRandomItem(dropTable).Id;
            return itemName;
        }

        return null;
    }

    private ItemData GetRandomItem(List<ItemData> list)
    {
        if (list == null || list.Count == 0)
        {
            Debug.LogError("드랍테이블이 null 이거나 비어있습니다.");
            return null;
        }

        int randomIndex = Random.Range(0, list.Count);
        return list[randomIndex];
    }

    public GameObject GetItemObj()
    {
        return droptableDataList.itemObj;
    }

    public AudioClip GetPickupItemSound()
    {
        return droptableDataList.pickupSound;
    }
}
