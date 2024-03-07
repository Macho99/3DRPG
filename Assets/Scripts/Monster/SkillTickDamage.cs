using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTickDamage : MonoBehaviour
{
    public int damage;
    bool isDelay = false;
    public float tickDelay;

    private void OnTriggerStay(Collider other)
    {
        if (!isDelay && other.TryGetComponent(out Player player))
        {
            player.TakeDamage(damage, true);
            StartCoroutine(TickDamage());
        }
    }

    IEnumerator TickDamage()
    {
        isDelay = true;
        yield return new WaitForSeconds(tickDelay);
        isDelay = false;
    }
}
