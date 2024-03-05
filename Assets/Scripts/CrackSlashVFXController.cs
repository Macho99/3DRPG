using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackSlashVFXController : VFXAutoOff
{
	[SerializeField] ParticleSystem lineParticle;

	ParticleSystem.MainModule mainModule;

	protected override void Awake()
	{
		base.Awake();
		mainModule = lineParticle.main;
	}

	public void Init(float lineDuration = 1f)
	{
		mainModule.startLifetime = lineDuration;
		SetOffTime(lineDuration);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		mainModule.startLifetime = 1f;
	}
}