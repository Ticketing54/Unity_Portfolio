using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropBox : MonoBehaviour //IPointerDownHandler,IDragHandler, IPointerUpHandler
{
    public List<Slot> DropSlotlist = new List<Slot>();   
    public MiniInfo miniinfo;        


    public MoveWindow moveWindow;
    bool WindowDrag = false;
    Vector2 Window_Preset = Vector2.zero;
    public RectTransform Window;


    public Vector3 Pos = Vector3.zero;

    private void Update()
    {

        if (this.gameObject.activeSelf == true)
        {
            //UIManager.uimanager.InventoryActive = true;
            //if (UIManager.uimanager.Inven.gameObject.activeSelf == false)
            //    UIManager.uimanager.Inven.gameObject.SetActive(true);

        }
            
        


    }
    //public void OnPointerDown(PointerEventData data)
    //{
    //    if (miniinfo.gameObject.activeSelf == true)
    //        miniinfo.gameObject.SetActive(false);



    //    if (Input.GetMouseButtonDown(0))
    //    {

    //        if (moveWindow.isInRect(data.position))
    //        {
    //            WindowDrag = true;
    //            Window_Preset = data.position - (Vector2)Window.position;

    //        }
           


    //        if (miniinfo.gameObject.activeSelf == true)
    //            miniinfo.gameObject.SetActive(false);


    //    }
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        for (int i = 0; i < DropSlotlist.Count; i++)
    //        {
    //            if (DropSlotlist[i].isInRect(data.position) && DropSlotlist[i].Icon.gameObject.activeSelf == true)
    //            {
                    
    //                for(int j = 0; j < Inven.Inven.Count; j++)
    //                {
    //                    if (Inven.Inven[j].item != null&&DropSlotlist[i].item.Index == Inven.Inven[j].item.Index)
    //                    {
    //                        Inven.Inven[j].item.ItemCount += DropSlotlist[i].item.ItemCount;
    //                        Inven.Inven[j].SetSlotCount();
    //                        DropSlotlist[i].Clear();
    //                        return;
    //                    }

    //                }

    //                for (int k = 0; k < Inven.Inven.Count; k++)
    //                {
    //                    if (Inven.Inven[k].Icon.gameObject.activeSelf == false)
    //                    {
    //                        Inven.Inven[k].item = DropSlotlist[i].item;
    //                        Inven.Inven[k].item.SlotNum = k;
    //                        Inven.Inven[k].Icon.gameObject.SetActive(true);
    //                        Inven.Inven[k].Icon.sprite = DropSlotlist[i].Icon.sprite;
    //                        DropSlotlist[i].Clear();
    //                        Inven.Inven[k].SetSlotCount();
    //                        return;
    //                    }
    //                }

    //            }
                  


    //        }
    //    }



    //}

    //public void AllItemRecieve()
    //{
    //    foreach(Slot one in DropSlotlist)
    //    {
    //        bool itemExist = false;
    //        if (one.item == null)
    //            return;
    //        for (int i = 0; i < Inven.Inven.Count; i++)
    //        {
    //            if(Inven.Inven[i].item != null && one.item.Index == Inven.Inven[i].item.Index)
    //            {
    //                Inven.Inven[i].item.ItemCount += one.item.ItemCount;
    //                Inven.Inven[i].SetSlotCount();
    //                one.Clear();
    //                itemExist = true;
    //                break;
    //            }               
    //        }
    //        if (itemExist == true)
    //            continue;
    //        for (int j = 0; j < Inven.Inven.Count; j++)
    //        {
    //            if (Inven.Inven[j].Icon.gameObject.activeSelf == false)
    //            {
    //                Inven.Inven[j].item = one.item;
    //                Inven.Inven[j].item.SlotNum = j;
    //                Inven.Inven[j].Icon.gameObject.SetActive(true);
    //                Inven.Inven[j].Icon.sprite = one.Icon.sprite;
    //                one.Clear();
    //                Inven.Inven[j].SetSlotCount();
    //                break;


    //            }
    //        }
           

    //    }
    //}
    //public void CloseDropBox()   //슬롯 클리어
    //{
    //    for(int i = 0; i < DropSlotlist.Count; i++)
    //    {
    //        DropSlotlist[i].Clear();
    //    }

    //    if (miniinfo.gameObject.activeSelf == true)
    //        miniinfo.gameObject.SetActive(false);


    //    this.gameObject.SetActive(false);
    //}
    //public void AddItem(string _itemlist)
    //{
    //    string[] items = _itemlist.Split('/');


    //    for(int i = 0; i < items.Length-1; i++)
    //    {
    //        string[] dropitem = items[i].Split('#');            
    //        for(int j=0; j < int.Parse(dropitem[1]); j++)
    //        {
    //            float percent = Random.Range(0, 100);
    //            if (percent < float.Parse(dropitem[2]))
    //            {
    //                bool isDone = false;
    //                for(int a = 0; a < DropSlotlist.Count; a++)
    //                {
    //                    if(DropSlotlist[a].item  != null && DropSlotlist[a].item.Index == int.Parse(dropitem[0]))
    //                    {
    //                        DropSlotlist[a].item.ItemCount += 1;
    //                        DropSlotlist[a].SetSlotCount();
    //                        isDone = true;
    //                        break;
    //                    }
    //                }
    //                if(isDone == true)
    //                {
    //                    continue;
                        
    //                }
    //                for(int b = 0; b < DropSlotlist.Count; b++)
    //                {
    //                    if(DropSlotlist[b].Icon.gameObject.activeSelf == false)
    //                    {

    //                        List<string> iteminfo = ItemTableManager.instance.Item_Table.GetData(int.Parse(dropitem[0]));
    //                        Item tmp = new Item(int.Parse(iteminfo[0]), int.Parse(iteminfo[1]), iteminfo[2], iteminfo[3], iteminfo[4], iteminfo[5], int.Parse(iteminfo[6]), int.Parse(iteminfo[7]));
    //                        tmp.ItemCount = int.Parse(dropitem[1]);
    //                        DropSlotlist[b].Add(tmp);
    //                        break;

    //                    }                       
    //                }                   
    //            }
    //            else
    //            {
    //                continue;
    //            }



    //        }


            

    //    }




    //}

    //public void OnDrag(PointerEventData data)
    //{
    //    if (WindowDrag == true)
    //    {

    //        Window.position = data.position - Window_Preset;
    //    }
    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    WindowDrag = false;
    //}
}
