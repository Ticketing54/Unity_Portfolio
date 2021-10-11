using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class InventoryUI : MonoBehaviour,IPointerUpHandler, IPointerDownHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
{ 
    public List<Slot> list = new List<Slot>();      // 인벤토리
    public List<Slot> Q_list = new List<Slot>();    // 퀵 슬롯
    public List<Slot> E_list = new List<Slot>();    // 장비창    
    

    public MoveWindow movewindow;                   //윈도우창 움직이기
    public RectTransform Window;                    //움직이는 대상
    public bool WindowDrag = false;                 //윈도우 바를 클릭했을때
    public Vector2 Window_Preset = Vector2.zero;
    
    

    public List<Slot> OldSlotList;
    public Vector2 OldClickPos;
    public Image MoveIcon = null;
    public Item Moveitem = null;
    public Item Clickitem = null;
    public Image ClickitemImage = null;
    public int WorkingSlot = -1;
    public Character Player;
    public MiniInfo miniinfo;
    public Shop shop;

    
    public GameObject InventoryMax;
    public GameObject DontClick;


    public bool isClick = false;
    public bool ItemInfo = false;
    
    


    private void Update()
    {
        if(Player == null)
        {
            Player = Character.Player;
        }
      
        
    }

    
    public void OnPointerDown(PointerEventData data)
    {
        if (shop.gameObject.activeSelf == true)
            return;

        OldClickPos = data.position; // 클릭 포인트        
        if(miniinfo.gameObject.activeSelf==true)
            miniinfo.gameObject.SetActive(false);

        if (Input.GetMouseButton(0))
        {            
            if (InventoryMax.activeSelf == false)
            {
              

                for (int i = 0; i < Q_list.Count; i++)
                {
                    if (Q_list[i].isInRect(data.position) && Q_list[i].image.gameObject.activeSelf == true)
                    {
                        Clickitem = Q_list[i].item;
                        ClickitemImage = Q_list[i].image;
                        ItemInfo = true;
                        Begin_DragSlot(Q_list, i);
                        MoveIcon.transform.position = data.position;
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
                for (int j = 0; j < Q_list.Count; j++)
                {
                    if (Q_list[j].isInRect(data.position) && Q_list[j].image.gameObject.activeSelf == true)
                    {
                        Clickitem = Q_list[j].item;
                        ClickitemImage = Q_list[j].image;
                        ItemInfo = true;
                        Begin_DragSlot(Q_list, j);
                        MoveIcon.transform.position = data.position;
                        return;
                    }

                }
                for (int k = 0; k < E_list.Count; k++)
                {
                    if (E_list[k].isInRect(data.position) && E_list[k].image.gameObject.activeSelf == true)
                    {
                        Clickitem = E_list[k].item;
                        ClickitemImage = E_list[k].image;
                        ItemInfo = true;
                        Begin_DragSlot(E_list, k);
                        MoveIcon.transform.position = data.position;
                        return;
                    }
                   
                }
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].isInRect(data.position) && list[i].image.gameObject.activeSelf == true)
                    {
                        Clickitem = list[i].item;
                        ClickitemImage = list[i].image;
                        ItemInfo = true;
                        Begin_DragSlot(list, i);
                        MoveIcon.transform.position = data.position;
                        return;
                    }

                }
                

            }
            
        }
        if (Input.GetMouseButtonDown(1))
        {
            
            if (InventoryMax.activeSelf == false)
            {
                for (int i = 0; i < Q_list.Count; i++)
                {
                    if (Q_list[i].isInRect(data.position) && Q_list[i].image.gameObject.activeSelf == true)
                    {
                        Begin_Click_R(Q_list, i);
                        return;
                    }

                }
            }
            else if (InventoryMax.activeSelf == true)
            {
                for (int j = 0; j < Q_list.Count; j++)
                {
                    if (Q_list[j].isInRect(data.position) && Q_list[j].image.gameObject.activeSelf == true)
                    {
                        Begin_Click_R(Q_list, j);
                        return;
                    }

                }
                for (int k = 0; k < E_list.Count; k++)
                {
                    if (E_list[k].isInRect(data.position) && E_list[k].image.gameObject.activeSelf == true)
                    {
                        Begin_Click_R(E_list, k);
                        return;
                    }
                    
                }
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].isInRect(data.position) && list[i].image.gameObject.activeSelf == true)
                    {
                        Begin_Click_R(list, i);
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
        
        if(Input.GetMouseButtonUp(0) && OldClickPos == data.position && ItemInfo == true)   // 좌 클릭 시
        {
            if (WorkingSlot < 0)
                return;
            Vector3 Pos;
            if (OldSlotList == Q_list)
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
        else if (Input.GetMouseButtonUp(1) && OldClickPos == data.position && isClick == true) // 우 클릭 시
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
                for (int i = 0; i < Q_list.Count; i++)
                {
                    if (Q_list[i].isInRect(data.position) && Q_list[i].image.gameObject.activeSelf == false)
                    {
                        SoundManager.soundmanager.soundsPlay("Pick");
                        End_Drag_Empty(Q_list, i);
                        return;
                    }
                    else if (Q_list[i].isInRect(data.position) && Q_list[i].item.Index == Moveitem.Index)
                    {
                        SoundManager.soundmanager.soundsPlay("Pick");
                        End_Drag_Same(Q_list, i);
                        return;
                    }
                    else if (Q_list[i].isInRect(data.position) && Q_list[i].image.gameObject.activeSelf == true)
                    {
                        SoundManager.soundmanager.soundsPlay("Pick");
                        End_Drag_Different(Q_list, i);
                        return;
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

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].isInRect(data.position) && list[i].image.gameObject.activeSelf == false)
                    {
                        SoundManager.soundmanager.soundsPlay("Pick");
                        End_Drag_Empty(list, i);
                        return;
                    }
                    else if (list[i].isInRect(data.position) && list[i].item.Index == Moveitem.Index)
                    {
                        SoundManager.soundmanager.soundsPlay("Pick");
                        End_Drag_Same(list, i);
                        return;
                    }
                    else if (list[i].isInRect(data.position) && list[i].image.gameObject.activeSelf == true)
                    {
                        SoundManager.soundmanager.soundsPlay("Pick");
                        End_Drag_Different(list, i);
                        return;
                    }

                }

                for (int j = 0; j < Q_list.Count; j++)
                {
                    if (Q_list[j].isInRect(data.position) && Q_list[j].image.gameObject.activeSelf == false)
                    {
                        SoundManager.soundmanager.soundsPlay("Pick");
                        End_Drag_Empty(Q_list, j);
                        return;
                    }
                    else if (Q_list[j].isInRect(data.position) && Q_list[j].item.Index == Moveitem.Index)
                    {
                        SoundManager.soundmanager.soundsPlay("Pick");
                        End_Drag_Same(Q_list, j);
                        return;
                    }
                    else if (Q_list[j].isInRect(data.position) && Q_list[j].image.gameObject.activeSelf == true)
                    {
                        SoundManager.soundmanager.soundsPlay("Pick");
                        End_Drag_Different(Q_list, j);
                        return;
                    }

                }
                for (int k = 0; k < E_list.Count; k++)
                {
                    if (E_list[k].isInRect(data.position) && E_list[k].image.gameObject.activeSelf == false)
                    {
                        SoundManager.soundmanager.soundsPlay("Pick");
                        End_Drag_Empty(E_list, k);
                        return;
                    }
                    else if (E_list[k].isInRect(data.position) && E_list[k].image.gameObject.activeSelf == true)
                    {
                        SoundManager.soundmanager.soundsPlay("Pick");
                        End_Drag_Different(E_list, k);
                        return;
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


    public void Begin_Click_R(List<Slot> _list, int _num)
    {
        Clickitem = _list[_num].item;
        ClickitemImage = _list[_num].image;
        isClick = true;
        OldSlotList = _list;
        WorkingSlot = _num;
    }
    public void End_Click_R()
    {


        if (Clickitem.itemType == Item.ItemType.Equipment)
        {
            Begin_DragSlot(OldSlotList, WorkingSlot);

            if (OldSlotList == E_list)
            {
                Clickitem = null;
                ClickitemImage = null;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].image.gameObject.activeSelf == false)
                    {

                        End_Drag_Empty(list, i);
                        return;
                    }
                }
                End_Drag_Empty(OldSlotList, WorkingSlot);
            }
            else
            {
                if (E_list[(int)Clickitem.EquipType].image.gameObject.activeSelf == false)
                {
                    End_Drag_Empty(E_list, (int)Clickitem.EquipType);
                    Clickitem = null;
                    ClickitemImage = null;
                    return;

                }
                else if (E_list[(int)Clickitem.EquipType].image.gameObject.activeSelf == true)
                {
                    End_Drag_Different(E_list, (int)Clickitem.EquipType);
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
    public void Begin_DragSlot(List<Slot> _list, int _num)  // 드래그 시작
    {
        if(_list == E_list)
        {
            CancleItem(_list[_num].item);
        }

        MoveIcon.gameObject.SetActive(true);
        MoveIcon.sprite = _list[_num].image.sprite;
        Moveitem = _list[_num].item;        
        _list[_num].Clear();
        OldSlotList = _list;
        WorkingSlot = _num;
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
        if (_list == E_list)
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
                    OldSlotList[WorkingSlot].image.gameObject.SetActive(true);
                    OldSlotList[WorkingSlot].image.sprite = _list[_num].image.sprite;
                    OldSlotList[WorkingSlot].SetSlotCount();


                    _list[_num].item = Moveitem;
                    _list[_num].item.SlotNum = _num;
                    _list[_num].image.sprite = MoveIcon.sprite;
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
        else if(_list == Q_list)
        {
            if(Moveitem.itemType != Item.ItemType.Used)
            {
                End_Drag_Empty(OldSlotList, WorkingSlot);
                return;
            }
        }
        if (OldSlotList == Q_list && _list[_num].item.itemType != Item.ItemType.Used)
        {
            End_Drag_Empty(OldSlotList, WorkingSlot);
            return;
        }





        OldSlotList[WorkingSlot].item = _list[_num].item;
        OldSlotList[WorkingSlot].item.SlotNum = WorkingSlot;
        OldSlotList[WorkingSlot].image.gameObject.SetActive(true);
        OldSlotList[WorkingSlot].image.sprite = _list[_num].image.sprite;
        OldSlotList[WorkingSlot].SetSlotCount();


        _list[_num].item = Moveitem;
        _list[_num].item.SlotNum = _num;
        _list[_num].image.sprite = MoveIcon.sprite;
        _list[_num].SetSlotCount();
        Moveitem = null;
        MoveIcon.gameObject.SetActive(false);
        WorkingSlot = -1;
        OldSlotList = null;

    }
    public void End_Drag_Empty(List<Slot> _list, int _num) // 빈곳에 드래그 했을때 
    {

        if (_list == E_list)
        {
            if (Moveitem.itemType == Item.ItemType.Equipment && (int)Moveitem.EquipType == _num)
            {
                if((int)Moveitem.EquipType == _num)
                {
                    SoundManager.soundmanager.soundsPlay("Equip");
                    ApplyStatus(Moveitem);
                    _list[_num].item = Moveitem;
                    _list[_num].item.SlotNum = _num;
                    _list[_num].image.gameObject.SetActive(true);
                    _list[_num].image.sprite = MoveIcon.sprite;

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
        else if (_list == Q_list)
        {
            if (Moveitem.itemType != Item.ItemType.Used)
            {
                End_Drag_Empty(OldSlotList, WorkingSlot);
                return;
            }
        }



        _list[_num].item = Moveitem;
        _list[_num].item.SlotNum = _num;
        _list[_num].image.gameObject.SetActive(true);
        _list[_num].image.sprite = MoveIcon.sprite;
        
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
        foreach(Slot one in list)
        {
            one.Clear();
        }
        foreach (Slot one in Q_list)
        {
            one.Clear();
        }
        foreach (Slot one in E_list)
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
        for(int i = 0; i < list.Count; i++)
        {
            if(list[i].image.gameObject.activeSelf == false)
            {
                list[i].Add(_item);
                return;
            }
        }
    }
}
