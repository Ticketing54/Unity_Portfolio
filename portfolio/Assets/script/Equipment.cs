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
    public void Add(int _index, Item _NewItem)
    {
        EquipItem[_index] = _NewItem;
        Stat.EquipStatus(_NewItem);
    }
    public void Subtract(int _index)
    {
        EquipItem[_index].itemType = 0;
        Stat.TakeOffStatus(EquipItem[_index]);
    }
}
