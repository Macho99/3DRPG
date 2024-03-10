using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBossRoom : MonoBehaviour
{
    public DeathKnight knight;
    public GameObject blockObj;

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            knight.target = player.transform;
            BlockOn();
            GameManager.UI.ShowBossUI<BossUI>("UI/BossUI/BossUI");
            GetComponent<SphereCollider>().enabled = false;
        }
    }

    private void BlockOn()
    {
        blockObj.GetComponent<BoxCollider>().enabled = true;
        blockObj.GetComponent<MeshRenderer>().enabled = true;
    }
}
