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
	[SerializeField] Transform equipInvenCamPivot;

	private CinemachineTrackedDolly bowUltiTrackedDolly;
	private bool equipInvCamRotating;

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

	public void RotateEquipInvPivot(float xRotateValue)
	{
		equipInvCamRotating = true;
		Quaternion quaternion = Quaternion.Euler(0f, xRotateValue * Time.deltaTime, 0f);
		equipInvenCamPivot.localRotation = equipInvenCamPivot.localRotation * quaternion;
	}

	public void RotateEquipInvPivot()
	{
		equipInvCamRotating = false;
		_ = StartCoroutine(CoRotate());
	}

	private IEnumerator CoRotate()
	{
		while (equipInvCamRotating == false)
		{
			equipInvenCamPivot.localRotation = Quaternion.Lerp(equipInvenCamPivot.localRotation, 
				Quaternion.identity, Time.deltaTime * 10f);
			if(Mathf.Abs(equipInvenCamPivot.localRotation.eulerAngles.y) < 0.5f)
			{
				equipInvenCamPivot.localRotation = Quaternion.identity;
				break;
			}
				
			yield return null;
		}
	}
}