using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager
{
    public static int order = 0;
    Stack<GameObject> _popupStack = new Stack<GameObject>();
    Stack<GameObject> _firstSelected = new Stack<GameObject>();

    public static GameObject root;

    public GameObject Root { get { return root; } }
    
    public static GameObject sceneUI;
    
    public GameObject SceneUI { get { return sceneUI; } }

    public void Init()
    {
        root = GameObject.Find("@UI_Root");
        if (root == null)
        {
            root = new GameObject { name = "@UI_Root" };
            Canvas canvas = Util.GetOrAddComponent<Canvas>(root);
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler canvasScaler = Util.GetOrAddComponent<CanvasScaler>(root);
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            root.AddComponent<GraphicRaycaster>();
        }
    }

    public void LoadScenePanel<T>(string panelName, Transform transform = null) where T : Component
    {
        if (sceneUI != null)
        {
            Managers.Resource.Destroy(sceneUI);
        }

        sceneUI = Managers.Resource.Instantiate($"UI/Panel/{panelName}", root.transform);
        sceneUI.AddComponent<T>();
    }

    public GameObject LoadPopupPanel(string panelName)
    {
        GameObject panel = Managers.Resource.Instantiate($"UI/Panel/Panel",root.transform);
        GameObject popup = Managers.Resource.Instantiate($"UI/Popup/{panelName}",root.transform);
        popup.transform.SetParent(panel.transform);
        _popupStack.Push(panel);
        return popup;
    }

    public void CloseTopPopup()
    {
        if (_popupStack.Count == 0)
            return;
        Managers.Resource.Destroy(_popupStack.Pop().gameObject);
        
    }

    public bool PopupActive()
    {
        return (_popupStack.Count > 0);
    }

    public void SetEventSystemNavigation(GameObject go)
    {
        EventSystem.current.firstSelectedGameObject = go;
    }
    /*
    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : MonoBehaviour
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject go = Managers.Resource.Instantiate($"UI/subitem/{name}");
        if (parent != null)
        {
            go.transform.SetParent(parent);
        }

        return Util.GetOrAddComponent<T>(go);
    }
    
	public T ShowSceneUI<T>(string name = null) where T : UI_Scene
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
		T sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;

		go.transform.SetParent(Root.transform);

		return sceneUI;
	}

	public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Util.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        go.transform.SetParent(Root.transform);

		return popup;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
		if (_popupStack.Count == 0)
			return;

        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;
        _order--;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    public void Clear()
    {
        CloseAllPopupUI();
        _sceneUI = null;
    }
    */
}