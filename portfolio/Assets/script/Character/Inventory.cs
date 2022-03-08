using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Inventory : ItemMove
{    
    Item[] Inven = new Item[18];    
    
    
    public  Item GetItem(int _SlotNum)
    {
        return  Inven[_SlotNum];
    }   
    public Item PopItem(int _SlotNum)
    {
        Item PopItem = GetItem(_SlotNum);
        Inven[_SlotNum] = null;
        return PopItem;
    }

    
    public bool PushItem(Item _NewItem)
    {
        for(int slotNum = 0; slotNum < Inven.Length; slotNum++)
        {
            if (Inven[slotNum] == null)
            {
                Inven[slotNum] = _NewItem;                

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
        Inven[_Index] = _NewItem;
    }   
    public int Empty_SlotNum()
    {
        for(int index = 0; index < Inven.Length; index++)
        {
            if(Inven[index] == null)
            {                
                return index;
            }
        }
        return -1;
    }
    public Item Exchange(int _Index, Item _NewItem)
    {
        Item OldItem = Inven[_Index];

        if(OldItem != null && OldItem.Index == _NewItem.Index )
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

    public bool PossableMoveItem(int _index, Item _MoveItem)
    {
        return true;
    }

}
