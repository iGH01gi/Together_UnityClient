public abstract class ItemProduct
{
    //공통 속성
    protected int Id { get; set; }
    protected int Price { get; set; }
    protected string EnglishName { get; set; }
    protected string KoreanName { get; set; }
    protected string EnglishDescription { get; set; }
    protected string KoreanDescription { get; set; }

    /// <summary>
    /// 아이템이 생성될 때 필수로 설정되어야 하는 것들을 설정함
    /// </summary>
    protected virtual void Init(int id, int price, string englishName, string koreanName, string englishDescription, string koreanDescription)
    {
        Id = id;
        Price = price;
        EnglishName = englishName;
        KoreanName = koreanName;
        EnglishDescription = englishDescription;
        KoreanDescription = koreanDescription;
    }

    /// <summary>
    /// 아이템 사용시 기능 구현
    /// </summary>
    public abstract void Use();

    /// <summary>
    /// 아이템 선택시 기능 구현(사용은 안하고 들고만 있는 상태)
    /// </summary>
    public abstract void OnHold(); //(설치 가능여부 이런것도 여기서 구현 가능)
}