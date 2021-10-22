using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    Dictionary<int, Item> Inven = new Dictionary<int, Item>();
    Item item;
    int Capacity = 18;
    public int CAPACITY
    {        
        get
        {
            return Capacity;
        }
        set
        {
            if (value < 18)
                Capacity = 18;
            else
                Capacity = value;
        }
    }
    public List<int> GetKeys()
    {
        List<int> Keys = new List<int>(Inven.Keys);
        return Keys;
    }
    
    public Item GetItem(int _SlotNum)
    {
        
     
        if(Inven.TryGetValue(_SlotNum,out item))
        {
            return item;
        }
        else
        {
            return null;
        }
    }   
    public Item PopItem(int _SlotNum)
    {
        if (Inven.TryGetValue(_SlotNum, out item))
        {
            Inven.Remove(_SlotNum);
            return item;
        }
        else
        {
            return null;
        }
    }
    public bool PushItem(Item _NewItem)
    {
        for (int i = 0; i < CAPACITY; i++)
        {
            if (!Inven.TryGetValue(i, out item))
            {
                Inven.Add(i, _NewItem);
                return true;
            }
        }
        return false;
    }
    public Item AddItem(int _Index, Item _NewItem)
    {
        if(Inven.TryGetValue(_Index,out item))
        {
            if(Inven[_Index].Index == _NewItem.Index)
            {
                Inven[_Index].ItemCount += _NewItem.ItemCount;
                return null;
            }
            Inven.Remove(_Index);
            Inven.Add(_Index, _NewItem);
            return item;
        }
        else
        {
            Inven.Add(_Index, _NewItem);
            return null;
        }        
    }   
    public bool IsEmpty(int _Num)
    {
        return !(Inven.TryGetValue(_Num, out item));
    }     
}
