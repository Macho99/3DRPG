using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterShield : Monster
{
    [SerializeField] private float maxStamina;
    public float currentStamina;
    [SerializeField] private float staminaRate; // 스태미너 재생성 수치
    [SerializeField] private float blockAngle; // 방어 가능 각도
    public bool guardHit; // 방패로 막았었는지 체크

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
            currentStamina -= 20;
            guardHit = true;

            if (currentStamina <= 0)
            {
                state = State.GUARD_BREAK;
                //currentStamina = maxStamina;
                anim.SetTrigger("GuardBreak");
                audioSource?.PlayOneShot(SetSound(race, "GuardBreak"));
            }
            else
            {
                audioSource?.PlayOneShot(SetSound(race, "Block"));
                anim.SetTrigger("Guard");
            }
        }
        else if (state == State.GUARD_BREAK)
        {
            if (currentHp > 0f)
            {
                SubCurHP((int)damage * 2);
                if (canvas.enabled == false)
                {
                    canvas.enabled = true;
                }
                anim.SetTrigger("Hit");
                audioSource?.PlayOneShot(SetSound(race, "Hit"));
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
                SubCurHP((int)damage);
                if (canvas.enabled == false)
                {
                    canvas.enabled = true;
                }
                anim.SetTrigger("Hit");
                viewAngle = 360f;
                obstacleMask = LayerMask.GetMask("Nothing");

                audioSource?.PlayOneShot(SetSound(race, "Hit"));

                if (target == null)
                {
                    Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

                    for (int i = 0; i < targetsInViewRadius.Length; i++)
                    {
                        Transform target = targetsInViewRadius[i].transform;

                        if (target.TryGetComponent(out Player player))
                        {
                            this.target = target;
                        }

                    }
                }
            }
            else
            {
                Die();
            }
        }
    }
}
