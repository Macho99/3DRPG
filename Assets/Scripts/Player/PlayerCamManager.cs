using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamManager : MonoBehaviour
{
	[SerializeField] CinemachineVirtualCamera aimCam;
	[SerializeField] CinemachineVirtualCamera followCam;
	[SerializeField] CinemachineVirtualCamera fireRainCastCam;
	[SerializeField] CinemachineVirtualCamera bowUltiCastCam;

	public void SetAimCam(bool value)
	{
		if(value == true)
		{
			aimCam.Priority = 20;
		}
		else
		{
			aimCam.Priority = 0;
		}
	}

	public void SetFireRainCastCam(bool value)
	{
		if(value == true)
		{
			fireRainCastCam.Priority = 20;
		}
		else
		{
			fireRainCastCam.Priority = 0;
		}
	}
	public void SetBowUltiCastCam(bool value)
	{
		//print(value);
		if (value == true)
		{
			bowUltiCastCam.Priority = 20;
		}
		else
		{
			bowUltiCastCam.Priority = 0;
		}
	}
}