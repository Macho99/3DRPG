using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeapon : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private bool hitFeedback;
    [SerializeField] private float stunDuration;
    [SerializeField] private Vector3 knockback;

    [SerializeField] private Monster monster;
    [SerializeField] private DeathKnight knight;

    public Transform knightTf;

    private void Awake()
    {
        monster = GetComponentInParent<Monster>();
        knight = GetComponentInParent<DeathKnight>();
    }

    private void Start()
    {
        UpdateInfo();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (player.CurState == Player.State.Stun || player.CurState == Player.State.Dodge)
            {
                return;
            }

            // 플레이어 데미지 함수 실행
            UpdateInfo();

            if (hitFeedback)
            {
                player.TakeDamage(damage, hitFeedback);
            }
            else
            {
                if (knightTf != null)
                {
                    Vector3 knockbackDir = (player.transform.position - knightTf.position).normalized;
                    knockback = knockbackDir;
                }
                else if (monster == null && knight == null)
                {
                    CalcKnockbackDir(player.transform);
                }

                player.TakeDamage(damage, hitFeedback, stunDuration, knockback);
            }
        }
    }

    private void UpdateInfo()
    {
        if (monster != null && knight == null)
        {
            damage = monster.Damage;
            hitFeedback = monster.HitFeedBack;
            stunDuration = monster.StunDuration;
            knockback = monster.KnockBack;
        }
        else if (monster == null && knight != null)
        {
            damage = knight.Damage;
            hitFeedback = knight.HitFeedBack;
            stunDuration = knight.StunDuration;
            knockback = knight.KnockBack;
        }
        else
        {
            // Skill or Throwable Object
        }
    }

    private void CalcKnockbackDir(Transform target)
    {
        if (!hitFeedback)
        {
            Vector3 knockbackDir = (target.position - transform.position).normalized;
            knockback = knockbackDir;
        }
    }
}
