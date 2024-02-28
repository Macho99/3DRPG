using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkills : MonoBehaviour
{
    [SerializeField] private List<GameObject> normalSKills = new List<GameObject>();
    [SerializeField] private List<GameObject> twoHandedSkills = new List<GameObject>();

    [SerializeField] private Transform weaponPoint;
    [SerializeField] private Transform kickPoint;

    private void UseNormalMeleeAttack_3()
    {
        GameObject skill1Prefab = Instantiate(normalSKills[2], kickPoint.position, Quaternion.identity);
        skill1Prefab.transform.forward = transform.forward;
        Destroy(skill1Prefab, 1f);
    }

    private void UseNormalSkill_1()
    {
        GameObject skill1Prefab = Instantiate(normalSKills[0], weaponPoint.position, Quaternion.identity);
        Rigidbody rb = skill1Prefab.GetComponent<Rigidbody>();
        skill1Prefab.transform.forward = transform.forward;
        rb.velocity = skill1Prefab.transform.forward * 20f;
        Destroy(skill1Prefab, 5f);
    }

    private void UseNormalSkill_2()
    {
        Vector3 spawnPosition = transform.position + 4f * transform.forward;
        GameObject skill1Prefab = Instantiate(normalSKills[1], spawnPosition, Quaternion.identity);
        Rigidbody rb = skill1Prefab.GetComponent<Rigidbody>();
        skill1Prefab.transform.forward = transform.forward;
        rb.velocity = skill1Prefab.transform.forward * 20f;
        Destroy(skill1Prefab, 5f);
    }

    private void UseNormalSkill_3()
    {
        Vector3 spawnPosition = transform.position + 7f * transform.forward;
        GameObject skill1Prefab = Instantiate(normalSKills[3], spawnPosition, Quaternion.identity);
        skill1Prefab.transform.forward = transform.forward;
        Destroy(skill1Prefab, 7f);
    }

    private void UseTwoHandedSkill_1()
    {

    }

    private void UseTwoHandedSkill_2()
    {

    }

    private void UseTwoHandedSkill_3()
    {

    }
}
