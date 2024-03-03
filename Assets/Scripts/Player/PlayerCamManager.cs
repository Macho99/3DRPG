using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamManager : MonoBehaviour
{
	[SerializeField] CinemachineVirtualCamera aimCam;
	[SerializeField] CinemachineVirtualCamera followCam;
	[SerializeField] CinemachineVirtualCamera skillCastCam;

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

	public void SetSkillCastCam(bool value)
	{
		if(value == true)
		{
			skillCastCam.Priority = 20;
		}
		else
		{
			skillCastCam.Priority = 0;
		}
	}
}