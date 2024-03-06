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
    private float speedValue; // �ִϸ��̼� �ӵ� ��ġ

    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] protected int damage;
    [SerializeField] private bool hitFeedback;
    [SerializeField] private float stunDuration;
    [SerializeField] private Vector3 knockback;
    public int Damage => damage;
    public bool HitFeedBack => hitFeedback;
    public float StunDuration => stunDuration;
    public Vector3 KnockBack => knockback;

    public float meleeAttackRange;
    public float skillAttackRange;
    public float attackDelay;
    public float moveSpeed;
    public float rotationSpeed;

    public Transform target;

    public BoxCollider attackCol; // ���� ����

    NavMeshAgent agent;
    Animator anim;
    Rigidbody rb;

    public BossState bossState;

    //[SerializeField] private Avatar avatar;

    [SerializeField] private GameObject myWeapon;
    [SerializeField] private Material mySwordMaterial;
    [SerializeField] private GameObject swordEffect;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        hitFeedback = true;
    }

    private void Start()
    {
        swordEffect.SetActive(false);
        attackCol.enabled = false;
        currentHp = maxHp;
        agent.speed = moveSpeed;
        agent.stoppingDistance = skillAttackRange;
        bossState = BossState.NORMAL;
    }

    public void TakeDamage(float damage)
    {
        if (bossState == BossState.DEAD) { return; }

        currentHp -= damage;

        if (currentHp <= maxHp / 2)
        {
            // ������ ��ȯ ����
            anim.SetBool("ChangePhase", true);
        }

        if (currentHp > 0f)
        {
            // �ǰ� �ִϸ��̼� ( �ʿ� ���� )
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

    //public void ChangeAvatar()
    //{
    //    bossState = BossState.TWOHANDED;
    //    //ChangeWeapon();
    //    anim.avatar = this.avatar;
    //}

    public void ChangeWeapon()
    {
        bossState = BossState.TWOHANDED;
        myWeapon.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        myWeapon.transform.GetChild(0).GetComponent<MeshRenderer>().material = mySwordMaterial;
        swordEffect.SetActive(true);
    }
}
