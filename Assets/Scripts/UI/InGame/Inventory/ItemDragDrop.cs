using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragDrop : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private Transform _originaSlot;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Vector3 _localPos;

    public void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>(); //드래그 중에는 레이캐스트를 막아야함
        _localPos = transform.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _originaSlot = transform.parent; //드래그 시작 시 부모 저장
        transform.SetParent(Managers.UI.SceneUI.transform.Find($"DragPanel"));//드래그 중 옮길 가라 패널
        _canvasGroup.blocksRaycasts = false; //드래그 중 레이캐스트를 막음
        _canvasGroup.alpha = 0.6f; //드래그 중 투명도 조절
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.position = Input.mousePosition; //드래그 중 마우스 위치로 이동
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        GameObject droppedLocation = eventData.pointerCurrentRaycast.gameObject;
        if (droppedLocation != null)
        {
            droppedLocation = droppedLocation.transform.parent.gameObject;
        }
        if (droppedLocation != null && droppedLocation.GetComponent<InventorySlot>() != null)
        {
            InventorySlot current = transform.GetComponent<InventorySlot>();
            InventorySlot target = droppedLocation.GetComponent<InventorySlot>();
            //기존 슬롯의 아이템을 신규 슬롯에 보내기
            transform.SetParent(droppedLocation.transform.parent);
            transform.localPosition = _localPos;
            
            //신규 슬롯의 아이템을 기존 슬롯에 보내기
            
            droppedLocation.transform.SetParent(_originaSlot);
            droppedLocation.transform.localPosition = _localPos;
            Debug.Log("Swap Complete");
        }
        else
        {
            transform.SetParent(_originaSlot);
            transform.localPosition = _localPos;
        }
        _canvasGroup.blocksRaycasts = true; //드래그 종료 시 레이캐스트 허용
        _canvasGroup.alpha = 1f; //드래그 종료 시 투명도 원상복귀
    }
}