using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkills : MonoBehaviour
{
    public List<GameObject> normalSKills = new List<GameObject>();
    public List<GameObject> twoHandedSkills = new List<GameObject>();

    public Transform shotPoint;

    private void UseNormalSkill_1()
    {
        GameObject skill1Prefab = Instantiate(normalSKills[0], shotPoint.position, Quaternion.identity);
        Rigidbody rb = skill1Prefab.GetComponent<Rigidbody>();
        skill1Prefab.transform.forward = transform.forward;
        rb.velocity = skill1Prefab.transform.forward * 20f;
        Destroy(skill1Prefab, 2f);
    }
}
