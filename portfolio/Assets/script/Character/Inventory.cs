using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Inventory : ItemMove
{    
    Item[] inven = new Item[18];    
    
    
    public  Item GetItem(int _SlotNum)
    {
        return  inven[_SlotNum];
    }   
    public Item PopItem(int _SlotNum)
    {
        Item PopItem = GetItem(_SlotNum);
        inven[_SlotNum] = null;
        return PopItem;
    }

    
    public bool PushItem(Item _NewItem)
    {
        for(int slotNum = 0; slotNum < inven.Length; slotNum++)
        {
            if (inven[slotNum] == null)
            {
                inven[slotNum] = _NewItem;                

                if(UIManager.uimanager.InventoryActive == true)
                {   
                    UIManager.uimanager.updateInven();
                }

                return true;
            }
        }
        return false;
    }   
    
    public void AddItem(int _Index, Item _NewItem)
    {
        inven[_Index] = _NewItem;
    }   
    public int Empty_SlotNum()
    {
        for(int index = 0; index < inven.Length; index++)
        {
            if(inven[index] == null)
            {                
                return index;
            }
        }
        return -1;
    }
    public Item Exchange(int _Index, Item _NewItem)
    {
        Item OldItem = inven[_Index];

        if(OldItem != null && OldItem.Index == _NewItem.Index )
        {
            OldItem.ItemCount += _NewItem.ItemCount;
            return null;
        }
        inven[_Index] = _NewItem;
        return OldItem;
    }
    public bool IsEmpty(int _Num)
    {
        return inven[_Num] == null;
    }

    public bool PossableMoveItem(int _index, Item _MoveItem)
    {
        return true;
    }
    public string InvenInfo()
    {
        string Data = string.Empty;
        for (int i = 0; i < inven.Length; i++)
        {
           
            if (inven[i] != null)
            {
                if (Data != string.Empty)
                {
                    Data += "/";
                }
                Data += inven[i].Index + "," + i + "," + inven[i].ItemCount ;
            }
        }
        return Data;
        
    }
}
