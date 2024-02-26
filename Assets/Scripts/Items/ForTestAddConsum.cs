using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForTestAddConsum : MonoBehaviour
{
    public Consum consum;

    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            Debug.Log("consum");
            GameManager.Inven.TryGainConsumItem(consum);
        }
        //yield return new WaitForSeconds(3f);
        //Debug.Log("consum");
        //GameManager.Inven.TryGainConsumItem(consum);
    }
}
