using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForTestAddConsum : MonoBehaviour
{
    public SOItem TestConsum;
    public SOItem TestWeapon;
    public SOItem TestArmor;

    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            GameManager.Inven.TryGainItem(TestConsum);
            yield return new WaitForSeconds(1f);
            GameManager.Inven.TryGainItem(TestWeapon);
            yield return new WaitForSeconds(1f);
            GameManager.Inven.TryGainItem(TestArmor);
        }
        //yield return new WaitForSeconds(3f);
        //Debug.Log("consum");
        //GameManager.Inven.TryGainConsumItem(consum);
    }
}
