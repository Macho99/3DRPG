using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollowHead : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1.6f ,player.transform.position.z);
    }
}
