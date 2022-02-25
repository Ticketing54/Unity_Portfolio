using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class ITemUiManager : MonoBehaviour,IPointerUpHandler, IPointerDownHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    [SerializeField]
    UI_Inventory Inven;
    [SerializeField]
    UI_Equipment Equip;
    [SerializeField]
    UI_QuickSlot Quick;
   
    public delegate void UISlotUpdateType(int _SlotNum);    
    
    
    [SerializeField]
    ITEMLISTTYPE WorkingType;      // 드래그중인 List 종류    
    [SerializeField]
    int WorkingSlotNum;                // 드래그중인 슬롯 넘버      
    [SerializeField]
    Vector2 ClickPos;               // 클릭 지점
    [SerializeField]
    Vector3 InfoPreset;             // 정보창 프리셋
    [SerializeField]
    Sprite WorkingSprite;
    [SerializeField]
    Image MoveIcon = null;          // 드래그아이템 이미지
    [SerializeField]
    Item item = null;

    [SerializeField]
    MoveWindow movewindow;          //윈도우창 움직이기
    [SerializeField]
    RectTransform Window;           //움직이는 대상
    [SerializeField]
    bool WindowDrag = false;        //윈도우 바를 클릭했을때    
    [SerializeField]
    GameObject DontClick;           // 클릭 제한 화면

    Vector2 Window_Preset = Vector2.zero;       // 창 드래그 시 적용 프리셋


    [SerializeField]           // 클릭시 정보창 
    MiniInfo miniinfo;

    public Shop shop;    
    public GameObject InventoryMax;
    public bool isClick = false;
    public bool ItemInfo = false;

    bool LeftClick = false;
    bool RightClick = false;

    
   
    #region Reset // UpdateUI
    void UIMoveUpdate(ITEMLISTTYPE _Type, int _SlotNum)
    {
        switch (_Type)
        {
            case ITEMLISTTYPE.INVEN:
                Inven.UpdateSlot(_SlotNum);
                return;
            case ITEMLISTTYPE.EQUIP:
                Equip.UpdateSlot(_SlotNum);
                return;
            case ITEMLISTTYPE.QUICK:
                Quick.UpdateSlot(_SlotNum);
                return;
            default:
                return ;
        }
    }
    
    void WorkingReset()
    {
        WorkingType = ITEMLISTTYPE.NONE;
        WorkingSlotNum = -1;
        WorkingSprite = null;
        ClickPos = Vector2.zero;
        LeftClick = false;
        RightClick = false;
        if (MoveIcon.gameObject.activeSelf == true)
            MoveIcon.gameObject.SetActive(false);
    }
    public void UpdateUIInfo()
    {
        Inven.UpdateInven();
        Equip.UpdateEquip();

        //Quick슬롯은 처음에 보이기 때문에 처음에 업데이트 한번 할 것.
    }
    public Vector3 GetInfoPreset(ITEMLISTTYPE _Type, Vector2 ClickPos)
    {
        if (_Type == ITEMLISTTYPE.QUICK)
        {
            return new Vector3(ClickPos.x + 75f, ClickPos.y + 100f, 0);
        }
        else
        {
            return new Vector3(ClickPos.x + 75f, ClickPos.y - 100f, 0);
        }
    }
    #endregion

    #region StartMoveItem   
    // 드래그
    void MoveItemSetting(int _Num, ITEMLISTTYPE _Type,Sprite _Sprite)
    {
        WorkingType = _Type;        
        WorkingSlotNum = _Num;
        MoveIcon.gameObject.SetActive(true);
        WorkingSprite = _Sprite;
        MoveIcon.sprite = WorkingSprite;
        MoveIcon.transform.position = ClickPos;
        LeftClick = true;
    }
    // 클릭
    void ClickItemSetting(int _Num, ITEMLISTTYPE _Type, Sprite _Sprite)
    {
        WorkingType = _Type;
        WorkingSlotNum = _Num;
        RightClick = true;
    }
   
    #endregion

    #region EndMoveItem
   //드래그 성공
    void EndItemSetting(int _E_Num, ITEMLISTTYPE _EndListType)
    {
        Character.Player.ItemMove(WorkingType, WorkingSlotNum, _EndListType, _E_Num, UIMoveUpdate);        
        WorkingReset();

    }
    //드래그 실패
    void DragFail()
    {        
        UIMoveUpdate(WorkingType, WorkingSlotNum);
        WorkingReset();
    }
    //우 클릭
    void ItemInfoOn(Vector3 _Preset)
    {
        item = Character.Player.GetItem(WorkingType, WorkingSlotNum);
        miniinfo.gameObject.SetActive(true);
        miniinfo.MiniInfoUpdate(item, _Preset, WorkingSprite);
        item = null;
    }
    //좌 클릭
    #endregion

    public void OnPointerDown(PointerEventData data)
    {
        if (shop.gameObject.activeSelf == true)
            return;

        ClickPos = data.position; // 클릭지점        

        UIManager.uimanager.CloseMiniitemInfo();

        

        if (Input.GetMouseButton(0))
        {            
            if (InventoryMax.activeSelf == false)
            {
                if (Quick.ClickDownQuick_Item(MoveItemSetting, data.position))
                    return;                
            }
            else if (InventoryMax.activeSelf == true)
            {
                if (movewindow.isInRect(data.position))
                {
                    WindowDrag = true;
                    Window_Preset = data.position - (Vector2)Window.position;

                }
                if (Inven.ClickdownInven(MoveItemSetting, data.position))
                    return;
                if (Equip.ClickDownEquip(MoveItemSetting, data.position))
                    return;
                if (Quick.ClickDownQuick_Item(MoveItemSetting, data.position))
                    return;                

            }
            
        }
        if (Input.GetMouseButtonDown(1))
        {
            
            if (InventoryMax.activeSelf == false)
            {
                if (Quick.ClickDownQuick_Item(ClickItemSetting, data.position))
                    return;
               
            }
            else if (InventoryMax.activeSelf == true)
            {
                if (Quick.ClickDownQuick_Item(ClickItemSetting, data.position))
                    return;
                if (Equip.ClickDownEquip(ClickItemSetting, data.position))
                    return;
                if (Inven.ClickdownInven(ClickItemSetting, data.position))
                    return;

            }
        }
        
    }  

   
    public void OnPointerUp(PointerEventData data)
    {
        if (WorkingSlotNum < 0)
            return;
        if (shop.gameObject.activeSelf == true)
            return;
        WindowDrag = false;

        if (ClickPos == data.position && LeftClick == true)              // 좌 클릭 시
        {
            ItemInfoOn(GetInfoPreset(WorkingType, data.position));                    
            DragFail();            
            return;
        }
        else if (ClickPos == data.position && RightClick == true)       // 우 클릭 시
        {
            Character.Player.ItemMove(WorkingType, WorkingSlotNum, UIMoveUpdate);                //   수정할 것!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            WorkingReset();            
            return;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (InventoryMax.activeSelf == false)   // 인벤토리가 꺼져있을때
            {
                if (Quick.ClickUpQuick_Item(EndItemSetting, data.position))
                    return;
                if (WorkingSlotNum >= 0 && MoveIcon.gameObject.activeSelf == true)
                {                    
                    DragFail();
                    return;
                }
            }
            else if (InventoryMax.activeSelf == true)  // 인벤토리 켜져있을때
            {
                if (Inven.ClickUpInven(EndItemSetting, data.position))
                    return;
                if (Quick.ClickUpQuick_Item(EndItemSetting, data.position))
                    return;
                if (Equip.CLickUpEquip(EndItemSetting, data.position))
                    return;
                //SoundManager.soundmanager.soundsPlay("Pick");
                if (WorkingSlotNum >= 0 && MoveIcon.gameObject.activeSelf == true)
                {
                    DragFail();
                    return;
                }
            }


        }
    } 
    

    
    public void OnBeginDrag(PointerEventData data)
    {
        if (shop.gameObject.activeSelf == true)
            return;
        DontClick.SetActive(true);
    }
    public void OnDrag(PointerEventData data)
    {
        if (shop.gameObject.activeSelf == true)
            return;
        MoveIcon.rectTransform.position = data.position;
       
        if(WindowDrag == true)
        {
            
            Window.position = data.position - Window_Preset;
        }
    }
    public void OnEndDrag(PointerEventData data)
    {
        if (shop.gameObject.activeSelf == true)
            return;
        DontClick.SetActive(false);            

    }
   
   
 

}
