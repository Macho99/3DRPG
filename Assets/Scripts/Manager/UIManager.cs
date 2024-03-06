using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
	private EventSystem eventSystem;
	private Canvas popUpCanvas;
	private Stack<PopUpUI> popUpStack;
	private Canvas windowCanvas;
	private Canvas inGameCanvas;
	private Canvas sceneCanvas;

	private bool menuOpened;
	[HideInInspector] public UnityEvent<bool> OnMenuToggle = new();

    private void Awake()
	{
		eventSystem = GameManager.Resource.Instantiate<EventSystem>("UI/EventSystem");
		eventSystem.transform.parent = transform;

		popUpCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
		popUpCanvas.gameObject.name = "PopUpCanvas";
		popUpCanvas.sortingOrder = 100;
		popUpStack = new Stack<PopUpUI>();

		windowCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
		windowCanvas.gameObject.name = "WindowCanvas";
		windowCanvas.sortingOrder = 10;

		sceneCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
		sceneCanvas.gameObject.name = "SceneCanvas";
		sceneCanvas.sortingOrder = 1;

		inGameCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
		inGameCanvas.gameObject.name = "InGameCanvas";
		inGameCanvas.sortingOrder = 0;

		//menu = GameManager.Resource.Instantiate<MenuUI>("UI/PopUpUI/Menu");
	}

    public T ShowPopUpUI<T>(T popUpUI) where T : PopUpUI
	{
		if (popUpStack.Count > 0)
		{
			PopUpUI prevUI = popUpStack.Peek();
			prevUI.gameObject.SetActive(false);
		}
		else
		{
			FieldSFC.Player?.IgnoreInput(true);
		}

		T ui = GameManager.Pool.GetUI<T>(popUpUI);
		ui.transform.SetParent(popUpCanvas.transform, false);
		popUpStack.Push(ui);

		_ = StartCoroutine(FadeIn(ui.GetComponent<CanvasGroup>()));
		
		return ui;
	}

	private IEnumerator FadeIn(CanvasGroup cg)
	{
		float fadeTime = 0.2f;
		float accumTime = 0f;
        while (accumTime < fadeTime)
        {
            cg.alpha = Mathf.Lerp(0f, 1f, accumTime / fadeTime);
            yield return 0;
            accumTime += Time.deltaTime;
        }
        cg.alpha = 1f;
    }

    public T ShowPopUpUI<T>(string path) where T : PopUpUI
	{
		T ui = GameManager.Resource.Load<T>(path);
		return ShowPopUpUI(ui);
	}

	public void ClosePopUpUI()
	{
		PopUpUI ui = popUpStack.Pop();
		_= StartCoroutine(FadeOut(ui.GetComponent<CanvasGroup>()));
		//GameManager.Pool.ReleaseUI(ui.gameObject);

		if (popUpStack.Count > 0)
		{
			PopUpUI curUI = popUpStack.Peek();
			curUI.gameObject.SetActive(true);
		}
		else
		{
			FieldSFC.Player?.IgnoreInput(false);
		}
	}

    private IEnumerator FadeOut(CanvasGroup cg)
    {
        float fadeTime = 0.2f;
        float accumTime = 0f;
        while (accumTime < fadeTime)
        {
            cg.alpha = Mathf.Lerp(1f, 0f, accumTime / fadeTime);
            yield return 0;
            accumTime += Time.deltaTime;
        }
        cg.alpha = 0f;
        GameManager.Pool.ReleaseUI(cg.GetComponent<PopUpUI>().gameObject);
    }

    public void ClearPopUpUI()
	{
		while (popUpStack.Count > 0)
		{
			ClosePopUpUI();
		}
	}
	
	public T ShowWindowUI<T>(T windowUI) where T : WindowUI
	{
		T ui = GameManager.Pool.GetUI(windowUI);
		ui.transform.SetParent(windowCanvas.transform, false);
		return ui;
	}

	public T ShowWindowUI<T>(string path) where T : WindowUI
	{
		T ui = GameManager.Resource.Load<T>(path);
		return ShowWindowUI(ui);
	}

	public void SelectWindowUI<T>(T windowUI) where T : WindowUI
	{
		windowUI.transform.SetAsLastSibling();
	}

	public void CloseWindowUI<T>(T windowUI) where T : WindowUI
	{
		GameManager.Pool.ReleaseUI(windowUI.gameObject);
	}

	public void ClearWindowUI()
	{
		WindowUI[] windows = windowCanvas.GetComponentsInChildren<WindowUI>();

		foreach (WindowUI windowUI in windows)
		{
			GameManager.Pool.ReleaseUI(windowUI.gameObject);
		}
	}

	public T ShowInGameUI<T>(T gameUi) where T : InGameUI
	{
		T ui = GameManager.Pool.GetUI(gameUi);
		ui.transform.SetParent(inGameCanvas.transform, false);

		return ui;
	}

	public T ShowInGameUI<T>(string path) where T : InGameUI
	{
		T ui = GameManager.Resource.Load<T>(path);
		return ShowInGameUI(ui);
	}

	public void CloseInGameUI<T>(T inGameUI) where T : InGameUI
	{
		GameManager.Pool.ReleaseUI(inGameUI.gameObject);
	}

	public void ClearInGameUI()
	{
		InGameUI[] inGames = inGameCanvas.GetComponentsInChildren<InGameUI>();

		foreach (InGameUI inGameUI in inGames)
		{
			GameManager.Pool.ReleaseUI(inGameUI.gameObject);
		}
	}

	public T ShowSceneUI<T>(T gameUi) where T : SceneUI
	{
		T ui = GameManager.Pool.GetUI(gameUi);
		ui.transform.SetParent(sceneCanvas.transform, false);

		return ui;
	}

	public T ShowSceneUI<T>(string path) where T : SceneUI
	{
		T ui = GameManager.Resource.Load<T>(path);
		return ShowSceneUI(ui);
	}

	public void CloseSceneUI<T>(T inGameUI) where T : SceneUI
	{
		GameManager.Pool.ReleaseUI(inGameUI.gameObject);
	}

	public void ClearSceneUI()
	{
		if(inGameCanvas == null) return;

		SceneUI[] inGames = inGameCanvas.GetComponentsInChildren<SceneUI>();

		foreach (SceneUI inGameUI in inGames)
		{
			GameManager.Pool.ReleaseUI(inGameUI.gameObject);
		}
	}

	public void MenuToggle()
	{
		menuOpened = !menuOpened;

		if (menuOpened == true)
		{
			ShowPopUpUI<MenuUI>("UI/PopUpUI/Menu");
		}
		else
		{
			ClearPopUpUI();
		}
		OnMenuToggle?.Invoke(menuOpened);
	}

	public void InvenFullAlarm()
	{
		throw new NotImplementedException();
	}
}