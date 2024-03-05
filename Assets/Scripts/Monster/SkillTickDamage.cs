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
            // 플레이어 데미지 함수 실행
            print("Player Tick Damage!!");
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
