using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class Flashlight : MonoBehaviour, IItem
{
    public int ItemID { get; set; }
    public int PlayerID { get; set; }
    public string EnglishName { get; set; }

    public float BlindDuration {get; set;}
    public float FlashlightDistance {get; set;}
    public float FlashlightAngle {get; set;}
    public float FlashlightAvailableTime {get; set;}
    public float FlashlightTimeRequired {get; set;}


    private bool _isLightOn = false;
    private GameObject _flashlightGameObject;
    private GameObject _lightGameObject;
    private Light _light;
    private MovementInput _movementInput;
    private Coroutine _currentPlayingCoroutine;
    private Quaternion _originalLightRotation;

    private OtherDediPlayer _otherDediPlayer;
    public void LateUpdate()
    {
        if (_isLightOn)
        {
            if (PlayerID == Managers.Player._myDediPlayerId)
            {
                //빛과 동일한 길이의 레이 표시
                Debug.DrawRay(_lightGameObject.transform.position, _lightGameObject.transform.forward * FlashlightDistance, Color.red, 0.1f);

                // 현재 회전을 가져옵니다.
                Quaternion currentRotation = _lightGameObject.transform.rotation;

                // 현재 회전의 Euler 각도를 가져옵니다.
                Vector3 eulerAngles = currentRotation.eulerAngles;

                // X축 회전값을 _movementInput._rotationX로 설정합니다.
                float newXRotation = _movementInput._rotationX;

                // 새로운 회전 값을 적용합니다.
                Quaternion newRotation = Quaternion.Euler(newXRotation, eulerAngles.y, eulerAngles.z);
                _lightGameObject.transform.rotation = newRotation;
            }
            else
            {
                //빛과 동일한 길이의 레이 표시
                Debug.DrawRay(_lightGameObject.transform.position,
                    _lightGameObject.transform.forward * FlashlightDistance, Color.red, 0.1f);

                //회전 목표 카메라 위치를 가져옴
                Quaternion targetRotation = _otherDediPlayer._cameraWorldRotation;

                // 현재 회전을 가져옵니다.
                Quaternion currentRotation = _lightGameObject.transform.rotation;

                // 현재 회전의 Euler 각도를 가져옵니다.
                Vector3 eulerAngles = currentRotation.eulerAngles;

                // X축 회전값을 _movementInput._rotationX로 설정합니다.
                float newXRotation = targetRotation.eulerAngles.x;

                // 새로운 회전 부드럽게 값을 적용합니다.
                Quaternion newRotation = Quaternion.Euler(newXRotation, eulerAngles.y, eulerAngles.z);
                _lightGameObject.transform.rotation = Quaternion.Slerp(currentRotation, newRotation, Time.deltaTime * 5f);
            }


            /*//빛과 동일한 길이의 레이 표시
            Debug.DrawRay(_lightGameObject.transform.position, _lightGameObject.transform.forward * FlashlightDistance, Color.red, 0.1f);

            // 현재 회전을 가져옵니다.
            Quaternion currentRotation = _lightGameObject.transform.rotation;

            // 현재 회전의 Euler 각도를 가져옵니다.
            Vector3 eulerAngles = currentRotation.eulerAngles;

            // X축 회전값을 _movementInput._rotationX로 설정합니다.
            float newXRotation = _movementInput._rotationX;

            // 새로운 회전 값을 적용합니다.
            Quaternion newRotation = Quaternion.Euler(newXRotation, eulerAngles.y, eulerAngles.z);
            _lightGameObject.transform.rotation = newRotation;*/
        }
    }

    public void Init(int itemId,int playerId, string englishName)
    {
        this.ItemID = itemId;
        this.PlayerID = playerId;
        this.EnglishName = englishName;
    }
    
    public void Init(int itemId, int playerId, string englishName, float blindDuration, float flashlightDistance, float flashlightAngle, float flashlightAvailableTime, float flashlightTimeRequired)
    {
        Init(itemId, playerId, englishName);
        BlindDuration = blindDuration;
        FlashlightDistance = flashlightDistance;
        FlashlightAngle = flashlightAngle;
        FlashlightAvailableTime = flashlightAvailableTime;
        FlashlightTimeRequired = flashlightTimeRequired;
    }
    
    public bool Use(IMessage recvPacket = null)
    {

        if (PlayerID == Managers.Player._myDediPlayerId)
        {
            //이미 사용중인데 또 사용하려고 하면, 기존 코루틴 종료하고 코루틴 다시시작
            if (_isLightOn)
            {
                StopCoroutine(_currentPlayingCoroutine);
                _currentPlayingCoroutine = StartCoroutine(LightOffAfterSeconds(FlashlightAvailableTime));
                return true;
            }

            GameObject myDediPlayerGameObject = Managers.Player.GetPlayerObject(PlayerID);

            //myDediPlayerGameObject의 자식들중 3(flashlight를 의미하는 아이템id)의 이름을 가진 오브젝트를 참조함
            _flashlightGameObject = Util.FindChild(myDediPlayerGameObject, "3", true);
            if (_flashlightGameObject != null)
            {
                _lightGameObject = Util.FindChild(_flashlightGameObject, "Light", true);
                if (_lightGameObject != null)
                {
                    //회전 원복을 위한 값 저장
                    _originalLightRotation = _lightGameObject.transform.rotation;

                    //애니메이션 킴
                    PlayerAnimController anim = Managers.Player.GetAnimator(PlayerID);
                    anim.isFlashlight = true;
                    anim.PlayAnim();

                    _light = _lightGameObject.GetComponent<Light>();
                    _light.enabled = true;
                    _light.range = FlashlightDistance; 
                    _light.spotAngle = FlashlightAngle;

                    //불 킴
                    _movementInput = Managers.Player._myDediPlayer.GetComponent<MovementInput>();
                    _isLightOn = true;

                    //일정 시간 후 불 끔
                    _currentPlayingCoroutine = StartCoroutine(LightOffAfterSeconds(FlashlightAvailableTime));

                }
            }
        }
        else
        {
            //이미 사용중인데 또 사용하려고 하면, 기존 코루틴 종료하고 코루틴 다시시작
            if (_isLightOn)
            {
                StopCoroutine(_currentPlayingCoroutine);
                _currentPlayingCoroutine = StartCoroutine(LightOffAfterSeconds(FlashlightAvailableTime));
                return true;
            }

            GameObject otherDediPlayerGameObject = Managers.Player.GetPlayerObject(PlayerID);
            _otherDediPlayer = otherDediPlayerGameObject.GetComponent<OtherDediPlayer>();

            //otherDediPlayerGameObject의 자식들중 3(flashlight를 의미하는 아이템id)의 이름을 가진 오브젝트를 참조함
            _flashlightGameObject = Util.FindChild(otherDediPlayerGameObject, "3", true);
            if (_flashlightGameObject != null)
            {
                _lightGameObject = Util.FindChild(_flashlightGameObject, "Light", true);
                if (_lightGameObject != null)
                {
                    //회전 원복을 위한 값 저장
                    _originalLightRotation = _lightGameObject.transform.rotation;

                    //애니메이션 킴
                    PlayerAnimController anim = Managers.Player.GetAnimator(PlayerID);
                    anim.isFlashlight = true;
                    anim.PlayAnim();

                    _light = _lightGameObject.GetComponent<Light>();
                    _light.enabled = true;
                    _light.range = FlashlightDistance;
                    _light.spotAngle = FlashlightAngle;

                    //불 킴
                    _movementInput = Managers.Player._otherDediPlayers[PlayerID].GetComponent<MovementInput>();
                    _isLightOn = true;

                    //일정 시간 후 불 끔
                    _currentPlayingCoroutine = StartCoroutine(LightOffAfterSeconds(FlashlightAvailableTime));

                }
            }   
        }

        Debug.Log("Item Flashlight Use");

        return true;
    }

    IEnumerator LightOffAfterSeconds(float seconds)
    {
        if (PlayerID == Managers.Player._myDediPlayerId)
        {
            //플레이어가 사용한 아이템을 서버로 전송(아이템 사용 패킷 전송)
            CDS_UseFlashlightItem useFlashlightItemPacket = new CDS_UseFlashlightItem()
            {
                MyDediplayerId = PlayerID,
                ItemId = ItemID
            };
            Managers.Network._dedicatedServerSession.Send(useFlashlightItemPacket);
        }

        yield return new WaitForSeconds(seconds);
        _light.enabled = false;
        _isLightOn = false;

        //애니메이션 끔
        PlayerAnimController anim = Managers.Player.GetAnimator(PlayerID);
        anim.isFlashlight = false;
        anim.PlayAnim();

        //회전 원복
        _lightGameObject.transform.rotation = _originalLightRotation;

        //파괴
        Object.Destroy(gameObject);
    }

    public void OnHold()
    {

    }

    public void OnHit()
    {
        
    }
}
