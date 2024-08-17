using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager
{
    private InputActionAsset _inputActionAsset;
    private bool _isCursorVisible;
    private MovementInput _movementInput;
    ObjectInput _objectInput;
    PlayerAnimController _playerAnimController;

    public void Init()
    {
        _inputActionAsset = Resources.Load<InputActionAsset>("playerInput");
        _isCursorVisible = false;
        /*
        root = GameObject.Find("@Input");
       /* if (root == null)
        {
            root = new GameObject { name = "@Input" };
            Object.DontDestroyOnLoad(root);
            PlayerInput playerInput = root.AddComponent<PlayerInput>();
            playerInput.actions = _inputActionAsset;
            playerInput.notificationBehavior = PlayerNotifications.BroadcastMessages;
            LobbyInput = new GameObject { name = "LobbyInput" };
            LobbyInput.transform.SetParent(root.transform);
            LobbyInput.AddComponent<LobbyInput>();
            InGameInput = new GameObject { name = "InGameInput" };
            InGameInput.transform.SetParent(root.transform);
            InGameInput.AddComponent<InGameInput>();
            
            //테스트를 위한 임시 코드
            LobbyInput.SetActive(false);
        }
        */
    }
    
    public void EnableInput()
    {
        
        _inputActionAsset.Enable();
    }
    
    public void DisableInput()
    {
        _inputActionAsset.Disable();
    }

    public void EnableCursor()
    {
        if (_movementInput == null || _playerAnimController ==null || _objectInput == null)
        {
            _movementInput = Managers.Player._myDediPlayer.GetComponent<MovementInput>();
            _playerAnimController = Managers.Player._myDediPlayer.GetComponentInChildren<PlayerAnimController>();
            _objectInput = Managers.Player._myDediPlayer.GetComponent<ObjectInput>();
        }
        _movementInput.enabled = false;
        _objectInput.enabled = false;
        _playerAnimController.PlayerAnimClear();
        _playerAnimController.enabled = false;
        Cursor.visible = true;
        _isCursorVisible = true;
    }
    
    public void DisableCursor()
    {
        if (_movementInput == null || _playerAnimController ==null || _objectInput == null)
        {
            _movementInput = Managers.Player._myDediPlayer.GetComponent<MovementInput>();
            _playerAnimController = Managers.Player._myDediPlayer.GetComponentInChildren<PlayerAnimController>();
            _objectInput = Managers.Player._myDediPlayer.GetComponent<ObjectInput>();
        }
        _objectInput.enabled = true;
        _movementInput.enabled = true;
        _playerAnimController.enabled = true;
        Cursor.visible = false;
        _isCursorVisible = false;
    }
}
