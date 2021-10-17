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
    public Item GetItem(int _Num)
    {
        if (Inven[_Num] == null || Inven[_Num].itemType == Item.ItemType.None)
            return null;
        else
            return Inven[_Num];
    }   
    public void AddItem(int _Index, Item _NewItem)
    {
        Inven[_Index] = _NewItem;
    }
    public void Subtract(int _index)
    {
        Inven[_index].itemType = 0;
    }
    public bool IsEmpty(int _Num)
    {
        if (Inven[_Num] == null || Inven[_Num].itemType == Item.ItemType.None)
            return true;
        else
            return false;
    } 
    public Item StartItemMove(int _Start)  // 시작
    {
        if (Inven[_Start] == null)
            return null;
        else
        {
            Item temp = Inven[_Start];
            Inven[_Start] = null;
            return temp;
        }
            
        
    }
}
