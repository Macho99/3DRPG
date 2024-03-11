using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    private MonsterRace race;
    private SphereCollider col;
    private MeshRenderer meshRenderer;

    AudioSource audioSource;
    GameObject parentObj;

    private void Awake()
    {
        col = GetComponent<SphereCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
        parentObj = transform.parent.gameObject;
    }

    public void SetMonsterRace(MonsterRace race)
    {
        this.race = race;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            string itemName = GameManager.Monster.GetDropItemId(race);
            GameManager.Inven.AddItemWithAlarm(GameManager.Data.GetItem(itemName));

            col.enabled = false;
            meshRenderer.enabled = false;
            audioSource?.PlayOneShot(GameManager.Monster.GetPickupItemSound());

            Destroy(parentObj, 1f);
        }
    }
}
