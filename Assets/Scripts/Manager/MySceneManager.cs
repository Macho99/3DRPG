using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
	private Dictionary<string, GameObject> sceneDict = new();

	public void RegisterScene(string name, GameObject root)
	{
		sceneDict.Add(name, root);
	}

	public void MoveScene(string name)
	{
		foreach(var root in sceneDict.Values)
		{
			root.SetActive(false);
		}
		sceneDict[name].SetActive(true);
	}

	public bool CheckSceneLoaded(string name)
	{
		return sceneDict.ContainsKey(name);
	}
}