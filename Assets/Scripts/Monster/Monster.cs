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

    [SerializeField] private float viewRadius; // 탐지 범위
    public float viewAngle; // 탐지 각도
    public float originViewAngle;

    public Vector3 spawnPosition; // 처음 스폰된 위치
    public float distanceFromOriginPos; // 처음 있던 위치로부터의 거리
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

    //IEnumerator Turn(Vector3 target)
    //{
    //    while (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(target - transform.position)) > .1f)
    //    {
    //        Vector3 directionToTarget = target - transform.position;
    //        directionToTarget.y = 0;
    //        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget.normalized);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    //        yield return null;
    //    }

    //}

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

    // TODO: *Patrol? *어택 판정 구체화, *어택 variation 추가, Hit Effect 추가, Sound 추가, *원거리 몹 추가
    // *BattleIdle 상태에서 좌우로 움직이던지 해서 자연스럽게 변경 ( 앞으로 걷다가 잠깐 좌우로 왔다갔다, 다시 앞으로 걷기 )
    // 근거리, 근거리 방패, 원거리, 미믹 ( 버퍼? 근접 원거리 스위칭? )
    // 상태이상?

    // # 비활성화 미믹에 상호작용 하면 미믹은 Taunt 애니메이션 ( 플레이어 넘어짐 )
}
