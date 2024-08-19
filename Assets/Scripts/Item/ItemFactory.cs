using UnityEngine;

public abstract class ItemFactory
{ 
    //공통 속성
    protected int FactoryId { get; set; }
    protected int FactoryPrice { get; set; }
    protected string FactoryEnglishName { get; set; }
    protected string FactoryKoreanName { get; set; }
    protected string FactoryEnglishDescription { get; set; }
    protected string FactoryKoreanDescription { get; set; }
    
    //필수 설정되어야 하는 것들 설정
    public virtual void FactoryInit(int id, int price, string englishName, string koreanName, string englishDescription,
        string koreanDescription)
    {
        FactoryId = id;
        FactoryPrice = price;
        FactoryEnglishName = englishName;
        FactoryKoreanName = koreanName;
        FactoryEnglishDescription = englishDescription;
        FactoryKoreanDescription = koreanDescription;
    }

    public abstract IItem CreateItem();

    public int GetFactoryID()
    {
        return FactoryId;
    }
}