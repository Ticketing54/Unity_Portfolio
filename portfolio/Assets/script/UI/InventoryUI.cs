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
public class InventoryUI : MonoBehaviour,IPointerUpHandler, IPointerDownHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
{ 
    [SerializeField]
    List<ItemSlot> Inven = new List<ItemSlot>();           // 인벤토리
    [SerializeField]
    List<ItemSlot> Quick = new List<ItemSlot>();           // 퀵 슬롯
    [SerializeField]
    List<ItemSlot> Equip = new List<ItemSlot>();           // 장비창    

    public delegate void MouseClickDown();
    
    
    [SerializeField]
    List<ItemSlot> MoveList;        // 드래그중인 List    
    [SerializeField]
    ItemListType MoveListType;      // 드래그중인 List 종류
    [SerializeField]
    Item MoveItem;                  // 드래그중인 Item
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

    #region Click
    // 눌렀을 때
    void ClickDown(int _Num, ItemListType _Type, MouseClickDown _Mouse)
    {
        switch (_Type)
        {
            case ItemListType.EQUIP:
                MoveList = Equip;
                MoveItem = Character.Player.Equip.GetItem(_Num);
                break;
            case ItemListType.INVEN:
                MoveList = Inven;
                MoveItem = Character.Player.Inven.GetItem(_Num);
                break;
            case ItemListType.QUICK:
                MoveList = Quick;
                MoveItem = Character.Player.Quick.GetItem(_Num);
                break;
            default:
                return;
        }
        MoveListType = _Type;
        MoveSlotNum = _Num;
        _Mouse();

    }
    // 우 클릭
    void RightClick()
    {
        isClick = true;
    }
    // 좌 클릭
    void LeftClick()
    {
        MoveIcon.gameObject.SetActive(true);
        MoveIcon.sprite = MoveList[MoveSlotNum].ICON;
        MoveIcon.transform.position = ClickPos;
        MoveList[MoveSlotNum].Clear();
    }
    #endregion
    #region Drag
    //드래그 실패
    void DragFail()
    {
        LastSlotNum = -1;
        MoveList[MoveSlotNum].Add(MoveListType);
        MoveInfoReset();
    }
    //드래그 성공
    void DragSuccess(List<ItemSlot> _ui, int _Num, ItemListType _EndListType)
    {
        LastSlotNum = _Num;
        switch (MoveListType)
        {
            case ItemListType.EQUIP:
                MoveEquipTo(_EndListType);
                break;
            case ItemListType.QUICK:
                MoveQuickTo(_EndListType);
                break;
            case ItemListType.INVEN:
                MoveInvenTo(_EndListType);
                break;
            default:
                break;
        }
        UpdateItemSlot(_ui, _Num, _EndListType);
        LastSlotNum = -1;
    }
    void MoveEquipTo(ItemListType _EndListType)
    {
        if (_EndListType != ItemListType.INVEN)
            return;
        Character.Player.Equip.StartItemMove(MoveSlotNum, _EndListType, LastSlotNum, Character.Player.Inven.AriveItem);
    }
    void MoveQuickTo(ItemListType _EndListType)
    {
        switch (_EndListType)
        {
            case ItemListType.INVEN:
                Character.Player.Equip.StartItemMove(MoveSlotNum, _EndListType, LastSlotNum, Character.Player.Inven.AriveItem);
                break;
            case ItemListType.QUICK:
                Character.Player.Equip.StartItemMove(MoveSlotNum, _EndListType, LastSlotNum, Character.Player.Quick.AriveItem);
                break;
            default:
                break;
        }
    }
    void MoveInvenTo(ItemListType _EndListType)
    {
        switch (_EndListType)
        {
            case ItemListType.INVEN:
                Character.Player.Inven.StartItemMove(MoveSlotNum, _EndListType, LastSlotNum, Character.Player.Inven.AriveItem);
                break;
            case ItemListType.QUICK:
                Character.Player.Inven.StartItemMove(MoveSlotNum, _EndListType, LastSlotNum, Character.Player.Quick.AriveItem);
                break;
            case ItemListType.EQUIP:
                Character.Player.Inven.StartItemMove(MoveSlotNum, _EndListType, LastSlotNum, Character.Player.Equip.AriveItem);
                break;
            default:
                break;
        }
    }
    #endregion
    #region ItemMoveUpdate
    // 화면 갱신
    void UpdateItemSlot(List<ItemSlot> _List, int _Num, ItemListType _listType)
    {
        _List[_Num].Add(_listType);
        MoveList[MoveSlotNum].Add(MoveListType);
        MoveInfoReset();
    }
    // Move 정보 리셋
    void MoveInfoReset()
    {
        MoveListType = ItemListType.NONE;
        MoveItem = null;
        MoveSlotNum = -1;
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
                for (int i = 0; i < Quick.Count; i++)
                {
                    if (Quick[i].isInRect(data.position) && Quick[i].ActiveIcon())
                    {
                        ClickDown(i, ItemListType.QUICK, LeftClick);                   
                        ItemInfo = true;                        
                        return;
                    }

                }
            }
            else if (InventoryMax.activeSelf == true)
            {
                if (movewindow.isInRect(data.position))
                {
                    WindowDrag = true;
                    Window_Preset = data.position - (Vector2)Window.position;

                }
                for (int j = 0; j < Quick.Count; j++)
                {
                    if (Quick[j].isInRect(data.position) && Quick[j].ActiveIcon())
                    {
                        ClickDown(j, ItemListType.QUICK, LeftClick);                        
                        ItemInfo = true;                                          
                        return;
                    }

                }
                for (int k = 0; k < Equip.Count; k++)
                {
                    if (Equip[k].isInRect(data.position) && Equip[k].ActiveIcon())
                    {
                        ClickDown(k, ItemListType.EQUIP,LeftClick);
                        ItemInfo = true;                        
                        return;
                    }
                   
                }
                for (int i = 0; i < Inven.Count; i++)
                {
                    if (Inven[i].isInRect(data.position) && Inven[i].ActiveIcon())
                    {
                        ClickDown(i, ItemListType.INVEN, LeftClick);
                        ItemInfo = true;                                     
                        return;
                    }

                }
                

            }
            
        }
        if (Input.GetMouseButtonDown(1))
        {
            
            if (InventoryMax.activeSelf == false)
            {
                for (int i = 0; i < Quick.Count; i++)
                {
                    if (Quick[i].isInRect(data.position) && Quick[i].ActiveIcon())
                    {
                        ClickDown(i, ItemListType.QUICK, RightClick);                        
                        return;
                    }

                }
            }
            else if (InventoryMax.activeSelf == true)
            {
                for (int j = 0; j < Quick.Count; j++)
                {
                    if (Quick[j].isInRect(data.position) && Quick[j].ActiveIcon())
                    {
                        
                        ClickDown(j, ItemListType.QUICK, RightClick);
                        return;
                    }

                }
                for (int k = 0; k < Equip.Count; k++)
                {
                    if (Equip[k].isInRect(data.position) && Equip[k].ActiveIcon())
                    {
                        ClickDown(k, ItemListType.EQUIP, RightClick);
                        return;
                    }
                    
                }
                for (int i = 0; i < Inven.Count; i++)
                {
                    if (Inven[i].isInRect(data.position) && Inven[i].ActiveIcon())
                    {
                        ClickDown(i, ItemListType.INVEN, RightClick);
                        return;
                    }

                }
                


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
        
        if(Input.GetMouseButtonUp(0) && ClickPos == data.position && ItemInfo == true)   // 좌 클릭 시
        {            
            if (MoveListType == ItemListType.QUICK)
            {
                InfoPreset = new Vector3(data.position.x + 75f, data.position.y + 100f, 0);
            }
            else
            {
                InfoPreset = new Vector3(data.position.x + 75f, data.position.y - 100f, 0);
            }            
            ItemInfo = false;
            
            End_Click_L(InfoPreset);
            DragFail();
            return;
            
        }
        else if (Input.GetMouseButtonUp(1) && ClickPos == data.position && isClick == true) // 우 클릭 시
        {
            isClick = false;
            End_Click_R();
            return;                    
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (InventoryMax.activeSelf == false)   // 인벤토리가 꺼져있을때
            {
                for (int i = 0; i < Quick.Count; i++)
                {
                    if (Quick[i].isInRect(data.position))
                    {
                        SoundManager.soundmanager.soundsPlay("Pick");
                        DragSuccess(Quick,i, ItemListType.QUICK);
                        return;

                    }                   
                }
                if (MoveSlotNum >= 0 && MoveIcon.gameObject.activeSelf == true)
                {
                    SoundManager.soundmanager.soundsPlay("Pick");
                    DragFail();
                    return;
                }
            }
            else if (InventoryMax.activeSelf == true)  // 인벤토리 켜져있을때
            {

                for (int i = 0; i < Inven.Count; i++)
                {
                    if (Inven[i].isInRect(data.position))
                    {
                        SoundManager.soundmanager.soundsPlay("Pick");
                        DragSuccess(Inven, i, ItemListType.INVEN);
                        return;
                    }
                }

                for (int j = 0; j < Quick.Count; j++)
                {
                    if (Quick[j].isInRect(data.position))
                    {
                        SoundManager.soundmanager.soundsPlay("Pick");
                        DragSuccess(Quick, j, ItemListType.QUICK);
                        return;
                    }
                }

                for (int k = 0; k < Equip.Count; k++)
                {
                    if (Equip[k].isInRect(data.position))
                    {
                        SoundManager.soundmanager.soundsPlay("Pick");
                        DragSuccess(Equip, k, ItemListType.EQUIP);
                        return;
                    }
                }

                if (MoveSlotNum>=0&& MoveIcon.gameObject.activeSelf == true)
                {
                    SoundManager.soundmanager.soundsPlay("Pick");
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
