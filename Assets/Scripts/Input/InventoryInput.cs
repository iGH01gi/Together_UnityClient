using UnityEngine;
using UnityEngine.InputSystem;
public class InventoryInput : MonoBehaviour
{
    void OnInventory(InputValue value)
    {
        InGameUI inGameUI = Managers.UI.GetComponentInSceneUI<InGameUI>();
        if (inGameUI._isInventoryOpen)
        {
            Managers.Input.DisableCursor();
            inGameUI.CloseInventory();
        }
        else
        {
            //다른 하던 일 멈추기
            transform.GetComponent<ObjectInput>().QuitCleansing();
            Managers.Input.EnableCursor();
            inGameUI.OpenInventory();
        }
    }
    
    void OnUseItem(InputValue value)
    {
        int itemID = Managers.Inventory._hotbar.CurrentSelectedItemID();
        if(itemID != -1)
        {
            Managers.Item._items[itemID].Use();
        }
    }
    
    void OnHotbar0(InputValue value)
    {
        Managers.Inventory._hotbar.ChangeSelected(0);
    }
    
    void OnHotbar1(InputValue value)
    {
        Managers.Inventory._hotbar.ChangeSelected(1);
    }
    
    void OnHotbar2(InputValue value)
    {
        Managers.Inventory._hotbar.ChangeSelected(2);
    }
    
    void OnHotbar3(InputValue value)
    {
        Managers.Inventory._hotbar.ChangeSelected(3);
    }
    
    void OnHotbar4(InputValue value)
    {
        Managers.Inventory._hotbar.ChangeSelected(4);
    }
}
