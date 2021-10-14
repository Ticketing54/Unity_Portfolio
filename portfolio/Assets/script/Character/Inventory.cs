using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    Item[] Inven;
    public delegate Item MoveItemCheck(Item _Item, int _Last);    
    public Inventory()
    {
        Inven = new Item[18];        
    }
    public Item GetItem(int _Num)
    {
        if (Inven[_Num] == null || Inven[_Num].itemType == Item.ItemType.None)
            return null;
        else
            return Inven[_Num];
    }   
    public void Add(int _Index, Item _NewItem)
    {
        Inven[_Index] = _NewItem;
    }
    public void Subtract(int _index)
    {
        Inven[_index].itemType = 0;
    }
    bool IsEmpty(int _Num)
    {
        if (Inven[_Num] == null || Inven[_Num].itemType == Item.ItemType.None)
            return true;
        else
            return false;
    }
    bool IsSame(int _First,int _Last)
    {
        return Inven[_First].Index == Inven[_Last].Index;
    }
    public void StartItemMove(int _Start,int _LastType,int _Last,MoveItemCheck Check)  // 시작
    {
        if (_LastType == 1 && Inven[_Start].itemType != Item.ItemType.Used)
            return;
        else if (_LastType == 2 && Inven[_Start].itemType != Item.ItemType.Equipment)
            return;
        Inven[_Start] = Check(Inven[_Start], _Last);
    }
    public Item AriveItem(Item _Item, int _Last)      //끝
    {
        if (IsEmpty(_Last))
        {
            Inven[_Last] = _Item;
            return null;
        }
        else if ((_Item.itemType != Item.ItemType.Equipment) &&(_Item.Index == Inven[_Last].Index))
        {
            Inven[_Last].ItemCount += _Item.Index;
            return null;
        }
        else
        {
            Item temp = Inven[_Last];
            Inven[_Last] = _Item;
            return temp;
        }
    }
}
