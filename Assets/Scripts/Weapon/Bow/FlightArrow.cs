using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class FlightArrow : MonoBehaviour
{
	ParticleSystem particle;

	private void Awake()
	{
		particle = GetComponentInChildren<ParticleSystem>();
	}

	public void ParticlePlay()
	{
		particle.Play();
	}
}