using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory : ItemMove
{    
    Item[] Inven = new Item[18];    
    int itemCount = 0;    
    
    public Item GetItem(int _SlotNum)
    {
        return Inven[_SlotNum];
    }   
    public Item PopItem(int _SlotNum)
    {
        Item PopItem = GetItem(_SlotNum);
        Inven[_SlotNum] = null;
        itemCount--;

        return PopItem;
    }
    public bool PushItem(Item _NewItem)
    {
        for(int slotNum = 0; slotNum < Inven.Length; slotNum++)
        {
            if (Inven[slotNum] == null)
            {
                Inven[slotNum] = _NewItem;
                ++itemCount;
                return true;
            }
        }
        return false;
    }   
    public void AddItem(int _Index, Item _NewItem)
    {
        Inven[_Index] = _NewItem;
    }   

    public Item ExchangeItem(int _Index, Item _NewItem)
    {
        Item OldItem = Inven[_Index];

        if(OldItem.Index == _NewItem.Index)
        {
            OldItem.ItemCount += _NewItem.ItemCount;
            return null;
        }
        Inven[_Index] = _NewItem;
        return OldItem;
    }
    public bool IsEmpty(int _Num)
    {
        return Inven[_Num] == null;
    }     
}
