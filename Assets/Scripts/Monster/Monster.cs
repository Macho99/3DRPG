using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Android;

public enum State
{
    IDLE,
    BLOCK,
    GUARD_BREAK,
    DEAD
}

public class Monster : MonoBehaviour
{
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float damage;
    [SerializeField] private float maxStamina;
    public float currentStamina;
    public float attackRange;
    public float attackDelay;
    public float moveSpeed;
    public float rotationSpeed;

    public int specialAttackRange;

    [SerializeField] private float viewRadius; // 탐지 범위
    public float viewAngle; // 탐지 각도
    public float originViewAngle;

    public Vector3 spawnPosition; // 처음 스폰된 위치
    public float distanceFromOriginPos; // 처음 있던 위치로부터의 거리
    public Vector3 spawnDir; // 처음 바라본 방향
    public Transform target;

    public bool isReturning; // 돌아가는 상태 체크

    [SerializeField] private LayerMask targetMask; // 타겟레이어
    [SerializeField] private LayerMask obstacleMask; // 장애물레이어

    NavMeshAgent agent;
    Animator anim;

    public State state;

    [SerializeField] private float staminaRate; // 스태미너 재생성 수치

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        currentHp = maxHp;
        currentStamina = maxStamina;
        originViewAngle = viewAngle;
        spawnPosition = transform.position;
        spawnDir = transform.forward;
        agent.speed = moveSpeed;
        agent.stoppingDistance = attackRange;
        state = State.IDLE;
        StartCoroutine(FindTargetWithDelay(.2f));
    }

    private void Update()
    {
        if (state == State.IDLE)
        {
            currentStamina = Mathf.Clamp(currentStamina + staminaRate * Time.deltaTime, 0, maxStamina);
        }
    }

    IEnumerator FindTargetWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    private void FindVisibleTargets()
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

    public void TakeDamage(float damage)
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

    private void Die()
    {
        anim.SetTrigger("Dead");
        StopAllCoroutines();
        target = null;
        state = State.DEAD;
        Destroy(gameObject, 3f);
    }
}
