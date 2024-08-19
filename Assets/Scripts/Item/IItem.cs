public interface IItem
{
    /// <summary>
    /// 아이템이 생성될 때 필수로 설정되어야 하는 것들을 설정함
    /// </summary>
    //public abstract void Init();

    /// <summary>
    /// 아이템 사용시 기능 구현
    /// </summary>
    public abstract void Use();

    /// <summary>
    /// 아이템 선택시 기능 구현(사용은 안하고 들고만 있는 상태)
    /// </summary>
    public abstract void OnHold(); //(설치 가능여부 이런것도 여기서 구현 가능)
}