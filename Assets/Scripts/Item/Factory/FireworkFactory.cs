using UnityEngine;

public class FireworkFactory : ItemFactory
{
    protected override GameObject CreateProduct()
    {
        GameObject fireworkObj = GameObject.Instantiate(Managers.Item._itemPrefabs[1]);
        fireworkObj.AddComponent<Firework>();
        return fireworkObj;
    }
}