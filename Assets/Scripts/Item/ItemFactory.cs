
using UnityEngine;

public abstract class ItemFactory
{
    public GameObject CreateItem()
    {
        GameObject itemObj = CreateProduct();
        
        //item오브젝트에서 IItem을 상속받은 컴포넌트를 찾는다
        IItem itemComponent = itemObj.GetComponent<IItem>();
        
        //초기 세팅
        itemComponent.Setting();
        
        return itemObj;
    }

    protected abstract GameObject CreateProduct(); //상속한 팩토리에서 구현
}