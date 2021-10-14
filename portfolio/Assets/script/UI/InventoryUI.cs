using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class InventoryUI : MonoBehaviour,IPointerUpHandler, IPointerDownHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
{ 
    [SerializeField]
    List<ItemSlot> Inven = new List<ItemSlot>();           // 인벤토리
    [SerializeField]
    List<ItemSlot> Quick = new List<ItemSlot>();           // 퀵 슬롯
    [SerializeField]
    public List<ItemSlot> Equip = new List<ItemSlot>();    // 장비창    
    public delegate void MouseClickDown();
    public delegate void DragType();
    

    List<ItemSlot> MoveList;
    public enum ListType
    {
        INVEN,
        QUICK,
        EQUIP,
        NONE
    }
    [SerializeField]
    ListType MoveListType;
    [SerializeField]
    Item MoveItem;
    [SerializeField]
    int MoveSlotNum;
    [SerializeField]
    int LastSlotNum;
    [SerializeField]
    int QuickSlotListNumber = 0;
    [SerializeField]
    Vector2 ClickPos;
    [SerializeField]
    Vector3 InfoPreset;   
    


    public Image MoveIcon = null;
    public MoveWindow movewindow;                   //윈도우창 움직이기
    public RectTransform Window;                    //움직이는 대상
    public bool WindowDrag = false;                 //윈도우 바를 클릭했을때
    public Vector2 Window_Preset = Vector2.zero;

    
    

    public List<ItemSlot> OldSlotList;
    
    
    
    


    public MiniInfo miniinfo;
    public Shop shop;

    
    public GameObject InventoryMax;
    public GameObject DontClick;


    public bool isClick = false;
    public bool ItemInfo = false;
    
    void ClickDown(int _Num, ListType _Type,MouseClickDown _Mouse)
    {
        switch (_Type)
        {
            case ListType.EQUIP:
                MoveList = Equip;
                MoveItem = Character.Player.Equip.GetItem(_Num);
                break;
            case ListType.INVEN:
                MoveList = Inven;
                MoveItem = Character.Player.Inven.GetItem(_Num);
                break;
            case ListType.QUICK:
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
    void DragFail()                 //드래그 실패
    {
        LastSlotNum = -1;        
        MoveList[MoveSlotNum].Add((int)MoveListType);
        MoveInfoReset();
    }
    void DragSuccess(List<ItemSlot> _ui,int _Num, ListType _Type)      //드래그 성공
    {
        LastSlotNum = _Num;
        switch (MoveListType)
        {
            case ListType.EQUIP:
                MoveEquipTo();
                break;
            case ListType.QUICK:
                MoveQuickTo(_Type);
                break;
            case ListType.INVEN:
                MoveInvenTo(_Type);
                break;
            default:
                break;
        }
        UpdateItemSlot(_ui, _Num, (int)_Type);
        LastSlotNum = -1;        
    }
    void MoveEquipTo()
    {
        Character.Player.Equip.StartItemMove(MoveSlotNum, (int)MoveListType,LastSlotNum,Character.Player.Inven.AriveItem);        
    }
    void MoveQuickTo(ListType _Type)
    {
        switch (_Type)
        {
            case ListType.INVEN:
                Character.Player.Equip.StartItemMove(MoveSlotNum, (int)MoveListType, LastSlotNum, Character.Player.Inven.AriveItem);
                break;
            case ListType.QUICK:
                Character.Player.Equip.StartItemMove(MoveSlotNum, (int)MoveListType, LastSlotNum, Character.Player.Quick.AriveItem);
                break;
            default:
                return;
        }        
    }
    void MoveInvenTo(ListType _Type)
    {
        switch (_Type)
        {
            case ListType.INVEN:
                Character.Player.Inven.StartItemMove(MoveSlotNum, (int)MoveListType, LastSlotNum, Character.Player.Inven.AriveItem);
                break;
            case ListType.QUICK:
                Character.Player.Inven.StartItemMove(MoveSlotNum, (int)MoveListType, LastSlotNum, Character.Player.Quick.AriveItem);
                break;
            case ListType.EQUIP:
                Character.Player.Inven.StartItemMove(MoveSlotNum, (int)MoveListType, LastSlotNum, Character.Player.Equip.AriveItem);
                break;
            default:
                return;
        }
    }
    void UpdateItemSlot(List<ItemSlot> _List,int _Num,int _listType) // 화면 갱신
    {        
        _List[_Num].Add(_listType);
        MoveList[MoveSlotNum].Add((int)MoveListType);
        MoveInfoReset();
    }
    void RightClick()
    {
        isClick = true;
    }
    void LeftClick()
    {       
        MoveIcon.gameObject.SetActive(true);
        MoveIcon.sprite = MoveList[MoveSlotNum].ICON;
        MoveIcon.transform.position = ClickPos;
        MoveList[MoveSlotNum].Clear();        
    }   
    void MoveInfoReset()
    {
        MoveListType = ListType.NONE;
        MoveItem = null;
        MoveSlotNum = -1;        
    }
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
                        ClickDown(i, ListType.QUICK, LeftClick);                   
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
                        ClickDown(j, ListType.QUICK, LeftClick);                        
                        ItemInfo = true;                                          
                        return;
                    }

                }
                for (int k = 0; k < Equip.Count; k++)
                {
                    if (Equip[k].isInRect(data.position) && Equip[k].ActiveIcon())
                    {
                        ClickDown(k, ListType.EQUIP,LeftClick);
                        ItemInfo = true;                        
                        return;
                    }
                   
                }
                for (int i = 0; i < Inven.Count; i++)
                {
                    if (Inven[i].isInRect(data.position) && Inven[i].ActiveIcon())
                    {
                        ClickDown(i, ListType.INVEN, LeftClick);
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
                        ClickDown(i, ListType.QUICK, RightClick);                        
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
                        
                        ClickDown(j, ListType.QUICK, RightClick);
                        return;
                    }

                }
                for (int k = 0; k < Equip.Count; k++)
                {
                    if (Equip[k].isInRect(data.position) && Equip[k].ActiveIcon())
                    {
                        ClickDown(k, ListType.EQUIP, RightClick);
                        return;
                    }
                    
                }
                for (int i = 0; i < Inven.Count; i++)
                {
                    if (Inven[i].isInRect(data.position) && Inven[i].ActiveIcon())
                    {
                        ClickDown(i, ListType.INVEN, RightClick);
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
            if (MoveListType == ListType.QUICK)
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
                        DragSuccess(Quick,i, ListType.QUICK);
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
                        DragSuccess(Inven, i, ListType.INVEN);
                        return;
                    }
                }

                for (int j = 0; j < Quick.Count; j++)
                {
                    if (Quick[j].isInRect(data.position))
                    {
                        SoundManager.soundmanager.soundsPlay("Pick");
                        DragSuccess(Quick, j, ListType.QUICK);
                        return;
                    }
                }

                for (int k = 0; k < Equip.Count; k++)
                {
                    if (Equip[k].isInRect(data.position))
                    {
                        SoundManager.soundmanager.soundsPlay("Pick");
                        DragSuccess(Equip, k, ListType.EQUIP);
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
    
    
   
    //public void SlotUpdate()  // 플레이어의 아이템정보를 불러온다.
    //{
    //    for (int i = 0; i < Character.Player.myIven.Count; i++)
    //    {
    //        list[Character.Player.myIven[i].SlotNum].Add(Character.Player.myIven[i]);
    //        list[Character.Player.myIven[i].SlotNum].SetSlotCount();
    //    }

    //    for(int j = 0; j < Character.Player.myEquip.Count; j++)
    //    {
    //        ApplyStatus(Character.Player.myEquip[j]);
    //        E_list[Character.Player.myEquip[j].SlotNum].Add(Character.Player.myEquip[j]);
    //        E_list[Character.Player.myEquip[j].SlotNum].SetSlotCount();
            
    //    }
        
    //}
    
 

}
