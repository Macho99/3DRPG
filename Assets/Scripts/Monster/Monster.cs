using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum State
{
    IDLE,
    BLOCK,
    GUARD_BREAK,
    DEAD
}

public enum MonsterRace
{
    Orc,
    Skeleton,
    TargetDummy,
    Mimic,
    Boss
}

public class Monster : MonoBehaviour
{
    [SerializeField] protected float maxHp;
    [SerializeField] protected float currentHp;
    [SerializeField] protected int damage;
    public float hPRatio { get { return (float)currentHp / maxHp; } }
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

    /*[HideInInspector]*/ public bool isReturning; // 돌아가는 상태 체크

    [SerializeField] protected LayerMask targetMask; // 타겟레이어
    [SerializeField] public LayerMask obstacleMask; // 장애물레이어

    protected AudioSource audioSource;
    protected NavMeshAgent agent;
    protected Animator anim;

    public State state;
    public MonsterRace race;

    public List<Transform> wayPoints;
    protected Canvas canvas;

    [HideInInspector] public UnityEvent<float> OnMonsterHPChange = new();
    [HideInInspector] public UnityEvent OnMonsterDie = new();


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        canvas = transform.Find("Canvas").GetComponent<Canvas>();
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
            obstacleMask = LayerMask.GetMask("Nothing");
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
        SubCurHP((int)damage);

        if (canvas.enabled == false)
        {
            canvas.enabled = true;
        }

        if (currentHp > 0f)
        {
            anim.SetTrigger("Hit");
            viewAngle = 360f;
            obstacleMask = LayerMask.GetMask("Nothing");

            audioSource?.PlayOneShot(SetSound(race, "Hit"));

            if (target == null)
            {
                Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, 30, targetMask);

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

    protected void Die()
    {
        GetComponent<Collider>().enabled = false;
        DropItem();
        anim.SetTrigger("Dead");
        StopAllCoroutines();
        target = null;
        state = State.DEAD;
        audioSource?.PlayOneShot(SetSound(race, "Hit"));
        audioSource?.PlayOneShot(SetSound(race, "Dead"));
        
        Destroy(gameObject, 3f);
    }

    private void DropItem()
    {
        GameObject obj = GameManager.Monster.GetItemObj();
        GameObject item = Instantiate(obj, transform.position + Vector3.up * 1f, Quaternion.identity);
        item.GetComponentInChildren<DropItem>()?.SetMonsterRace(race);
    }

    public void SubCurHP(int amount)
    {
        currentHp -= amount;
        OnMonsterHPChange?.Invoke(hPRatio);
        if (currentHp <= 0)
        {
            currentHp = 0;
            OnMonsterDie?.Invoke();
        }
    }

    protected AudioClip SetSound(MonsterRace race, string soundName)
    {
        return GameManager.Monster.GetMonsterSound(race, soundName);
    }

    protected GameObject SetEffect(MonsterRace race, string soundName)
    {
        return GameManager.Monster.GetMonsterEffect(race, soundName);
    }

    public void RecovereryHp()
    {
        currentHp = maxHp;
        SubCurHP(0);
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

    protected void PlayAttackSound()
    {
        audioSource?.PlayOneShot(SetSound(race, "Attack"));
    }

    private void PlayOpenSound()
    {
        audioSource?.PlayOneShot(SetSound(race, "Open"));
    }

    private void PlayShotSound()
    {
        audioSource?.PlayOneShot(SetSound(race, "RangedAttack"));
    }
}
