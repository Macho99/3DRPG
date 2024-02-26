using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkKnight : MonoBehaviour
{
    Animator anim;
    private float speedValue;


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAttackReady(float speedValue)
    {
        anim.SetFloat("AttackSpeed", speedValue);
    }

    private void OnAttackStart(float speedValue)
    {
        anim.SetFloat("AttackSpeed", speedValue);
    }

    private void OnAttackEnd(float speedValue)
    {
        anim.SetFloat("AttackSpeed", speedValue);
    }
}
