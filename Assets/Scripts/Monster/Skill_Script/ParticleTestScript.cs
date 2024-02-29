using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTestScript : MonoBehaviour
{
    private ParticleSystem particle;

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("Particle collided with: " + other.name);
    }
    //void OnParticleCollision()
    //{
    //    Debug.Log("Particle collided with: ");
    //}

    private void OnParticleTrigger()
    {
        Debug.Log("1");
    }
}
