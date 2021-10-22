using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
public enum ItemListType
{
    INVEN,
    QUICK,
    EQUIP,
    NONE
}
public enum Click
{
    LeftClick,
    RightClick
}
public class ITemUiManager : MonoBehaviour,IPointerUpHandler, IPointerDownHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    [SerializeField]
    UI_Inventory Inven;
    [SerializeField]
    UI_Equipment Equip;
    [SerializeField]
    UI_QuickSlot Quick;
   
    public delegate void UISlotUpdate(int _SlotNum);    
    
    
    [SerializeField]
    ItemListType MoveListType;      // 드래그중인 List 종류    
    [SerializeField]
    int MoveSlotNum;                // 드래그중인 슬롯 넘버
    [SerializeField]
    int LastSlotNum;                // 드래그 한 지점 슬롯 넘버    
    [SerializeField]
    Vector2 ClickPos;               // 클릭 지점
    [SerializeField]
    Vector3 InfoPreset;             // 정보창 프리셋
    
    [SerializeField]
    Image MoveIcon = null;          // 드래그아이템 이미지

    [SerializeField]
    MoveWindow movewindow;          //윈도우창 움직이기
    [SerializeField]
    RectTransform Window;           //움직이는 대상
    [SerializeField]
    bool WindowDrag = false;        //윈도우 바를 클릭했을때    
    [SerializeField]
    GameObject DontClick;           // 클릭 제한 화면

    Vector2 Window_Preset = Vector2.zero;       // 창 드래그 시 적용 프리셋


    public MiniInfo miniinfo;
    public Shop shop;    
    public GameObject InventoryMax;
    public bool isClick = false;
    public bool ItemInfo = false;


    public void UpdateInventory()
    {
        Inven.UpdateInven();
    }
    void UpdateUISlot()
    {
        Character.Player.Quick.AllItemUpdateUi(Quick.UpdateSlot, Quick.UpdateClear);
    }

    #region Click   
    void MoveItemSetting(int _Num, ItemListType _Type,Sprite _Sprite)
    {
        MoveListType = _Type;        
        MoveSlotNum = _Num;
        MoveIcon.gameObject.SetActive(true);
        MoveIcon.sprite = _Sprite;
        MoveIcon.transform.position = ClickPos;        
    }
    void EndItemSetting(int _E_Num, ItemListType _EndListType)
    {
        LastSlotNum = _E_Num;
        Character.Player.ItemMove(MoveListType, MoveSlotNum, _EndListType, _E_Num);
        UIMoveUpdate(MoveListType)(MoveSlotNum);
        UIMoveUpdate(_EndListType)(_E_Num);
        MoveInfoReset();
        LastSlotNum = -1;
    }
    #endregion
    #region Drag
    //드래그 실패
    void DragFail()
    {        
        UIMoveUpdate(MoveListType)(MoveSlotNum);
        LastSlotNum = -1;        
        MoveInfoReset();
    }
    //드래그 성공
    
    UISlotUpdate UIMoveUpdate(ItemListType _Type)
    {
        switch (_Type)
        {
            case ItemListType.INVEN:
                return Inven.UpdateSlot;
            case ItemListType.EQUIP:
                return Equip.UpdateSlot;
            case ItemListType.QUICK:
                return Quick.UpdateSlot;
            default:
                return null;
        }
    }
    void MoveInfoReset()
    {
        MoveListType = ItemListType.NONE;
        LastSlotNum = -1;
        MoveSlotNum = -1;
        if (MoveIcon.gameObject.activeSelf == true)
            MoveIcon.gameObject.SetActive(false);
    }
    #endregion

    public void OnPointerDown(PointerEventData data)
    {
        if (shop.gameObject.activeSelf == true)
            return;

        ClickPos = data.position; // 클릭지점        

        if(miniinfo.gameObject.activeSelf==true)
            miniinfo.gameObject.SetActive(false);

        

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
               
            }
            else if (InventoryMax.activeSelf == true)
            {

            }
        }
        
    }  
    public void OnPointerUp(PointerEventData data)
    {
        if (MoveSlotNum < 0)
            return;
        if (shop.gameObject.activeSelf == true)
            return;
        WindowDrag = false;
        
        //if(Input.GetMouseButtonUp(0) && ClickPos == data.position && ItemInfo == true)   // 좌 클릭 시
        //{            
        //    if (MoveListType == ItemListType.QUICK)
        //    {
        //        InfoPreset = new Vector3(data.position.x + 75f, data.position.y + 100f, 0);
        //    }
        //    else
        //    {
        //        InfoPreset = new Vector3(data.position.x + 75f, data.position.y - 100f, 0);
        //    }            
        //    ItemInfo = false;
            
        //    End_Click_L(InfoPreset);
        //    DragFail();
        //    return;
            
        //}
        //else if (Input.GetMouseButtonUp(1) && ClickPos == data.position && isClick == true) // 우 클릭 시
        //{
        //    isClick = false;
        //    End_Click_R();
        //    return;                    
            
        //}


        if (Input.GetMouseButtonUp(0))
        {
            if (InventoryMax.activeSelf == false)   // 인벤토리가 꺼져있을때
            {
                if (Quick.ClickUpQuick_Item(EndItemSetting, data.position))
                    return;
                if (MoveSlotNum >= 0 && MoveIcon.gameObject.activeSelf == true)
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
                if (MoveSlotNum >= 0 && MoveIcon.gameObject.activeSelf == true)
                {
                    DragFail();
                    return;
                }
            }


        }
    } 
    
    //
    
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
   
    public void End_Click_R()       //자동 장착
    {
        
    }
    public void End_Click_L(Vector3 Pos)    //정보 표시
    {
        
        //if(Clickitem != null && ClickitemImage != null)
        //{
        //    miniinfo.gameObject.SetActive(true);
        //    miniinfo.gameObject.transform.position = Pos;
        //    miniinfo.ItemImage.sprite = ClickitemImage.sprite;
        //    miniinfo.ItemName.text = Clickitem.ItemName;
        //    miniinfo.ItemType.text = Clickitem.itemType.ToString();
        //    miniinfo.ExPlain.text = Clickitem.ItemExplain;


        //    if (Clickitem.ItemProperty == "")
        //        miniinfo.Property.text = "";
        //    else
        //    {
        //        string[] tmp = Clickitem.ItemProperty.Split('/');
        //        miniinfo.Property.text = tmp[0] + " + " + tmp[1];
        //    }
           
        //}

    }  

}
