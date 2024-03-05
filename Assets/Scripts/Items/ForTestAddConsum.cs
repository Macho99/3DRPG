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
            Debug.Log("Katana");
            GameManager.Inven.invenData.AbbItem(TestWeapon, 1);
            yield return new WaitForSeconds(1f);
            Debug.Log("Armor");
            GameManager.Inven.invenData.AbbItem(TestConsum, 1);
            yield return new WaitForSeconds(1f);
            Debug.Log("Apple");
            GameManager.Inven.invenData.AbbItem(TestArmor, 1);
        }
        //yield return new WaitForSeconds(3f);
        //Debug.Log("consum");
        //GameManager.Inven.TryGainConsumItem(consum);
    }
}
