using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxTimer : MonoBehaviour
{
    Collider skillCol;
    float timer;

    private void Awake()
    {
        skillCol = GetComponent<Collider>();
    }

    private void Start()
    {
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > .5f)
        {
            skillCol.enabled = false;
        }
    }


}
