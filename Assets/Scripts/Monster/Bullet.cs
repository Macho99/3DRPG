using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int damage;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            // �÷��̾� ������ �Լ� ����
            //player.TakeDamage(damage, true);
            try
            {
                // �÷��̾� ������ �Լ� ����
                player.TakeDamage(damage, true);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error in TakeDamage: " + ex.Message);
            }
        }

        Destroy(gameObject);
    }
}
