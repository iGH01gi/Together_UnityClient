using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Components;

public class ChangeKeyBinding : MonoBehaviour
{
    //will add through editor as of now as file management isn't fixed yet
    [SerializeField] InputActionAsset _inputActionAsset;
    private InputActionMap _playerControl;
    private Dictionary<string, string> _keyBindings;

    void Start()
    {
        _playerControl = _inputActionAsset.FindActionMap("Player");
        _keyBindings = new Dictionary<string, string>();
        GetCurrentBinding();
        DisplayKeySetting();
    }

    void GetCurrentBinding()
    {
        foreach (var binding in _playerControl.bindings)
        {
            if (!binding.isComposite)
            {
                if (binding.name == "")
                {
                    _keyBindings.Add(binding.action,binding.path);
                }
                else
                {
                    _keyBindings.Add(binding.name,binding.path);
                }
            }
        }
    }

    void DisplayKeySetting()
    {
        foreach(KeyValuePair<string, string> current in _keyBindings)
        {
            GameObject go = Managers.Resource.Instantiate("UI/KeyBindings/[keyname]", transform);
            go.name = current.Key;
            go.transform.GetChild(0).GetComponent<LocalizeStringEvent>().StringReference.SetReference("StringTable",current.Key);
            go.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = current.Value.Replace("<Keyboard>/","");
        }
    }

    /*
    void ChangeKeySetting()
    {
        _playerControl["transform.parent.name"].PerformInteractiveRebinding().WithCancelingThrough("<Keyboard>/escape");
        foreach (var binding in _playerControl.bindings)
        {
            if (!binding.isComposite)
            {
                if (binding.name == "")
                {
                    _keyBindings.Add(binding.action,binding.path);
                }
                else
                {
                    _keyBindings.Add(binding.name,binding.path);
                }
            }
        }
    }
    */
}
