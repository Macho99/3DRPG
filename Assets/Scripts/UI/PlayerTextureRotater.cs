using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerTextureRotater : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	[SerializeField] float rotateMultiplier = 1f;
	PlayerCamManager playerCamManager;

	float prevXPos;

	public void OnBeginDrag(PointerEventData eventData)
	{
		if(playerCamManager == null)
		{
			Awake();
		}

		prevXPos = eventData.position.x;
	}

	public void OnDrag(PointerEventData eventData)
	{
		float curXPos = eventData.position.x;
		playerCamManager?.RotateEquipInvPivot((curXPos - prevXPos) * rotateMultiplier);
		prevXPos = curXPos;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		playerCamManager?.RotateEquipInvPivot();
	}

	private void Awake()
	{
		playerCamManager = FieldSFC.Player.GetComponent<PlayerCamManager>();
	}
}
