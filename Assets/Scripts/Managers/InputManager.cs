using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager
{
    static List<Define.InputType> _inputList = new List<Define.InputType>();
    static GameObject root;
    public void Init()
    {
        root = GameObject.Find("@Input");
        if (root == null)
        {
            root = new GameObject { name = "@Input" };
            Object.DontDestroyOnLoad(root);
            PlayerInput playerInput = root.AddComponent<PlayerInput>();
            playerInput.actions = Resources.Load<InputActionAsset>("playerInput");
            playerInput.notificationBehavior = PlayerNotifications.BroadcastMessages;
            AddInput(Define.InputType.UIInputHandler);
        }
    }

    public void AddInput(Define.InputType inputType)
    {
        if (!_inputList.Contains(inputType))
        {
            _inputList.Add(inputType);
            GameObject newInputObject = new GameObject(inputType.ToString());
            switch (inputType)
            {
                case Define.InputType.UIInputHandler:
                    newInputObject.AddComponent<UIInputHandler>();
                    break;
            }
            newInputObject.transform.SetParent(root.transform);
        }
    }

    public void RemoveInput(Define.InputType inputType)
    {
        if (_inputList.Contains(inputType))
        {
            GameObject.Destroy(root.transform.GetChild(_inputList.IndexOf(inputType)));
            _inputList.Remove(inputType);
        }
    }
}
