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
    private bool hitFeedback;
    private float stunDuration;
    private Vector3 knockback;

    public int Damage => damage;
    public bool HitFeedBack => hitFeedback;
    public float StunDuration => stunDuration;
    public Vector3 KnockBack => knockback;

    public float attackRange;
    public float attackDelay;
    public float walkSpeed;
    public float moveSpeed;
    public float rotationSpeed;
    [SerializeField] private float eyeHeight; 

    public int specialAttackRandomRange;

    [SerializeField] protected float viewRadius; // 탐지 범위
    public float viewAngle; // 탐지 각도
    [HideInInspector] public float originViewAngle;

    [HideInInspector] public Vector3 spawnPosition; // 처음 스폰된 위치
    public float distanceFromOriginPos; // 처음 있던 위치로부터의 거리
    [HideInInspector] public Vector3 spawnDir; // 처음 바라본 방향
    public Transform target;

    public BoxCollider attackCol; // 어택 범위

    [HideInInspector] public bool isReturning; // 돌아가는 상태 체크

    [SerializeField] protected LayerMask targetMask; // 타겟레이어
    [SerializeField] protected LayerMask obstacleMask; // 장애물레이어

    [SerializeField] List<GameObject> hitEffects = new List<GameObject>();
    [SerializeField] List<AudioClip> hitSounds = new();

    AudioSource audioSource;
    protected NavMeshAgent agent;
    protected Animator anim;

    [HideInInspector] public State state;

    public List<Transform> wayPoints;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {
        hitFeedback = true;
        attackCol.enabled = false;
        currentHp = maxHp;
        originViewAngle = viewAngle;
        spawnPosition = transform.position;
        spawnDir = transform.forward;
        agent.speed = moveSpeed;
        //agent.stoppingDistance = attackRange;
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

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, viewRadius);

    //    if (target != null)
    //    {
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawLine(transform.position, target.position);
    //    }

    //    Gizmos.color = Color.red;

    //    int segments = 36;
    //    float step = viewAngle / segments;
    //    for (int i = 0; i < segments; i++)
    //    {
    //        float angle = i * step - viewAngle / 2;
    //        float x = Mathf.Sin(Mathf.Deg2Rad * angle) * viewRadius;
    //        float z = Mathf.Cos(Mathf.Deg2Rad * angle) * viewRadius;
    //        Vector3 start = transform.position + Vector3.up * eyeHeight;
    //        Vector3 dir = transform.TransformDirection(new Vector3(x, 0, z));
    //        //Gizmos.DrawLine(transform.position, transform.position + dir);

    //        RaycastHit hit;
    //        if (Physics.Raycast(start, dir, out hit, viewRadius, obstacleMask))
    //        {
    //            Gizmos.DrawLine(start, hit.point);
    //        }
    //        else
    //        {
    //            Gizmos.DrawLine(start, transform.position + dir);
    //        }
    //    }
    //}

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

                if (!Physics.Raycast(transform.position + Vector3.up * eyeHeight, dirToTarget, distToTarget, obstacleMask))
                {
                    this.target = target;
                }
            }
        }
    }

    public virtual void TakeDamage(float damage)
    {
        if (state == State.DEAD || isReturning) { return; }

        currentHp -= damage;

        if (currentHp > 0f)
        {
            anim.SetTrigger("Hit");
            viewAngle = 360f;
            obstacleMask = LayerMask.NameToLayer("Nothing");

            //audioSource?.PlayOneShot(hitSounds[0]);
            //Quaternion effectRot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            //GameObject hitEffectPrefab = Instantiate(hitEffects[0], transform.position, effectRot);
            //Destroy(hitEffectPrefab, 1f);
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
        //audioSource?.PlayOneShot(hitSounds[1]);
        Destroy(gameObject, 3f);
    }

    public void RecovereryHp()
    {
        currentHp = maxHp;
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
