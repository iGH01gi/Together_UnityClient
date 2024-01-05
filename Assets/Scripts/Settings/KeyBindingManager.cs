using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Components;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class KeyBindingManager : MonoBehaviour
{
    //will add through editor as of now as file management isn't fixed yet
    [SerializeField] InputActionAsset _inputActionAsset;
    private InputActionMap _playerControl;
    
    //*** IMPORTANT ***
    //Dictionary _keybindings:
    //Key = key binding name
    //value = binding index / key binding
    private Dictionary<string,Tuple<int,InputBinding>> _keyBindings;
    InputActionRebindingExtensions.RebindingOperation _rebindOperation;


    void Start()
    {
        _playerControl = _inputActionAsset.FindActionMap("Player");
        _keyBindings = new Dictionary<string,Tuple<int, InputBinding>>();
        GetCurrentBinding();
        DisplayKeySetting();
    }

    void GetCurrentBinding()
    {
        foreach (var action in _playerControl.actions)
        {
            if (action.bindings.Count > 1)
            {
                int i = 0;
                foreach (var subaction in action.bindings)
                {
                    if (!subaction.isComposite)
                    {
                        _keyBindings.Add(subaction.name, Tuple.Create(i,subaction));
                    }
                    i++;
                }
            }
            else
            {
                _keyBindings.Add(action.name, Tuple.Create(0,action.bindings[0]));
            }
        }
    }

    void DisplayKeySetting()
    {
        
        foreach (KeyValuePair<string,Tuple<int, InputBinding>> current in _keyBindings)
        {
            GameObject go = Managers.Resource.Instantiate("UI/KeyBindings/[keyname]", transform);
            go.name = current.Key;
            go.transform.GetChild(0).GetComponent<LocalizeStringEvent>().StringReference
                .SetReference("StringTable", current.Key);
            go.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => ChangeKeySetting(go.transform.GetChild(1).gameObject,current));
            go.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text =
                current.Value.Item2.path.Replace("<Keyboard>/", "");
        }
    }

    public void ChangeKeySetting(GameObject button, KeyValuePair<string,Tuple<int, InputBinding>> current)
    {
        InputAction action = _playerControl[current.Value.Item2.action];
        action.Disable();
        _rebindOperation = action.PerformInteractiveRebinding(current.Value.Item1)
            .OnComplete(operation => RebindComplete(button, current,action))
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.1f)
            .Start();
    }
    
    void RebindComplete(GameObject button, KeyValuePair<string,Tuple<int, InputBinding>> current,InputAction action)
    {
        
        bool successful = true;
        bool isComposite = current.Value.Item1 > 0;
        string newBinding = action.bindings[current.Value.Item1].path;
        string keyname = current.Key;

        foreach (var search in _keyBindings)
        {
            if (keyname != search.Key && newBinding == search.Value.Item2.path)
            {
                //[ADD] UI POP UP
                Debug.Log("Overlapping Controls!!!!");
                action.ApplyBindingOverride(current.Value.Item1,current.Value.Item2.path);
                successful = false;
                break;
            }
        }

        if (successful)
        {
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = newBinding.Replace("<Keyboard>/", "");
            // [ADD] save binding changes to json
        }
        else
        {
            // [ADD] binding failed popup
        }
        action.Enable();
        _rebindOperation.Dispose();
        
        
        
        Debug.Log($"{keyname}finished rebinding to : {newBinding}");
    }
}