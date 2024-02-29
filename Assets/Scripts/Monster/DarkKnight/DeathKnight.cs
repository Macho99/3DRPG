using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BossState
{
    NORMAL,
    TWOHANDED,
    DEAD
}

public class DeathKnight : MonoBehaviour
{
    private float speedValue; // 애니메이션 속도 수치

    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float damage;
    public float meleeAttackRange;
    public float skillAttackRange;
    public float attackDelay;
    public float moveSpeed;
    public float rotationSpeed;

    private bool nearBy;

    public Transform target;

    NavMeshAgent agent;
    Animator anim;
    Rigidbody rb;

    public BossState bossState;

    public Avatar avatar;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        currentHp = maxHp;
        agent.speed = moveSpeed;
        agent.stoppingDistance = skillAttackRange;
        bossState = BossState.NORMAL;
    }

    public void TakeDamage(float damage)
    {
        if (bossState == BossState.DEAD) { return; }

        currentHp -= damage;

        if (currentHp > 0f)
        {
            //anim.SetTrigger("Hit");
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        anim.SetTrigger("Dead");
        target = null;
        bossState = BossState.DEAD;
        Destroy(gameObject, 3f);
    }

    private void SetAttackSpeed(float speedValue)
    {
        anim.SetFloat("AttackSpeed", speedValue);
    }

    private void OnMoveForward(float moveSpeed)
    {
        if (Vector3.Distance(transform.position, target.position) > agent.stoppingDistance)
        {
            Vector3 moveDir = new Vector3(transform.forward.x, 0, transform.forward.z);
            rb.velocity = moveSpeed * moveDir;
        }
    }

    private void agentCancel()
    {
        agent.enabled = false;
    }

    private void OnMoveUp(float height)
    {
        rb.AddForce(transform.up * height, ForceMode.Impulse);
    }

    private void OnMoveDown(float height)
    {
        rb.AddForce(Vector3.down * height, ForceMode.Impulse);
    }

    private void OnLanding()
    {
        agent.enabled = true;
    }

    private void OnMoveStop()
    {
        rb.velocity = Vector3.zero;
    }

    private void StopAttackTmp(float time)
    {
        anim.SetFloat("AttackSpeed", 0.05f);

        Invoke("ReStartMotion", time);
    }

    private void ReStartMotion()
    {
        anim.SetFloat("AttackSpeed", 1f);
    }

    public void ChangeAvatar()
    {
        bossState = BossState.TWOHANDED;
        anim.avatar = this.avatar;
    }
}
