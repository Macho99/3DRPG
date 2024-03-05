using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum State
{
    IDLE,
    BLOCK,
    GUARD_BREAK,
    DEAD
}

public class Monster : MonoBehaviour
{
    [SerializeField] protected float maxHp;
    [SerializeField] protected float currentHp;
    [SerializeField] protected int damage;
    [SerializeField] private bool hitFeedback;
    [SerializeField] private float stunDuration;
    [SerializeField] private Vector3 knockback;

    public int Damage => damage;
    public bool HitFeedBack => hitFeedback;
    public float StunDuration => stunDuration;
    public Vector3 KnockBack => knockback;

    public float attackRange;
    public float attackDelay;
    public float moveSpeed;
    public float rotationSpeed;

    public int specialAttackRandomRange;

    [SerializeField] protected float viewRadius; // Ž�� ����
    public float viewAngle; // Ž�� ����
    [HideInInspector] public float originViewAngle;

    [HideInInspector] public Vector3 spawnPosition; // ó�� ������ ��ġ
    public float distanceFromOriginPos; // ó�� �ִ� ��ġ�κ����� �Ÿ�
    [HideInInspector] public Vector3 spawnDir; // ó�� �ٶ� ����
    public Transform target;

    public BoxCollider attackCol; // ���� ����

    [HideInInspector] public bool isReturning; // ���ư��� ���� üũ

    [SerializeField] protected LayerMask targetMask; // Ÿ�ٷ��̾�
    [SerializeField] protected LayerMask obstacleMask; // ��ֹ����̾�

    protected NavMeshAgent agent;
    protected Animator anim;

    [HideInInspector] public State state;




    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {
        attackCol.enabled = false;
        currentHp = maxHp;
        originViewAngle = viewAngle;
        spawnPosition = transform.position;
        spawnDir = transform.forward;
        agent.speed = moveSpeed;
        agent.stoppingDistance = attackRange;
        state = State.IDLE;
        StartCoroutine(FindTargetWithDelay(.2f));
    }

    protected IEnumerator FindTargetWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    protected void FindVisibleTargets()
    {
        if (isReturning) { return; }

        if (target != null)
        {
            viewAngle = 360f;
            obstacleMask = LayerMask.NameToLayer("Nothing");
            return;
        }

        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.transform.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    this.target = target;
                }
            }
        }
    }

    public virtual void TakeDamage(float damage)
    {
        if (state == State.DEAD) { return; }

        currentHp -= damage;

        if (currentHp > 0f)
        {
            anim.SetTrigger("Hit");
            viewAngle = 360f;
            obstacleMask = LayerMask.NameToLayer("Nothing");
        }
        else
        {
            Die();
        }
    }

    protected void Die()
    {
        anim.SetTrigger("Dead");
        StopAllCoroutines();
        target = null;
        state = State.DEAD;
        Destroy(gameObject, 3f);
    }

    public void SetAttackTypeInfo(int damage, bool hitFeedback, float stunDuration, Vector3 knockback)
    {
        this.damage = damage;
        this.hitFeedback = hitFeedback;
        this.stunDuration = stunDuration;
        this.knockback = knockback;
    }

    private void SetKnockback()
    {
        hitFeedback = false;

        Vector3 knockbackDir = (target.position - transform.position).normalized;

        knockback = knockbackDir;
    }

    private void ResetKnockback()
    {
        hitFeedback = true;
        knockback = Vector3.zero;
    }
}
