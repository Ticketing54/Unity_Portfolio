using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Equipment
{    
    [SerializeField]
    Status Stat= null;
    Dictionary<EQUIPTYPE, Item> Equip = new Dictionary<EQUIPTYPE, Item>();
    Item item;
    Item ITEM { get { return item; } }
    EQUIPTYPE Type = EQUIPTYPE.NONE;
    public Equipment(Status _Stat)
    {        
        Stat = _Stat;
    }
    public Item GetItem(int _equipType)
    {
        Type = (EQUIPTYPE)_equipType;
        if(Equip.TryGetValue(Type,out item))
        {
            Type = EQUIPTYPE.NONE;
            return ITEM;
        }
        else
        {
            Type = EQUIPTYPE.NONE;
            return null;
        }
    }
    public List<EQUIPTYPE> GetKeys()
    {
        List<EQUIPTYPE> Keys = new List<EQUIPTYPE>(Equip.Keys);
        return Keys;
    }
    public Item PopEquip(int _equipType)
    {
        Type = (EQUIPTYPE)_equipType;
        if (Equip.TryGetValue(Type,out item))
        {
            Stat.TakeOffStatus(item);
            Equip.Remove(Type);
            Type = EQUIPTYPE.NONE;
            return ITEM;
        }
        else
        {
            Type = EQUIPTYPE.NONE;
            return null;
        }
        
    }
    public Item PushEquip(int _equipType, Item _NewItem)
    {
        Type = (EQUIPTYPE)_equipType;

        if (_NewItem.EquipType != Type|| _NewItem.itemType != ITEMTYPE.EQUIPMENT)             // 같은 타입이 아닐 경우 // 장비 아이템이 아닐 경우
            return _NewItem;

        if (Equip.TryGetValue(Type,out item))
        {
            Stat.TakeOffStatus(item);
            Equip.Remove(_NewItem.EquipType);
            Stat.EquipStatus(_NewItem);
            Equip.Add(_NewItem.EquipType, _NewItem);
            return ITEM;
        }
        else
        {
            Stat.EquipStatus(_NewItem);
            Equip.Add(_NewItem.EquipType, _NewItem);
            return null;
        }
    }
    
    public bool IsEmpty(EQUIPTYPE _equipType)
    {
        return !(Equip.TryGetValue(_equipType, out item));
    }
    
}
