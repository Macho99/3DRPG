using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
	EquipItem[] equipItems;
	ConsumpItem[] consumpItems;
	OtherItem[] otherItems;

	const int invSize = 20;

	private void Awake()
	{
		equipItems = new EquipItem[invSize];
		consumpItems = new ConsumpItem[invSize];
		otherItems = new OtherItem[invSize];
	}
}
