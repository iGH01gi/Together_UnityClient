using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class UIManager
{
    public static int order = 0;
    LinkedList<GameObject> _popupLinkedList = new LinkedList<GameObject>();
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
            Object.DontDestroyOnLoad(root);
            Canvas canvas = Util.GetOrAddComponent<Canvas>(root);
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler canvasScaler = Util.GetOrAddComponent<CanvasScaler>(root);
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            root.AddComponent<GraphicRaycaster>();
        }
    }
    
    public void LoadScenePanel(string sceneUIType)
    {
        if (sceneUI != null)
        {
            Managers.Resource.Destroy(sceneUI);
        }

        sceneUI = Managers.Resource.Instantiate($"UI/Scene/{sceneUIType}", root.transform);
        sceneUI.AddComponent(Type.GetType(sceneUIType));
    }

    public T LoadPopupPanel<T>(bool isBase = false,bool popupIteractableOnly = true) where T: UI_popup
    {
        GameObject popup;
        
        if (popupIteractableOnly)
        {
            _popupLinkedList.AddLast(Managers.Resource.Instantiate($"UI/Subitem/Panel", root.transform));
        }
        
        if (isBase)
        {
            popup =  Managers.Resource.Instantiate($"UI/Popup/{typeof(T)}",root.transform);
        }
        else
        {
            popup = Managers.Resource.Instantiate($"UI/Popup/{typeof(T).BaseType}",root.transform);
        }
        
        _popupLinkedList.AddLast(popup);
            
        popup.AddComponent(typeof(T));
        return popup.GetComponent<T>();
    }

    public void ClosePopup(GameObject gameObject = null)
    {
        if (!PopupActive())
            return;
        if (gameObject == null)
        {
            Managers.Resource.Destroy(_popupLinkedList.Last.Value);
            _popupLinkedList.RemoveLast();
            if (_popupLinkedList.Last.Value.name == "Panel")
            {
                Managers.Resource.Destroy(_popupLinkedList.Last.Value);
                _popupLinkedList.RemoveLast();
            }
        }
        else
        {
            var cur = _popupLinkedList.Find(gameObject);
            if (cur.Previous.Value.name == "Panel")
            {
                Managers.Resource.Destroy(cur.Previous.Value);
                _popupLinkedList.Remove(cur.Previous);
            }
            Managers.Resource.Destroy(_popupLinkedList.Find(gameObject).Value);
            _popupLinkedList.Remove(cur);
        }
    }

    public bool PopupActive()
    {
        return (_popupLinkedList.Count > 0);
    }

    public void SetEventSystemNavigation(GameObject go)
    {
        EventSystem.current.firstSelectedGameObject = go;
    }

    public void Clear()
    {
        if (sceneUI != null)
        {
            Managers.Resource.Destroy(sceneUI);
        }
        foreach (var cur in _popupLinkedList)
        {
            Managers.Resource.Destroy(cur);
            _popupLinkedList.Remove(cur);
        }
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