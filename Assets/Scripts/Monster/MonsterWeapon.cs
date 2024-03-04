using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeapon : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            print("Player Hit!!");
        }
    }
}
