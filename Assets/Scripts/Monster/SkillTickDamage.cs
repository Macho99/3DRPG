using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTickDamage : MonoBehaviour
{
    public float damage;
    bool isDelay = false;

    private void OnTriggerStay(Collider other)
    {
        if (!isDelay && other.TryGetComponent(out Player player))
        {
            print("Player Hit!!");
            StartCoroutine(TickDamage());
        }
    }

    IEnumerator TickDamage()
    {
        isDelay = true;
        yield return new WaitForSeconds(1f);
        isDelay = false;
    }
}
