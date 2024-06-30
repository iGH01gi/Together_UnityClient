using System;

public class TimeManager
{
    TimeSpan _dediserverClientTimeDelta; //데디서버와 클라이언트의 시간차이. 클라 타임스탬프에 이값을 더하면 대충 데디서버 시간이라고 추론 가능
    
    
    /// <summary>
    /// 데디서버의 타임스탬프를 요청함(데디 서버의 시간 추론을 위한 정보 획득을 위하여)
    /// </summary>
    public void GetTimeStamp()
    {
        
    }

    //데디서버의 타임스탬프를 받으면 rtt/2를 GetTimeStamp()호출시의 타임스탬프에 더한 값 Tc를 구하고,
    //데디서버가 응답한 타임스탬픅 Ts일때, Ts-Tc = d를 구한다.
    //이 d값을 이용하여 데디서버의 시간을 추론할 수 있다.(클라 타임스탬프에 d 더하면 대충 데디서버 시간값이라고 추론 가능)
    //이 과정을 여러번 수행하여서 평균,표준편차를 구하고, 표준편차를 벗어나는 데이터는 모두 outlier로 취급해서 고려x
    //이러한 이상치를 제외한 상태에서 평균이나 중위값을 계산한 다음 최종 d값을 도출한다. 
    //이때 rtt를 측정하는 방식은 Exponential Weighted Moving Average (EWMA)를 사용한다. (넷응설 챕3 70p참고)
    public void OnRecvDediServerTimeStamp()
    {
        
    }
}