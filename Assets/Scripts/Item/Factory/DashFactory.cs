using UnityEngine;

public class DashFactory : ItemFactory
{
    public float DashDistance { get; set; }
    
    public DashFactory(int id, int price, string englishName, string koreanName, string englishDescription,
        string koreanDescription, float dashDistance)
    {
        base.FactoryInit(id, price, englishName, koreanName, englishDescription, koreanDescription);
        DashDistance = dashDistance;
    }
    
    public new IItem CreateItem()
    {
        return null;
    }
}
