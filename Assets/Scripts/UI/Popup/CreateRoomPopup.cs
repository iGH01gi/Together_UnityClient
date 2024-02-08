using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class CreateRoomPopup : UI_popup
{
    private UI_InputField _roomName;
    private UI_Toggle _passwordToggle;
    private UI_InputField _password;
    void Start()
    {
        _roomName = transform.GetChild(0).GetChild(1).GetComponent<UI_InputField>();
        _passwordToggle = transform.GetChild(1).GetChild(1).GetComponent<UI_Toggle>();
        _password = transform.GetChild(2).GetChild(1).GetComponent<UI_InputField>();
        _roomName.SetChildString("RoomName");
        _passwordToggle.SetChildString("UsePassword");
        _password.SetChildString("Password");
        _passwordToggle.SetOnClick(UsePassword);
        transform.GetChild(3).GetComponent<UI_Button>().SetChildString("CreateRoom");
        transform.GetChild(3).GetComponent<UI_Button>().SetOnClick(SubmitCreateRoom);
        transform.GetChild(4).GetComponent<UI_Button>().SetChildString("Close");
        transform.GetChild(4).GetComponent<UI_Button>().SetOnClick(ClosePopup);
    }

    void UsePassword()
    {
        _password.SetInteractable(_passwordToggle.GetToggleState());
    }

    void SubmitCreateRoom()
    {
        if (_roomName.GetInputText().Length < 1)
        {
            Managers.UI.LoadPopupPanel<InputRoomNamePopup>();
        }
        
        else if (_password.GetInputText().Length < 1)
        {
            Managers.UI.LoadPopupPanel<InputPasswordPopup>();
        }
        else
        {
            C_MakeRoom cMakeRoom = new C_MakeRoom();
            cMakeRoom.Title = _roomName.GetInputText();
            cMakeRoom.IsPrivate = _passwordToggle.GetToggleState();
            cMakeRoom.Password = _password.GetInputText();
            Managers.Network._session.Send(cMakeRoom);
        }
    }
}