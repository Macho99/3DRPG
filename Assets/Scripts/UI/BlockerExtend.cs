using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockerExtend : MonoBehaviour, IPointerClickHandler
{
	Button button;

	private void Awake()
	{
		button = GetComponent<Button>();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if(eventData.button != PointerEventData.InputButton.Left)
		{
			button.onClick.Invoke();
		}
	}

}
