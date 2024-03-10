using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum BossState
{
    NORMAL,
    TWOHANDED,
    DEAD
}

public class DeathKnight : MonoBehaviour
{
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    public float hPRatio { get { return (float)currentHp / maxHp; } }
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

    public BoxCollider attackCol; // 어택 범위

    MonsterRace race;
    AudioSource audioSource;
    NavMeshAgent agent;
    Animator anim;
    Rigidbody rb;

    public BossState bossState;

    [SerializeField] private GameObject myWeapon;
    [SerializeField] private Material mySwordMaterial;
    [SerializeField] private GameObject swordEffect;
    [SerializeField] private EnterBossRoom room;

    [HideInInspector] public bool skillCD;
    [HideInInspector] public float skillCoolDown;

    [HideInInspector] public UnityEvent<float> OnBossHPChange = new();
    [HideInInspector] public UnityEvent OnBossDie = new();

    private void Awake()
    {
        room = FindObjectOfType<EnterBossRoom>();
        race = MonsterRace.Boss;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        hitFeedback = true;
        stunDuration = 1f;
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

        //currentHp -= damage;
        SubCurHP((int)damage);

        if (currentHp <= maxHp / 2)
        {
            // 페이지 전환 시점
            anim.SetBool("ChangePhase", true);
        }

        if (currentHp > 0f)
        {
            // 피격 시
            audioSource?.PlayOneShot(SetSound("Hit"));
            Quaternion effectRot = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
            GameObject hitEffectPrefab = Instantiate(SetEffect("Hit"), transform.position, effectRot);
            Destroy(hitEffectPrefab, 1f);
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        audioSource?.PlayOneShot(SetSound("Dead"));
        agent.isStopped = true;
        rb.velocity = Vector3.zero;
        anim.applyRootMotion = true;
        anim.SetTrigger("Dead");
        DropItem();
        room.BlockOnOff();
        target = null;
        bossState = BossState.DEAD;
        Invoke("ReleaseBossUI", 3f);
        Destroy(gameObject, 4f);
    }

    private void ReleaseBossUI()
    {
        GameManager.UI.ClearBossUI();
        GameManager.UI.CloseBossUI(GameManager.UI.SceneCanvas.transform.Find("BossUI").GetComponent<BossUI>());
    }

    private void DropItem()
    {
        GameObject obj = GameManager.Monster.GetItemObj();
        GameObject item = Instantiate(obj, transform.position + Vector3.up * 1f, Quaternion.identity);
        item.GetComponentInChildren<DropItem>()?.SetMonsterRace(race);
    }

    public void AddCurHP(int amount)
    {
        currentHp += amount;
        if (currentHp > maxHp)
            currentHp = maxHp;
        OnBossHPChange?.Invoke(hPRatio);
    }

    public void SubCurHP(int amount)
    {
        currentHp -= amount;
        OnBossHPChange?.Invoke(hPRatio);
        if (currentHp <= 0)
        {
            currentHp = 0;
            OnBossDie?.Invoke();
        }
    }

    private AudioClip SetSound(string soundName)
    {
        return GameManager.Monster.GetBossSound(soundName);
    }

    private GameObject SetEffect(string soundName)
    {
        return GameManager.Monster.GetBossEffect(soundName);
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
            //agent.Move(moveDir * moveSpeed);
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
        //agent.SetDestination(transform.position);
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

    public void ChangeWeapon()
    {
        bossState = BossState.TWOHANDED;
        myWeapon.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        myWeapon.transform.GetChild(0).GetComponent<MeshRenderer>().material = mySwordMaterial;
        swordEffect.SetActive(true);
    }

    private void OnOffWeaponColl()
    {
        attackCol.enabled = !attackCol.enabled;
    }

    private void SetRootMotion()
    {
        anim.applyRootMotion = true;
        anim.SetTrigger("isReady");
    }

    private void DeSetRootMotion()
    {
        anim.applyRootMotion = false;
        anim.SetTrigger("isReady");
    }

    private void NormalSwingSound()
    {
        audioSource?.PlayOneShot(SetSound("Attack"));
    }

    private void TwoHandedSwingSound()
    {
        audioSource?.PlayOneShot(SetSound("TwoHandedAttack"));
    }

    private void TwoHandedSkill2Sound()
    {
        audioSource?.PlayOneShot(GameManager.Monster.GetBossSkillSound("TwoHandedSkill2"));
    }

    private void TwoHandedSkill4_2Sound()
    {
        audioSource?.PlayOneShot(GameManager.Monster.GetBossSkillSound("TwoHandedSkill4_2"));
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
