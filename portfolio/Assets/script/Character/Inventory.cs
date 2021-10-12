using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    Item[] Inven;
    public Inventory()
    {
        Inven = new Item[18];
    }
    public Item GetItem(int _Num) { return Inven[_Num]; }
    public void Add(int _Index, Item _NewItem)
    {
        Inven[_Index] = _NewItem;
    }
    public void Subtract(int _index)
    {
        Inven[_index].itemType = 0;
    }
    public void Swap(int _First, int _Last)
    {
        Item tmp = Inven[_First];
        Inven[_First] = Inven[_Last];
        Inven[_Last] = tmp;
    }
}
