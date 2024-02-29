using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamManager : MonoBehaviour
{
	[SerializeField] CinemachineVirtualCamera aimCam;
	[SerializeField] CinemachineVirtualCamera followCam;

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
}