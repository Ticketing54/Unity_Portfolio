using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    Item[] Inven;
    enum ComPare
    {
        Same,
        Different,
    }

    // 비어 있을때
    // 아이템이 있을때
        // 같을때
        // 다를때
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
    public void Swap(int _First, int _Last)
    {
        if (IsEmpty(_Last))
        {
            Inven[_Last] = Inven[_First];
            Inven[_First] = null;
        }
        else
        {
            if (IsSame(_First, _Last))
            {
                Inven[_Last].ItemCount += Inven[_First].ItemCount;
                Inven[_First] = null;
            }
            else
            {
                Item tmp = Inven[_First];
                Inven[_First] = Inven[_Last];
                Inven[_Last] = tmp;
            }
        }
        
    }
}
