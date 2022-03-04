using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Equipment : ItemMove
{    
    [SerializeField]
    Status Stat= null;    

    Item[] Equip = new Item[5];
    Item item;

    Item ITEM { get { return item; } }
    
    public Equipment(Status _Stat)
    {        
        Stat = _Stat;
    }
    public ref Item GetItem(int _equipType)
    {
        return ref Equip[_equipType];
    }

    public Item PopItem(int _Index)
    {
        Item popItem = GetItem(_Index);
        Equip[_Index] = null;
        Stat.TakeOffStatus(popItem);
        return popItem;
    }
    public void AddItem(Item _NewItem)
    {
        if (_NewItem.itemType != ITEMTYPE.EQUIPMENT) 
        {
            Debug.LogError("잘못된 아이템을 장비로 장착하려합니다");
            return;
        }

        Equip[(int)_NewItem.itemType] = _NewItem;
        Stat.EquipStatus(_NewItem);        
    }

    public Item Exchange(int _index, Item _NewItem)
    {
        Item popItem = PopItem(_index);
        AddItem(_NewItem);
        return popItem;        
    }

    public bool PossableMoveItem(int _index, Item _MoveItem)
    {
        if (_MoveItem.itemType != ITEMTYPE.EQUIPMENT)
        {
            return false;
        }            
        else if(_index != (int)_MoveItem.itemType)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

  
}
