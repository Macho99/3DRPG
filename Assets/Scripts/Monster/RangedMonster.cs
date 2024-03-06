using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedMonster : MonoBehaviour
{
    public float meleeDistance;

    public GameObject bulletPrefab;
    public Transform shotPoint;

    NavMeshAgent agent;
    Monster monster;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        monster = GetComponent<Monster>();
    }

    private void Start()
    {
        agent.stoppingDistance = monster.attackRange;
    }

    public void Shot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shotPoint.position, Quaternion.identity);
        Vector3 dir = (monster.target.position - transform.position).normalized;
        bullet.transform.forward = dir;
        Destroy(bullet, 3f);
    }

    private void OnAttackStart()
    {
        monster.attackCol.enabled = true;
    }

    private void OnAttackEnd()
    {
        monster.attackCol.enabled = false;
    }
}
