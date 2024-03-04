using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedMonster : MonoBehaviour
{
    public float meleeDistance;

    public GameObject bulletPrefab;
    public Transform shotPoint;

    Monster monster;

    private void Start()
    {
        monster = GetComponent<Monster>();
    }

    public void Shot()
    {
        print("Shot!!!");

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
