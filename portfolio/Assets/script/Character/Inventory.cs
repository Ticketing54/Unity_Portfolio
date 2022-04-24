using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Inventory : ItemMove
{
    [SerializeField]
    Item[] inven;
    public Inventory()
    {
        inven =new Item[18];
    }

    public int gold { get; set; }
    
    public void BuyItem(int _price,int _itemIndex,int _itemCount =1)
    {
        gold -= _price;
        PushItem(new Item(_itemIndex,_itemCount));
    }
    public void SellItem(int _index, int _sellcount)
    {
        Item sellitem = GetItem(_index);

        gold += sellitem.SellPrice * _sellcount;

        if(sellitem.ItemCount == _sellcount)
        {
            inven[_index] = null;
        }
        else
        {
            inven[_index].ItemCount -= _sellcount;
        }
    }
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
    public void GetRewards(List<List<int>> _rewards)
    {
        if(_rewards == null)
        {
            return;
        }
        for (int i = 0; i < _rewards.Count; i++)
        {
            List<int> reward = _rewards[i];
            Item rewardItem = new Item(reward[0], reward[1]);
            PushItem(rewardItem);
        }
    }
    public bool PushItem(Item _NewItem)
    {
        for(int slotNum = 0; slotNum < inven.Length; slotNum++)
        {
            if (inven[slotNum] == null)
            {
                inven[slotNum] = _NewItem;

                UIManager.uimanager.updateUiSlot(ITEMLISTTYPE.INVEN, slotNum);               

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

        if(OldItem != null && OldItem.index == _NewItem.index )
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
                Data += inven[i].index + "," + i + "," + inven[i].ItemCount ;
            }
        }
        return Data;        
    }
   
}
