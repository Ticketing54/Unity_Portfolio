using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment
{
    Item[] EquipItem;
    Status Stat= null;
    
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
    public void AddItem(int _index, Item _NewItem)
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
    public Item StartItemMove(int _Start)  // 시작
    {
        if (EquipItem[_Start] == null)
            return null;
        else
        {
            return Subtract(_Start);
        }
    }

    public bool IsEmpty(int _Index)
    {
        if (EquipItem[_Index] == null && EquipItem[_Index].itemType == Item.ItemType.None)
            return true;

        return false;
    }    
}
