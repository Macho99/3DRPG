using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamManager : MonoBehaviour
{
	[SerializeField] CinemachineVirtualCamera aimCam;
	[SerializeField] CinemachineVirtualCamera followCam;
	[SerializeField] CinemachineVirtualCamera fireRainCastCam;
	[SerializeField] CinemachineVirtualCamera bowUltiCastCam;
	[SerializeField] CinemachineVirtualCamera bowUltiTrackCam;
	[SerializeField] Transform bowUltiTrackLookPoint;

	private CinemachineTrackedDolly bowUltiTrackedDolly;

	private void Awake()
	{
		bowUltiTrackedDolly = bowUltiTrackCam.GetCinemachineComponent<CinemachineTrackedDolly>();
	}

	private void SetCam(CinemachineVirtualCamera cam, bool value)
	{
		if (value == true)
		{
			cam.Priority = 20;
		}
		else
		{
			cam.Priority = 0;
		}
	}

	public void SetAimCam(bool value)
	{
		SetCam(aimCam, value);
	}

	public void SetFireRainCastCam(bool value)
	{
		SetCam(fireRainCastCam, value);
	}

	public void SetBowUltiCastCam(bool value)
	{
		SetCam(bowUltiCastCam, value);
	}

	public void SetBowUltiTrackCam(bool value)
	{
		bowUltiTrackCam.transform.localPosition = Vector3.zero;
		if(value == false)
		{
			_ = StartCoroutine(CoInitBowUltiTrackPos());
		}
		else
		{
			SetBowUltiLookZPos(0f);
			SetBowUltiTrackPos(0f);
		}
		SetCam(bowUltiTrackCam, value);
	}

	private IEnumerator CoInitBowUltiTrackPos()
	{
		yield return new WaitForSeconds(2f);
		SetBowUltiLookZPos(0f);
		SetBowUltiTrackPos(0f);
	}

	public void SetBowUltiTrackPos(float pos)
	{
		bowUltiTrackedDolly.m_PathPosition = pos;
	}

	public void SetBowUltiLookZPos(float zPos)
	{
		Vector3 lookPos = bowUltiTrackLookPoint.localPosition;
		lookPos.z = zPos;
		bowUltiTrackLookPoint.localPosition = lookPos;
	}
}