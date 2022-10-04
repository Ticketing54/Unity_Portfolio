using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Inventory : ItemMove
{
    [SerializeField]
    Item[] inven;

    int maxCount;
    int currentCount;
    bool inventoryActive = false;
    Character character;
    public Inventory(Character _character)
    {
        character = _character;
        inven =new Item[18];
        maxCount = 18;
        currentCount = 0;
        _character.AddKeyBoardSortCut(KeyCode.I, TryOpenInventory);
    }
    int gold;
    public int Gold { get => gold; set => gold = value; }

    void TryOpenInventory()
    {
        inventoryActive = !inventoryActive;
        if (inventoryActive)
        {
            OpenInventory();
        }
        else
        {
            CloseInventory();
        }
    }
    void OpenInventory()
    {
        if (inventoryActive == false)
        {
            inventoryActive = true;
        }
        UIManager.uimanager.AOpenInventoryUi();
    }
    void CloseInventory()
    {
        if (inventoryActive == true)
        {
            inventoryActive = false;
        }
        UIManager.uimanager.ACloseInventoryUi();
    }




    public void AddGold(int _gold)
    {
        gold += _gold;
        UIManager.uimanager.AGetGoldUpdateUi(_gold);
    }
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
        currentCount--;
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
            if(inven[slotNum]!= null&& inven[slotNum].index == _NewItem.index && _NewItem.itemType != ITEMTYPE.EQUIPMENT)
            {
                inven[slotNum].ItemCount += _NewItem.ItemCount;                
                UIManager.uimanager.ItemUpdateSlot(ITEMLISTTYPE.INVEN, slotNum);
                return true;
            }
        }
        for (int slotNum = 0; slotNum < inven.Length; slotNum++)
        {
            if (inven[slotNum] == null)
            {
                inven[slotNum] = _NewItem;
                currentCount++;
                UIManager.uimanager.ItemUpdateSlot(ITEMLISTTYPE.INVEN, slotNum);

                return true;
            }
        }
        return false;
    }   
    
    public void AddItem(int _Index, Item _NewItem)
    {
        currentCount++;
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
 
    public bool IsSlotEmpty(int _Num)
    {
        return inven[_Num] == null;
    }
    public bool IsInvenFull()
    {
        return currentCount >= maxCount;
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
