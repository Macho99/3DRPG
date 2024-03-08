using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int damage;

    Rigidbody rb;
    AudioSource audioSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject prefab = Instantiate(GameManager.Monster.GetMonsterEffect(MonsterRace.TargetDummy, "ExplosionCannon"), transform.position, Quaternion.identity);
        audioSource?.PlayOneShot(GameManager.Monster.GetMonsterSound(MonsterRace.TargetDummy, "ExplosionCannon"));
        Destroy(prefab, 2f);

        if (other.TryGetComponent(out Player player))
        {
            // 플레이어 데미지 함수 실행
            //player.TakeDamage(damage, true);
            try
            {
                // 플레이어 데미지 함수 실행
                player.TakeDamage(damage, true);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error in TakeDamage: " + ex.Message);
            }
        }
        gameObject.GetComponent<SphereCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;

        Destroy(gameObject, 3f);
    }
}
