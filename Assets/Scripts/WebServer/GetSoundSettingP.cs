using UnityEngine;

namespace WebServer
{
    //서버로부터 받을 데이터구조
    public class GetSoundSettingResponseDTO : BaseResponseDTO
    {
        public float Master;
        public float Bgm;
        public float Effects;
    }
    public class GetSoundSettingP : BaseWebProtocol
    {
        private string _userId;
        public void Init(string userId)
        {
            _userId = $"{userId}";
        }
        
        public override void SetUrlHttpMethod()
        {
            url = "/soundsetting"+$"/{_userId}";
            httpMethod = Define.HttpMethod.Get;
        }

        public override string MakeRequestBody()
        {
            return null;
        }
    }
}