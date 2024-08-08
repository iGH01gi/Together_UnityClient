using UnityEngine;

public class DashFactory : ItemFactory
{
    protected override GameObject CreateProduct()
    {
        GameObject dashObj = GameObject.Instantiate(Managers.Item._itemPrefabs[0]);
        dashObj.AddComponent<Dash>();
        return dashObj;
    }
}
