using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DropBox : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    [SerializeField]
    List<ItemSlot> dropBoxSlots;
    Character character;
    
    private void Start()
    {   
        UIManager.uimanager.AOpenDropBox += ()=> 
        { 
            gameObject.SetActive(true);
            SetDropBox();
        };
        UIManager.uimanager.AUpdateDropBox += UpdateSlot;
        workingIndex = -1;        
        dropBoxCount =  1;
        gameObject.SetActive(false);
    }

    #region DropBoxMessage

    [SerializeField]
    Image dropBoxCountMessage;
    [SerializeField]
    TextMeshProUGUI countText;
    int workingIndex;    
    int dropBoxCount;
    Item workingItem;

    public void AllReceive()
    {
        if (GameManager.gameManager.character.MoveToDropItem_All())
        {
            CloseDropBox();
        }
        else
        {
            UpdateAllSlot();
        }
    }
    public void OkButton()
    {
        GameManager.gameManager.character.MoveToDropItem(workingIndex, dropBoxCount);
        UpdateSlot(workingIndex);
        CloseDropBoxCountMessage();
    }
    public void NoButton()
    {
        CloseDropBoxCountMessage();
    }
    public void CountUpButton()
    {
        if (workingItem.ItemCount <= dropBoxCount)
        {
            return;
        }

        dropBoxCount++;
        countText.text = dropBoxCount.ToString();
    }
    public void CountDownButton()
    {
        if (dropBoxCount == 1)
        {
            return;
        }
        dropBoxCount--;
        countText.text = dropBoxCount.ToString();
    }
    void CloseDropBoxCountMessage()
    {
        workingItem     = null;
        workingIndex    = -1;
        dropBoxCount    = 1;
        dropBoxCountMessage.gameObject.SetActive(false);
    }
    void OpenDropBoxCountMessage(int _index)
    {
        workingItem = GameManager.gameManager.character.GetDropBoxItem(_index);

        if(workingItem.ItemCount == 1)
        {
            OkButton();            
        }
        else
        {   
            dropBoxCountMessage.gameObject.SetActive(true);
        }
        
    }
    public void CloseDropBox()
    {
        CloseDropBoxCountMessage();
        gameObject.SetActive(false);
    }
    #endregion

    private void OnDisable()
    {
        character = null;
    }
    void SetDropBox()
    {   
        character = GameManager.gameManager.character;
        UpdateAllSlot();
    }
    
    public void UpdateAllSlot()    
    {
        List<Item> itemList = GameManager.gameManager.character.dropBox;
        if(itemList == null)
        {
            return;
        }

        for (int slotNumer = 0; slotNumer < itemList.Count; slotNumer++)
        {
            Item item = itemList[slotNumer];
            dropBoxSlots[slotNumer].Add(item.itemSpriteName,item.ItemCount);            
        }

        for (int emptyNumber = dropBoxSlots.Count; emptyNumber < dropBoxSlots.Count; emptyNumber++)
        {
            dropBoxSlots[emptyNumber].Clear();
        }
    }
    public void UpdateSlot(int _index)
    {
        Item item = character.GetDropBoxItem(_index);


        if (item == null)
        {
            dropBoxSlots[_index].Clear();
        }
        else
        {
            dropBoxSlots[_index].Add(item.itemSpriteName, item.ItemCount);
        }
    }

   
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(1))
        {
            for (int i = 0; i < dropBoxSlots.Count; i++)
            {
                if (dropBoxSlots[i].isInRect(eventData.position) && character.GetDropBoxItem(i) != null)
                {
                    workingIndex = i;
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(workingIndex == -1)
        {
            return;
        }
        else
        {
            if (Input.GetMouseButtonUp(1))
            {
                if (dropBoxSlots[workingIndex].isInRect(eventData.position))
                {
                    OpenDropBoxCountMessage(workingIndex);
                    return;
                }

            }
            else
            {
                workingIndex = -1;
            }
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
