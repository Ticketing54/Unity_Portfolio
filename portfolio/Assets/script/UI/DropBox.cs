using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropBox : MonoBehaviour, IPointerDownHandler,IDragHandler, IPointerUpHandler
{
    public List<Slot> DropSlotlist = new List<Slot>();   
    public MiniInfo miniinfo;    
    public InventoryUI Inven;


    public MoveWindow moveWindow;
    bool WindowDrag = false;
    Vector2 Window_Preset = Vector2.zero;
    public RectTransform Window;


    public Vector3 Pos = Vector3.zero;

    private void Update()
    {

        if (this.gameObject.activeSelf == true)
        {
            UIManager.uimanager.InventoryActive = true;
            if (UIManager.uimanager.Inven.gameObject.activeSelf == false)
                UIManager.uimanager.Inven.gameObject.SetActive(true);

        }
            
        


    }
    public void OnPointerDown(PointerEventData data)
    {
        if (miniinfo.gameObject.activeSelf == true)
            miniinfo.gameObject.SetActive(false);



        if (Input.GetMouseButtonDown(0))
        {

            if (moveWindow.isInRect(data.position))
            {
                WindowDrag = true;
                Window_Preset = data.position - (Vector2)Window.position;

            }
            for (int i = 0; i < DropSlotlist.Count; i++)
            {
                if (DropSlotlist[i].isInRect(data.position)&& DropSlotlist[i].image.gameObject.activeSelf == true)
                {
                    Pos = new Vector3(data.position.x + 75f, data.position.y - 100f, 0);
                    miniinfo.gameObject.SetActive(true);
                    miniinfo.gameObject.transform.position = Pos;
                    miniinfo.ItemImage.sprite = DropSlotlist[i].image.sprite;
                    miniinfo.ItemName.text = DropSlotlist[i].item.ItemName;
                    miniinfo.ItemType.text = DropSlotlist[i].item.itemType.ToString();
                    miniinfo.ExPlain.text = DropSlotlist[i].item.ItemExplain;

                    if (DropSlotlist[i].item.ItemProperty == "")
                        miniinfo.Property.text = "";
                    else
                    {
                        string[] tmp = DropSlotlist[i].item.ItemProperty.Split('/');
                        miniinfo.Property.text = tmp[0] + " + " + tmp[1];
                    }
                    
                    return;
                }
                

            }


            if (miniinfo.gameObject.activeSelf == true)
                miniinfo.gameObject.SetActive(false);


        }
        if (Input.GetMouseButtonDown(1))
        {
            for (int i = 0; i < DropSlotlist.Count; i++)
            {
                if (DropSlotlist[i].isInRect(data.position) && DropSlotlist[i].image.gameObject.activeSelf == true)
                {
                    
                    for(int j = 0; j < Inven.list.Count; j++)
                    {
                        if (Inven.list[j].item != null&&DropSlotlist[i].item.Index == Inven.list[j].item.Index)
                        {
                            Inven.list[j].item.ItemCount += DropSlotlist[i].item.ItemCount;
                            Inven.list[j].SetSlotCount();
                            DropSlotlist[i].Clear();
                            return;
                        }

                    }

                    for (int k = 0; k < Inven.list.Count; k++)
                    {
                        if (Inven.list[k].image.gameObject.activeSelf == false)
                        {
                            Inven.list[k].item = DropSlotlist[i].item;
                            Inven.list[k].item.SlotNum = k;
                            Inven.list[k].image.gameObject.SetActive(true);
                            Inven.list[k].image.sprite = DropSlotlist[i].image.sprite;
                            DropSlotlist[i].Clear();
                            Inven.list[k].SetSlotCount();
                            return;
                        }
                    }

                }
                  


            }
        }



    }

    public void AllItemRecieve()
    {
        foreach(Slot one in DropSlotlist)
        {
            bool itemExist = false;
            if (one.item == null)
                return;
            for (int i = 0; i < Inven.list.Count; i++)
            {
                if(Inven.list[i].item != null && one.item.Index == Inven.list[i].item.Index)
                {
                    Inven.list[i].item.ItemCount += one.item.ItemCount;
                    Inven.list[i].SetSlotCount();
                    one.Clear();
                    itemExist = true;
                    break;
                }               
            }
            if (itemExist == true)
                continue;
            for (int j = 0; j < Inven.list.Count; j++)
            {
                if (Inven.list[j].image.gameObject.activeSelf == false)
                {
                    Inven.list[j].item = one.item;
                    Inven.list[j].item.SlotNum = j;
                    Inven.list[j].image.gameObject.SetActive(true);
                    Inven.list[j].image.sprite = one.image.sprite;
                    one.Clear();
                    Inven.list[j].SetSlotCount();
                    break;


                }
            }
           

        }
    }
    public void CloseDropBox()   //슬롯 클리어
    {
        for(int i = 0; i < DropSlotlist.Count; i++)
        {
            DropSlotlist[i].Clear();
        }

        if (miniinfo.gameObject.activeSelf == true)
            miniinfo.gameObject.SetActive(false);


        this.gameObject.SetActive(false);
    }
    public void AddItem(string _itemlist)
    {
        string[] items = _itemlist.Split('/');


        for(int i = 0; i < items.Length-1; i++)
        {
            string[] dropitem = items[i].Split('#');            
            for(int j=0; j < int.Parse(dropitem[1]); j++)
            {
                float percent = Random.Range(0, 100);
                if (percent < float.Parse(dropitem[2]))
                {
                    bool isDone = false;
                    for(int a = 0; a < DropSlotlist.Count; a++)
                    {
                        if(DropSlotlist[a].item  != null && DropSlotlist[a].item.Index == int.Parse(dropitem[0]))
                        {
                            DropSlotlist[a].item.ItemCount += 1;
                            DropSlotlist[a].SetSlotCount();
                            isDone = true;
                            break;
                        }
                    }
                    if(isDone == true)
                    {
                        continue;
                        
                    }
                    for(int b = 0; b < DropSlotlist.Count; b++)
                    {
                        if(DropSlotlist[b].image.gameObject.activeSelf == false)
                        {

                            List<string> iteminfo = ItemTableManager.instance.Item_Table.GetData(int.Parse(dropitem[0]));
                            Item tmp = new Item(int.Parse(iteminfo[0]), int.Parse(iteminfo[1]), iteminfo[2], iteminfo[3], iteminfo[4], iteminfo[5], int.Parse(iteminfo[6]), int.Parse(iteminfo[7]));
                            tmp.ItemCount = int.Parse(dropitem[1]);
                            DropSlotlist[b].Add(tmp);
                            break;

                        }                       
                    }                   
                }
                else
                {
                    continue;
                }



            }


            

        }




    }

    public void OnDrag(PointerEventData data)
    {
        if (WindowDrag == true)
        {

            Window.position = data.position - Window_Preset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        WindowDrag = false;
    }
}
