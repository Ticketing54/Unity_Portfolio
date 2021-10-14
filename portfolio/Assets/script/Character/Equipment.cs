using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment
{
    Item[] EquipItem;
    Status Stat= null;
    public delegate Item MoveItemCheck(Item _Item, int _Last);
    public Equipment(int _Size, Status _Stat)
    {
        EquipItem = new Item[_Size];
        Stat = _Stat;
    }
    public Item GetItem(int _Num)
    {
        if (EquipItem[_Num] == null || EquipItem[_Num].itemType == Item.ItemType.None)
            return null;
        else
            return EquipItem[_Num];
    }
    public void Add(int _index, Item _NewItem)
    {
        EquipItem[_index] = _NewItem;
        Stat.EquipStatus(_NewItem);
    }
    public Item Subtract(int _index)
    {
        Item temp = EquipItem[_index];
        Stat.TakeOffStatus(EquipItem[_index]);
        EquipItem[_index] = null;
        return temp;
    }
    public void StartItemMove(int _Start, int _LastType, int _Last, MoveItemCheck Check)  // 시작
    {
        if(_LastType == 0)
            EquipItem[_Start] = Check(EquipItem[_Start], _Last);
    }

    bool IsEmpty(int _Index)
    {
        if (EquipItem[_Index] == null && EquipItem[_Index].itemType == Item.ItemType.None)
            return true;

        return false;
    }
    public Item AriveItem(Item _Item, int _Last)      //끝
    {
        if (IsEmpty(_Last))
        {
            Add(_Last, _Item);
            return null;
        }        
        else
        {
            Item temp = Subtract(_Last);
            Add(_Last, _Item);
            return temp;
        }
    }
}
