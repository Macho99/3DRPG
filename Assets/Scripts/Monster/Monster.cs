using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum State
{
    IDLE,
    MOVE,
    ATTACK,
    BLOCK,
    DEAD
}

public class Monster : MonoBehaviour
{
    [SerializeField] private int maxHp;
    [SerializeField] private int currentHp;
    [SerializeField] private int damage;
    public float attackRange;
    [SerializeField] private float attackDelay;
    public float moveSpeed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float viewRadius; // Ž�� ����
    public float viewAngle; // Ž�� ����
    public float originViewAngle;

    public Vector3 spawnPosition; // ó�� ������ ��ġ
    public float distanceFromOriginPos; // ó�� �ִ� ��ġ�κ����� �Ÿ�
    public Transform target;

    public bool isReturning; // ���ư��� ���� üũ

    [SerializeField] private LayerMask targetMask; // Ÿ�ٷ��̾�
    [SerializeField] private LayerMask obstacleMask; // ��ֹ����̾�

    NavMeshAgent agent;
    Animator anim;

    public State state;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        currentHp = maxHp;
        originViewAngle = viewAngle;
        spawnPosition = transform.position;
        agent.speed = moveSpeed;
        agent.stoppingDistance = attackRange;
        state = State.IDLE;
        StartCoroutine(FindTargetWithDelay(.2f));
    }


    private void Move()
    {
        if (state == State.ATTACK) { return; }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            StartCoroutine(Attack());
            anim.SetBool("Move", false);
        }
        else if (target == null)
        {
            state = State.IDLE;
            agent.SetDestination(transform.position);
            anim.SetBool("Move", false);
        }
        else
        {
            StopCoroutine(Attack());
            agent.SetDestination(target.position);
            anim.SetBool("Move", true);
            state = State.MOVE;
        }
    }

    IEnumerator Attack()
    {
        state = State.ATTACK;

        while (agent.remainingDistance <= agent.stoppingDistance && target != null)
        {
            if (state == State.DEAD) { yield break; }

            transform.LookAt(target.position);

            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            anim.SetTrigger("Attack");
            // �÷��̾� Attack
            print("Attack");

            yield return new WaitForSeconds(2f);
        }
        print("1");
        agent.isStopped = false;
        state = State.MOVE;
    }

    IEnumerator Turn(Vector3 target)
    {
        while (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(target - transform.position)) > .1f)
        {
            Vector3 directionToTarget = target - transform.position;
            directionToTarget.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            yield return null;
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
                    //agent.SetDestination(target.position);
                }
            }
        }
    }

    private void Update()
    {
        //if (target != null && state != State.DEAD)
        //{
        //    Move();

        //    CheckDistance();
        //}
    }

    private void CheckDistance()
    {
        if (Vector3.Distance(transform.position, spawnPosition) > distanceFromOriginPos)
        {
            if (!isReturning)
            {
                target = null;
                StartCoroutine(ReturnToOriginPos());
            }
        }
        else
        {
            StopCoroutine(ReturnToOriginPos());
            isReturning = false;
        }
    }

    IEnumerator ReturnToOriginPos()
    {
        isReturning = true;
        viewAngle = originViewAngle;
        obstacleMask = LayerMask.NameToLayer("Structure");

        while (Vector3.Distance(transform.position, spawnPosition) > .5f)
        {
            agent.SetDestination(spawnPosition);
            agent.stoppingDistance = 0f;
            currentHp = maxHp;
            yield return null;
        }

        isReturning = false;
        agent.stoppingDistance = attackRange;
    }

    public void TakeDamage(int damage)
    {
        if (state == State.DEAD) { return; }


        if (currentHp > 0f)
        {
            currentHp -= damage;
            anim.SetTrigger("Hit");
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        anim.SetTrigger("Die");
        StopAllCoroutines();
        state = State.DEAD;
        Destroy(gameObject, 3f);
    }
}
