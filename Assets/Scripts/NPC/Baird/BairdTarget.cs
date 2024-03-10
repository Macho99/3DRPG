using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BairdTarget : MonoBehaviour
{
    public Vector3 posOne;
    public Vector3 posTwo;
    public Vector3 posThree;
    public Vector3 posFour;

    public Vector3 PosOne => posOne + offset;
	public Vector3 PosTwo => posTwo + offset;
	public Vector3 PosThree => posThree + offset;
	public Vector3 PosFour => posFour + offset;
	public Vector3 offset;

    private void Start()
    {
        transform.position = PosOne;
    }

    private void Update()
    {
        if (Vector3.Distance(FindObjectOfType<Baird>().transform.position, transform.position) < 0.1f)
        {
            if(transform.position == PosOne)
            {
                transform.position = PosTwo;
            }
            else if(transform.position == PosTwo)
            {
                transform.position = PosThree;
            }
            else if(transform.position == PosThree)
            {
                transform.position = PosFour;
            }
            else if(transform.position == PosFour)
            {
                transform.position = PosOne;
            }
        }
    }
}
