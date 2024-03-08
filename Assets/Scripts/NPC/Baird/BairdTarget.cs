using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BairdTarget : MonoBehaviour
{
    public Vector3 posOne;
    public Vector3 posTwo;
    public Vector3 posThree;
    public Vector3 posFour;

    private void Start()
    {
        transform.position = posOne;
    }

    private void Update()
    {
        if (Vector3.Distance(FindObjectOfType<Baird>().transform.position, transform.position) < 0.1f)
        {
            if(transform.position == posOne)
            {
                transform.position = posTwo;
            }
            else if(transform.position == posTwo)
            {
                transform.position = posThree;
            }
            else if(transform.position == posThree)
            {
                transform.position = posFour;
            }
            else if(transform.position == posFour)
            {
                transform.position = posOne;
            }
        }
    }
}
