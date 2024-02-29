using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterShield : Monster
{
    [SerializeField] private float maxStamina;
    [SerializeField] private float currentStamina;
    [SerializeField] private float staminaRate; // ���¹̳� ����� ��ġ
    [SerializeField] private float blockAngle; // ��� ���� ����
    public bool guardHit; // ���з� ���Ҿ����� üũ

    public Transform blockTarget;

    protected override void Start()
    {
        base.Start();
        currentStamina = maxStamina;
        StartCoroutine(FindGuardTargetWithDelay(.2f));
    }

    private void Update()
    {
        if (state == State.IDLE)
        {
            currentStamina = Mathf.Clamp(currentStamina + staminaRate * Time.deltaTime, 0, maxStamina);
        }
    }

    IEnumerator FindGuardTargetWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindGuardTargets();
        }
    }

    private void FindGuardTargets()
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

    public override void TakeDamage(float damage)
    {
        if (state == State.DEAD) { return; }


        if (blockTarget != null && state == State.BLOCK)
        {
            // ���ҵ� ������ ����
            currentStamina -= 20;
            guardHit = true;
            print("Reduce Damage or Guard Damage");

            if (currentStamina <= 0)
            {
                // ���� �극��ũ
                state = State.GUARD_BREAK;
                currentStamina = maxStamina;
                anim.SetTrigger("GuardBreak");
                print("Guard Break");
            }
            else
            {
                anim.SetTrigger("Guard");
            }
        }
        else if (state == State.GUARD_BREAK)
        {
            if (currentHp > 0f)
            {
                currentHp -= damage * 1.5f;
                anim.SetTrigger("Hit");
                viewAngle = 360f;
                obstacleMask = LayerMask.NameToLayer("Nothing");
            }
            else
            {
                Die();
            }

        }
        else
        {
            if (currentHp > 0f)
            {
                currentHp -= damage;
                anim.SetTrigger("Hit");
                viewAngle = 360f;
                obstacleMask = LayerMask.NameToLayer("Nothing");
            }
            else
            {
                Die();
            }
        }
    }
}
