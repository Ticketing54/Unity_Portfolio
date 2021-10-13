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
    int MoveItemIndex;
    [SerializeField]
    int MoveSlotNum;
    [SerializeField]
    int LastSlotNum;
    [SerializeField]
    int QuickSlotListNumber = 0;
    [SerializeField]
    Vector2 ClickPos;
    public int WorkingSlot = -1;
    


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
        MoveListType = _Type;       
        MoveItemIndex = Character.Player.Quick.GetItem(QuickSlotListNumber, _Num).Index;
        MoveSlotNum = _Num;
        _Mouse();

    }
    void ClickUp(int _Num, ListType _Type, DragType _drag)
    {
        LastSlotNum = _Num;

    }    
    void DragFail()
    {
        LastSlotNum = -1;        
        MoveList[MoveSlotNum].Add();
        MoveInfoReset();
    }
    void DragSuccess()
    {
        //장비창일때
        //퀵슬롯일때
        //인벤일때
        Character.Player.Inven.Swap(MoveSlotNum, LastSlotNum);

    }
    

    void RightClick()
    {
        isClick = true;
    }
    void LeftClick()
    {
        switch (MoveListType)
        {
            case ListType.EQUIP:
                MoveList = Equip;
                break;
            case ListType.INVEN:
                MoveList = Inven;
                break;
            case ListType.QUICK:
                MoveList = Quick;
                break;
            default:
                return;
        }
        MoveIcon.gameObject.SetActive(true);
        MoveIcon.sprite = MoveList[MoveSlotNum].ICON;
        MoveIcon.transform.position = ClickPos;
        MoveList[MoveSlotNum].Clear();        
    }   
    void MoveInfoReset()
    {
        MoveListType = ListType.NONE;
        MoveItemIndex = -1;
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
        if (shop.gameObject.activeSelf == true)
            return;
        WindowDrag = false;
        
        if(Input.GetMouseButtonUp(0) && ClickPos == data.position && ItemInfo == true)   // 좌 클릭 시
        {
            if (WorkingSlot < 0)
                return;
            Vector3 Pos;
            if (OldSlotList == Quick)
            {
                Pos = new Vector3(data.position.x + 75f, data.position.y + 100f, 0);
            }
            else
            {
                Pos = new Vector3(data.position.x + 75f, data.position.y - 100f, 0);
            }
            End_Drag_Empty(OldSlotList, WorkingSlot);
            ItemInfo = false;
            
            End_Click_L(Pos);
            return;
            
        }
        else if (Input.GetMouseButtonUp(1) && ClickPos == data.position && isClick == true) // 우 클릭 시
        {
            isClick = false;
            End_Click_R();
            return;                    
            
        }

        if (WorkingSlot < 0)
            return;
        if (Input.GetMouseButtonUp(0))
        {
            if (InventoryMax.activeSelf == false)   // 인벤토리가 꺼져있을때
            {
                for (int i = 0; i < Quick.Count; i++)
                {
                    if (Quick[i].isInRect(data.position))
                    {
                        if (!Quick[i].ActiveIcon())
                        {
                            SoundManager.soundmanager.soundsPlay("Pick");
                            End_Drag_Empty(Quick, i);
                            return;
                        }
                        else if(Quick[i].item.Index == Moveitem.Index)
                        {
                            SoundManager.soundmanager.soundsPlay("Pick");
                            End_Drag_Same(Quick, i);
                            return;
                        }
                        else if (Quick[i].ActiveIcon())
                        {
                            SoundManager.soundmanager.soundsPlay("Pick");
                            End_Drag_Different(Quick, i);
                            return;
                        }
                    }                   
                }
                if (WorkingSlot >= 0 && MoveIcon.gameObject.activeSelf == true)
                {
                    SoundManager.soundmanager.soundsPlay("Pick");
                    End_Drag_Empty(OldSlotList, WorkingSlot);

                }
            }
            else if (InventoryMax.activeSelf == true)  // 인벤토리 켜져있을때
            {

                for (int i = 0; i < Inven.Count; i++)
                {
                    if (Inven[i].isInRect(data.position))
                    {
                        if (!Inven[i].ActiveIcon())
                        {
                            SoundManager.soundmanager.soundsPlay("Pick");
                            End_Drag_Empty(Inven, i);
                            return;
                        }
                        else if (Inven[i].item.Index == Moveitem.Index)
                        {
                            SoundManager.soundmanager.soundsPlay("Pick");
                            End_Drag_Same(Inven, i);
                            return;
                        }
                        else if (Inven[i].ActiveIcon())
                        {
                            SoundManager.soundmanager.soundsPlay("Pick");
                            End_Drag_Different(Inven, i);
                            return;
                        }
                    }
                }

                for (int j = 0; j < Quick.Count; j++)
                {
                    if (Quick[j].isInRect(data.position))
                    {
                        if (!Quick[j].ActiveIcon())
                        {
                            SoundManager.soundmanager.soundsPlay("Pick");
                            End_Drag_Empty(Quick, j);
                            return;
                        }
                        else if (Quick[j].item.Index == Moveitem.Index)
                        {
                            SoundManager.soundmanager.soundsPlay("Pick");
                            End_Drag_Same(Quick, j);
                            return;
                        }
                        else if (Quick[j].ActiveIcon())
                        {
                            SoundManager.soundmanager.soundsPlay("Pick");
                            End_Drag_Different(Quick, j);
                            return;
                        }
                    }
                }

                for (int k = 0; k < Equip.Count; k++)
                {
                    if (Equip[k].isInRect(data.position))
                    {
                        if (!Equip[k].ActiveIcon())
                        {
                            SoundManager.soundmanager.soundsPlay("Pick");
                            End_Drag_Empty(Equip, k);
                            return;
                        }
                        else if (Equip[k].ActiveIcon())
                        {
                            SoundManager.soundmanager.soundsPlay("Pick");
                            End_Drag_Different(Equip, k);
                            return;
                        }
                    }
                }

                if (WorkingSlot >= 0 && MoveIcon.gameObject.activeSelf == true)
                {
                    SoundManager.soundmanager.soundsPlay("Pick");
                    End_Drag_Empty(OldSlotList, WorkingSlot);
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
   
    public void End_Click_R()
    {
        if (Clickitem.itemType == Item.ItemType.Equipment)
        {
            Begin_DragSlot(OldSlotList, WorkingSlot);

            if (OldSlotList == Equip)
            {
                Clickitem = null;
                ClickitemImage = null;
                for (int i = 0; i < Inven.Count; i++)
                {
                    if (Inven[i].Icon.gameObject.activeSelf == false)
                    {

                        End_Drag_Empty(Inven, i);
                        return;
                    }
                }
                End_Drag_Empty(OldSlotList, WorkingSlot);
            }
            else
            {
                if (Equip[(int)Clickitem.EquipType].Icon.gameObject.activeSelf == false)
                {
                    End_Drag_Empty(Equip, (int)Clickitem.EquipType);
                    Clickitem = null;
                    ClickitemImage = null;
                    return;

                }
                else if (Equip[(int)Clickitem.EquipType].Icon.gameObject.activeSelf == true)
                {
                    End_Drag_Different(Equip, (int)Clickitem.EquipType);
                    Clickitem = null;
                    ClickitemImage = null;
                    return;
                }


            }

        }
    }
    public void End_Click_L(Vector3 Pos)
    {
        if(Clickitem != null && ClickitemImage != null)
        {
            miniinfo.gameObject.SetActive(true);
            miniinfo.gameObject.transform.position = Pos;
            miniinfo.ItemImage.sprite = ClickitemImage.sprite;
            miniinfo.ItemName.text = Clickitem.ItemName;
            miniinfo.ItemType.text = Clickitem.itemType.ToString();
            miniinfo.ExPlain.text = Clickitem.ItemExplain;


            if (Clickitem.ItemProperty == "")
                miniinfo.Property.text = "";
            else
            {
                string[] tmp = Clickitem.ItemProperty.Split('/');
                miniinfo.Property.text = tmp[0] + " + " + tmp[1];
            }
           
        }

    }
    
    public void End_Drag_Same(List<Slot> _list, int _num)  // 드래그 목표가 같을 때
    {
        if(_list[_num].item.itemType == Item.ItemType.Equipment)
        {
            
            End_Drag_Different(_list, _num);

            return;
        }


        _list[_num].item.ItemCount += Moveitem.ItemCount;
        _list[_num].SetSlotCount();
        Moveitem = null;
        MoveIcon.gameObject.SetActive(false);
        WorkingSlot = -1;
    }
    public void End_Drag_Different(List<Slot> _list, int _num) // 드래그 목표가 다를 때
    {
        if (_list == Equip)
        {
            if (Moveitem.itemType == Item.ItemType.Equipment && (int)Moveitem.EquipType == _num)
            {
                if ((int)Moveitem.EquipType == _num)
                {
                    SoundManager.soundmanager.soundsPlay("Equip");
                    ApplyStatus(Moveitem);
                    CancleItem(_list[_num].item);
                    OldSlotList[WorkingSlot].item = _list[_num].item;
                    OldSlotList[WorkingSlot].item.SlotNum = WorkingSlot;
                    OldSlotList[WorkingSlot].Icon.gameObject.SetActive(true);
                    OldSlotList[WorkingSlot].Icon.sprite = _list[_num].Icon.sprite;
                    OldSlotList[WorkingSlot].SetSlotCount();


                    _list[_num].item = Moveitem;
                    _list[_num].item.SlotNum = _num;
                    _list[_num].Icon.sprite = MoveIcon.sprite;
                    _list[_num].SetSlotCount();
                    Moveitem = null;
                    MoveIcon.gameObject.SetActive(false);
                    WorkingSlot = -1;
                    return;
                }
                else
                {
                    End_Drag_Empty(OldSlotList, WorkingSlot);
                    return;
                }



            }
            else
            {
                End_Drag_Empty(OldSlotList, WorkingSlot);
                return;
            }
        }
        else if(_list == Quick)
        {
            if(Moveitem.itemType != Item.ItemType.Used)
            {
                End_Drag_Empty(OldSlotList, WorkingSlot);
                return;
            }
        }
        if (OldSlotList == Quick && _list[_num].item.itemType != Item.ItemType.Used)
        {
            End_Drag_Empty(OldSlotList, WorkingSlot);
            return;
        }





        OldSlotList[WorkingSlot].item = _list[_num].item;
        OldSlotList[WorkingSlot].item.SlotNum = WorkingSlot;
        OldSlotList[WorkingSlot].Icon.gameObject.SetActive(true);
        OldSlotList[WorkingSlot].Icon.sprite = _list[_num].Icon.sprite;
        OldSlotList[WorkingSlot].SetSlotCount();


        _list[_num].item = Moveitem;
        _list[_num].item.SlotNum = _num;
        _list[_num].Icon.sprite = MoveIcon.sprite;
        _list[_num].SetSlotCount();
        Moveitem = null;
        MoveIcon.gameObject.SetActive(false);
        WorkingSlot = -1;
        OldSlotList = null;

    }
    public void End_Drag_Empty(List<Slot> _list, int _num) // 빈곳에 드래그 했을때 
    {

        if (_list == Equip)
        {
            if (Moveitem.itemType == Item.ItemType.Equipment && (int)Moveitem.EquipType == _num)
            {
                if((int)Moveitem.EquipType == _num)
                {
                    SoundManager.soundmanager.soundsPlay("Equip");
                    ApplyStatus(Moveitem);
                    _list[_num].item = Moveitem;
                    _list[_num].item.SlotNum = _num;
                    _list[_num].Icon.gameObject.SetActive(true);
                    _list[_num].Icon.sprite = MoveIcon.sprite;

                    _list[_num].SetSlotCount();
                    Moveitem = null;
                    MoveIcon.gameObject.SetActive(false);
                    WorkingSlot = -1;
                    return;
                }
                else
                {
                    End_Drag_Empty(OldSlotList, WorkingSlot);
                    return;
                }
               

                
            }
            else
            {
                End_Drag_Empty(OldSlotList, WorkingSlot);
                return;
            }


        }
        else if (_list == Quick)
        {
            if (Moveitem.itemType != Item.ItemType.Used)
            {
                End_Drag_Empty(OldSlotList, WorkingSlot);
                return;
            }
        }



        _list[_num].item = Moveitem;
        _list[_num].item.SlotNum = _num;
        _list[_num].Icon.gameObject.SetActive(true);
        _list[_num].Icon.sprite = MoveIcon.sprite;
        
        _list[_num].SetSlotCount();
        Moveitem = null;
        MoveIcon.gameObject.SetActive(false);
        WorkingSlot = -1;
        OldSlotList = null;

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
    
    public void SlotReset()
    {
        foreach(Slot one in Inven)
        {
            one.Clear();
        }
        foreach (Slot one in Quick)
        {
            one.Clear();
        }
        foreach (Slot one in Equip)
        {
            one.Clear();
        }
    }
    public void ApplyStatus(Item _item)
    {
        string[] tmp = _item.ItemProperty.Split('/');


        if (_item.itemType == Item.ItemType.Equipment)
        {
            if (tmp[0] == "Defend")
            {
                Character.Player.Stat.ATK += float.Parse(tmp[1]);
            }
            else if (tmp[0] == "Atk")
            {
                Character.Player.Stat.ATK += float.Parse(tmp[1]);
            }

        }
        else if (_item.itemType == Item.ItemType.Used)
        {
            if (tmp[0] == "Hp")
            {
                Character.Player.Stat.HP += float.Parse(tmp[1]);
            }
            else if (tmp[0] == "Mp")
            {
                Character.Player.Stat.HP += float.Parse(tmp[1]);
            }

        }

        if (Character.Player.Stat.HP >= Character.Player.Stat.MAXHP)
            Character.Player.Stat.HP = Character.Player.Stat.MAXHP;
        if (Character.Player.Stat.MP >= Character.Player.Stat.MAXMP)
            Character.Player.Stat.MP = Character.Player.Stat.MAXMP;

    }   //장비템 효과 적용
    public void CancleItem(Item _item)
    {
        string[] tmp = _item.ItemProperty.Split('/');
        SoundManager.soundmanager.soundsPlay("Equip");

        if (_item.itemType == Item.ItemType.Equipment)
        {
            if (tmp[0] == "Defend")
            {
                Character.Player.Stat.MAXHP -= float.Parse(tmp[1]);
            }
            else if (tmp[0] == "Atk")
            {
                Character.Player.Stat.ATK -= float.Parse(tmp[1]);
            }
        }

        if (Character.Player.Stat.HP >= Character.Player.Stat.MAXHP)
            Character.Player.Stat.HP= Character.Player.Stat.MAXHP;
        if (Character.Player.Stat.MP >= Character.Player.Stat.MAXMP)
            Character.Player.Stat.MP = Character.Player.Stat.MAXMP;
    }   // 장비템 효과 적용



    public void getItem(Item _item)
    {
        for(int i = 0; i < Inven.Count; i++)
        {
            if(Inven[i].Icon.gameObject.activeSelf == false)
            {
                Inven[i].Add(_item);
                return;
            }
        }
    }
}
