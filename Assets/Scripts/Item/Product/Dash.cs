using System;

public class Dash : IItem
{
    //아이템 공통 보유 속성
    public int Id { get; set; }
    public int Price { get; set; }
    public string EnglishName { get; set; }
    public string KoreanName { get; set; }
    public string EnglishDescription { get; set; }
    public string KoreanDescription { get; set; }
    
    //이 아이템만의 속성
    public float DashDistance { get; set; }
    
    public void Setting()
    {
        //아이템 매니저로부터 아이템 데이터를 받아와서 설정
        Dash dashData = Managers.Item._items[1] as Dash;
        Id = dashData.Id;
        Price = dashData.Price;
        EnglishName = dashData.EnglishName;
        KoreanName = dashData.KoreanName;
        EnglishDescription = dashData.EnglishDescription;
        KoreanDescription = dashData.KoreanDescription;
        DashDistance = dashData.DashDistance;
    }

    public void Use()
    {
        
    }
}