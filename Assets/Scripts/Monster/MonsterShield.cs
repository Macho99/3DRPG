using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterShield : MonoBehaviour
{
    Monster monster;
    Animator anim;

    [SerializeField] private float viewRadius;
    public float blockAngle; // ��� ���� ����

    public Transform blockTarget;

    [SerializeField] private LayerMask targetMask; // Ÿ�ٷ��̾�

    public bool guardHit; // ���з� ���Ҿ����� üũ

    private void Awake()
    {
        monster = GetComponent<Monster>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(FindTargetWithDelay(.2f));
    }

    IEnumerator FindTargetWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindTargets();
        }
    }

    private void FindTargets()
    {
        blockTarget = null;

        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < blockAngle / 2)
            {
                blockTarget = target;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (blockTarget != null && monster.state == State.BLOCK)
        {
            // ���ҵ� ������ ����
            monster.currentStamina -= 20;
            guardHit = true;
            print("Reduce Damage or Guard Damage");

            if (monster.currentStamina <= 0)
            {
                // ���� �극��ũ
                monster.state = State.GUARD_BREAK;
                anim.SetTrigger("GuardBreak");
                print("Guard Break");
            }
            else
            {
                anim.SetTrigger("Guard");
            }
        }
        else if (monster.state == State.GUARD_BREAK)
        {
            monster.TakeDamage(damage * 1.5f);
        }
        else
        {
            monster.TakeDamage(damage);
        }
    }
}
