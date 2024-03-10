using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkills : MonoBehaviour
{
    [SerializeField] private Transform weaponPoint;
    [SerializeField] private Transform kickPoint;

    DeathKnight knight;
    AudioSource audioSource;

    private Vector3 targetDir;
    private Transform target;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        knight = GetComponent<DeathKnight>();
        target = knight.target;
    }

    private void UseNormalMeleeAttack_3()
    {
        GameObject skillPrefab = Instantiate(GameManager.Monster.GetBossSkill("NormalMeleeAttack3"), kickPoint.position, Quaternion.identity); 
        skillPrefab.GetComponent<MonsterWeapon>().knightTf = transform;
        skillPrefab.transform.forward = transform.forward;
        audioSource?.PlayOneShot(GameManager.Monster.GetBossSkillSound("NormalMeleeAttack3"));
        Destroy(skillPrefab, 1f);
    }

    private void UseNormalSkill_1()
    {
        GameObject skillPrefab = Instantiate(GameManager.Monster.GetBossSkill("NormalSkill1"), weaponPoint.position, Quaternion.identity);
        Rigidbody rb = skillPrefab.GetComponent<Rigidbody>();
        Vector3 dir = (knight.target.position - transform.position).normalized;
        Vector3 skillDir = new Vector3(dir.x, 0, dir.z);
        skillPrefab.transform.forward = skillDir;
        rb.velocity = skillPrefab.transform.forward * 20f;
        audioSource?.PlayOneShot(GameManager.Monster.GetBossSkillSound("NormalSkill1"));
        Destroy(skillPrefab, 5f);
    }

    private void UseNormalSkill_2()
    {
        Vector3 spawnPosition = transform.position + 4f * transform.forward;
        GameObject skillPrefab = Instantiate(GameManager.Monster.GetBossSkill("NormalSkill2"), spawnPosition, Quaternion.identity);
        Rigidbody rb = skillPrefab.GetComponent<Rigidbody>();
        Vector3 dir = (knight.target.position - transform.position).normalized;
        Vector3 skillDir = new Vector3(dir.x, 0, dir.z);
        skillPrefab.transform.forward = skillDir;
        rb.velocity = skillPrefab.transform.forward * 20f;
        audioSource?.PlayOneShot(GameManager.Monster.GetBossSkillSound("NormalSkill2"));
        Destroy(skillPrefab, 5f);
    }

    private void UseNormalSkill_3()
    {
        Vector3 spawnPosition = transform.position + 7f * transform.forward;
        GameObject skillPrefab = Instantiate(GameManager.Monster.GetBossSkill("NormalSkill3"), spawnPosition, Quaternion.identity);
        Vector3 dir = (knight.target.position - transform.position).normalized;
        Vector3 skillDir = new Vector3(dir.x, 0, dir.z);
        skillPrefab.transform.forward = skillDir;
        audioSource?.PlayOneShot(GameManager.Monster.GetBossSkillSound("NormalSkill3"));
        Destroy(skillPrefab, 7f);
    }

    private void UseTwoHandedSkill_1()
    {
        GameObject skillPrefab = Instantiate(GameManager.Monster.GetBossSkill("TwoHandedSkill1"), transform.position, Quaternion.identity);
        Rigidbody rb = skillPrefab.GetComponent<Rigidbody>();
        Vector3 dir = (knight.target.position - transform.position).normalized;
        Vector3 skillDir = new Vector3(dir.x, 0, dir.z);
        skillPrefab.transform.forward = skillDir;
        rb.velocity = skillPrefab.transform.forward * 20f;
        audioSource?.PlayOneShot(GameManager.Monster.GetBossSkillSound("TwoHandedSkill1"));
        Destroy(skillPrefab, 5f);
    }

    private void UseTwoHandedSkill_2()
    {
        GameObject skillPrefab = Instantiate(GameManager.Monster.GetBossSkill("TwoHandedSkill2"), weaponPoint.position, Quaternion.identity);
        Vector3 dir = (knight.target.position - transform.position).normalized;
        Vector3 skillDir = new Vector3(dir.x, 0, dir.z);
        skillPrefab.transform.forward = skillDir;

        Destroy(skillPrefab, 2f);
    }

    private void UseTwoHandedSkill_3()
    {
        GameObject skillPrefab = Instantiate(GameManager.Monster.GetBossSkill("TwoHandedSkill3"), transform.position, Quaternion.identity);
        skillPrefab.transform.forward = transform.forward;
        audioSource?.PlayOneShot(GameManager.Monster.GetBossSkillSound("TwoHandedSkill3"));
        Destroy(skillPrefab, 6f);
    }

    private void UseTwoHandedSkill_4()
    {
        knight.skillCoolDown = 45f;
        Vector3 spawnPosition = transform.position + 1f * transform.up;
        GameObject skillPrefab = Instantiate(GameManager.Monster.GetBossSkill("TwoHandedSkill4"), spawnPosition, Quaternion.identity);
        skillPrefab.transform.forward = transform.forward;
        audioSource?.PlayOneShot(GameManager.Monster.GetBossSkillSound("TwoHandedSkill4"));
        Destroy(skillPrefab, 5.5f);
    }
}
