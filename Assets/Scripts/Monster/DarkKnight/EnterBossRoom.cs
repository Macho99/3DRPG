using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBossRoom : MonoBehaviour
{
    public DeathKnight knight;
    public GameObject blockObj;
    AudioSource audioSource;
    int checkNum;

    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            knight.target = player.transform;
            BlockOnOff();
            GameManager.UI.ShowBossUI<BossUI>("UI/BossUI/BossUI");
            GetComponent<SphereCollider>().enabled = false;
            audioSource.Play();
        }
    }

    public void BlockOnOff()
    {
        checkNum++;

        BoxCollider blockCol = blockObj.GetComponent<BoxCollider>();
        MeshRenderer blockRenderer = blockObj.GetComponent<MeshRenderer>();

        blockCol.enabled = !blockCol.enabled;
        blockRenderer.enabled = !blockRenderer.enabled;

        if (checkNum > 1)
        {
            audioSource.Stop();
        }
    }
}
