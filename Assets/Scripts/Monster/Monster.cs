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

    [SerializeField] private float staminaRate; // ���¹̳� ����� ��ġ

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

    // TODO: *Patrol? *���� ���� ��üȭ, *���� variation �߰�, Hit Effect �߰�, Sound �߰�, *���Ÿ� �� �߰�
    // *BattleIdle ���¿��� �¿�� �����̴��� �ؼ� �ڿ������� ���� ( ������ �ȴٰ� ��� �¿�� �Դٰ���, �ٽ� ������ �ȱ� )
    // �ٰŸ�, �ٰŸ� ����, ���Ÿ�, �̹� ( ����? ���� ���Ÿ� ����Ī? )
    // �����̻�?

    // # ��Ȱ��ȭ �̹Ϳ� ��ȣ�ۿ� �ϸ� �̹��� Taunt �ִϸ��̼� ( �÷��̾� �Ѿ��� )
}
