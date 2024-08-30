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
    private Light _light;
    private MovementInput _movementInput;
    private Coroutine _currentPlayingCoroutine;
    private GameObject _flashLightSource;

    private OtherDediPlayer _otherDediPlayer;
    public void LateUpdate()
    {
        if (_isLightOn)
        {
            if (PlayerID == Managers.Player._myDediPlayerId)
            {
                //���� ������ ������ ���� ǥ��
                Debug.DrawRay(_flashLightSource.transform.position,
                    _flashLightSource.transform.forward * FlashlightDistance, Color.red, 0.1f);

                // ���� ȸ���� �����ɴϴ�.
                Quaternion currentRotation = _flashLightSource.transform.rotation;

                // ���� ȸ���� Euler ������ �����ɴϴ�.
                Vector3 eulerAngles = currentRotation.eulerAngles;

                // X�� ȸ������ _movementInput._rotationX�� �����մϴ�.
                float newXRotation = _movementInput._rotationX;

                // ���ο� ȸ�� ���� �����մϴ�.
                Quaternion newRotation = Quaternion.Euler(newXRotation, eulerAngles.y, eulerAngles.z);
                _flashLightSource.transform.rotation = newRotation;
            }
            else
            {
                //���� ������ ������ ���� ǥ��
                Debug.DrawRay(_flashLightSource.transform.position,
                    _flashLightSource.transform.forward * FlashlightDistance, Color.red, 0.1f);

                //ȸ�� ��ǥ ī�޶� ��ġ�� ������
                Quaternion targetRotation = _otherDediPlayer._cameraWorldRotation;

                // ���� ȸ���� �����ɴϴ�.
                Quaternion currentRotation = _flashLightSource.transform.rotation;

                // ���� ȸ���� Euler ������ �����ɴϴ�.
                Vector3 eulerAngles = currentRotation.eulerAngles;

                // X�� ȸ������ _movementInput._rotationX�� �����մϴ�.
                float newXRotation = targetRotation.eulerAngles.x;

                // ���ο� ȸ�� �ε巴�� ���� �����մϴ�.
                Quaternion newRotation = Quaternion.Euler(newXRotation, eulerAngles.y, eulerAngles.z);
                //_light.transform.rotation = Quaternion.Slerp(currentRotation, newRotation, Time.deltaTime * 20f);
                _flashLightSource.transform.rotation = newRotation;
            }


            /*//���� ������ ������ ���� ǥ��
            Debug.DrawRay(_lightGameObject.transform.position, _lightGameObject.transform.forward * FlashlightDistance, Color.red, 0.1f);

            // ���� ȸ���� �����ɴϴ�.
            Quaternion currentRotation = _lightGameObject.transform.rotation;

            // ���� ȸ���� Euler ������ �����ɴϴ�.
            Vector3 eulerAngles = currentRotation.eulerAngles;

            // X�� ȸ������ _movementInput._rotationX�� �����մϴ�.
            float newXRotation = _movementInput._rotationX;

            // ���ο� ȸ�� ���� �����մϴ�.
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
            //�̹� ������ε� �� ����Ϸ��� �ϸ�, ���� �ڷ�ƾ �����ϰ� �ڷ�ƾ �ٽý���
            if (_isLightOn)
            {
                StopCoroutine(_currentPlayingCoroutine);
                _currentPlayingCoroutine = StartCoroutine(LightOffAfterSeconds(FlashlightAvailableTime));
                return true;
            }

            GameObject myDediPlayerGameObject = Managers.Player.GetPlayerObject(PlayerID);

            _flashLightSource = Util.FindChild(myDediPlayerGameObject, "FlashLightSource", true);

            if (_flashLightSource != null)
            {
                GameObject lightGameObject = Util.FindChild(_flashLightSource, "Light", true);
                if (lightGameObject != null)
                {
                    //�ִϸ��̼� Ŵ
                    PlayerAnimController anim = Managers.Player.GetAnimator(PlayerID);
                    anim.isFlashlight = true;
                    anim.PlayAnim();

                    //����Ʈ Ŵ
                    _light = lightGameObject.GetComponent<Light>();
                    _light.enabled = true;
                    _light.range = FlashlightDistance; 
                    _light.spotAngle = FlashlightAngle;

                    //�� Ŵ
                    _movementInput = Managers.Player._myDediPlayer.GetComponent<MovementInput>();
                    _isLightOn = true;

                    //���� �ð� �� �� ��
                    _currentPlayingCoroutine = StartCoroutine(LightOffAfterSeconds(FlashlightAvailableTime));

                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            //�̹� ������ε� �� ����Ϸ��� �ϸ�, ���� �ڷ�ƾ �����ϰ� �ڷ�ƾ �ٽý���
            if (_isLightOn)
            {
                StopCoroutine(_currentPlayingCoroutine);
                _currentPlayingCoroutine = StartCoroutine(LightOffAfterSeconds(FlashlightAvailableTime));
                return true;
            }

            GameObject otherDediPlayerGameObject = Managers.Player.GetPlayerObject(PlayerID);
            _otherDediPlayer = otherDediPlayerGameObject.GetComponent<OtherDediPlayer>();

            _flashLightSource = Util.FindChild(otherDediPlayerGameObject, "FlashLightSource", true);

            GameObject flashlightGameObject = Util.FindChild(otherDediPlayerGameObject, "3", true);
            if (flashlightGameObject != null)
            {
                GameObject lightGameObject = Util.FindChild(_flashLightSource, "Light", true);
                if (lightGameObject != null)
                {
                    //�ִϸ��̼� Ŵ
                    PlayerAnimController anim = Managers.Player.GetAnimator(PlayerID);
                    anim.isFlashlight = true;
                    anim.PlayAnim();

                    //����Ʈ Ŵ
                    _light = lightGameObject.GetComponent<Light>();
                    _light.enabled = true;
                    _light.range = FlashlightDistance;
                    _light.spotAngle = FlashlightAngle;

                    //�� Ŵ
                    _movementInput = Managers.Player._otherDediPlayers[PlayerID].GetComponent<MovementInput>();
                    _isLightOn = true;

                    //���� �ð� �� �� ��
                    _currentPlayingCoroutine = StartCoroutine(LightOffAfterSeconds(FlashlightAvailableTime));

                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        Debug.Log("Item Flashlight Use");

        return true;
    }

    IEnumerator LightOffAfterSeconds(float seconds)
    {
        if (PlayerID == Managers.Player._myDediPlayerId)
        {
            //�÷��̾ ����� �������� ������ ����(������ ��� ��Ŷ ����)
            CDS_UseFlashlightItem useFlashlightItemPacket = new CDS_UseFlashlightItem()
            {
                MyDediplayerId = PlayerID,
                ItemId = ItemID
            };
            Managers.Network._dedicatedServerSession.Send(useFlashlightItemPacket);
        }

        yield return new WaitForSeconds(seconds);
        
        _isLightOn = false;

        //����Ʈ ��
        _light.enabled = false;

        //�ִϸ��̼� ��
        PlayerAnimController anim = Managers.Player.GetAnimator(PlayerID);
        anim.isFlashlight = false;
        anim.PlayAnim();

        //�ı�
        Object.Destroy(gameObject);
    }

    public void OnHold()
    {

    }

    public void OnHit()
    {
        
    }
}
