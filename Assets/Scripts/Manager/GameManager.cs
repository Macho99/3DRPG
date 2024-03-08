using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager instance;
	private static PoolManager poolManager;
	private static ResourceManager resourceManager;
	private static UIManager uiManager;
	private static InventoryManager inventoryManager;
	private static DataManager dataManager;
	private static StatManager statManager;
    private static MonsterManager monsterManager;

    public static GameManager Instance { get { return instance; } }
	public static PoolManager Pool { get { return poolManager; } }
	public static ResourceManager Resource { get { return resourceManager; } }
	public static UIManager UI { get { return uiManager; } }
	public static InventoryManager Inven { get { return inventoryManager; } }
	public static DataManager Data { get { return dataManager; } }
	public static StatManager Stat { get { return statManager; } }
    public static MonsterManager Monster { get { return monsterManager; } }

    private void Awake()
	{
		if (instance != null)
		{
			Destroy(this);
			return;
		}

		instance = this;
		DontDestroyOnLoad(this);
		InitManagers();
	}

	private void OnDestroy()
	{
		if (instance == this)
			instance = null;
	}

	private void InitManagers()
	{
		GameObject resourceObj = new GameObject();
		resourceObj.name = "ResourceManager";
		resourceObj.transform.parent = transform;
		resourceManager = resourceObj.AddComponent<ResourceManager>();

		GameObject dataObj = new GameObject();
		dataObj.name = "DataManager";
		dataObj.transform.parent = transform;
		dataManager = dataObj.AddComponent<DataManager>();

		GameObject poolObj = new GameObject();
		poolObj.name = "PoolManager";
		poolObj.transform.parent = transform;
		poolManager = poolObj.AddComponent<PoolManager>();

		GameObject uiObj = new GameObject();
		uiObj.name = "UIManager";
		uiObj.transform.parent = transform;
		uiManager = uiObj.AddComponent<UIManager>();

        GameObject invenObj = new GameObject();
        invenObj.name = "InventoryManager";
        invenObj.transform.parent = transform;
        inventoryManager = invenObj.AddComponent<InventoryManager>();

		GameObject statObj = new GameObject();
		statObj.name = "StatManager";
		statObj.transform.parent = transform;
		statManager = statObj.AddComponent<StatManager>();

        GameObject monsterObj = new GameObject();
        monsterObj.name = "MonsterManager";
        monsterObj.transform.parent = transform;
        monsterManager = monsterObj.AddComponent<MonsterManager>();

    }
}