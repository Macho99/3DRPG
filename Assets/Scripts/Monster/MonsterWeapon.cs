using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeapon : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private bool hitFeedback;
    [SerializeField] private float stunDuration;
    [SerializeField] private Vector3 knockback;

    private Monster monster;
    private DeathKnight knight;

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
            // �÷��̾� ������ �Լ� ����
            UpdateInfo();

            if (hitFeedback)
            {
                player.TakeDamage(damage, hitFeedback);
            }
            else
            {
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
            Debug.Log("No Script Error");
        }
    }
}
