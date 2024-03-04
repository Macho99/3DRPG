using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollowHead : MonoBehaviour
{
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        var head = player.transform.Find("PT_Head");
        transform.parent = head;
        transform.localPosition = head.position;
    }
}
