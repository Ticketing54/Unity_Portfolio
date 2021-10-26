using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Equipment
{    
    [SerializeField]
    Status Stat= null;
    Dictionary<EquipMentType, Item> Equip = new Dictionary<EquipMentType, Item>();
    Item item;
    Item ITEM { get { return item; } }
    EquipMentType Type = EquipMentType.NONE;
    public Equipment(Status _Stat)
    {        
        Stat = _Stat;
    }
    public Item GetItem(int _equipType)
    {
        Type = (EquipMentType)_equipType;
        if(Equip.TryGetValue(Type,out item))
        {
            Type = EquipMentType.NONE;
            return ITEM;
        }
        else
        {
            Type = EquipMentType.NONE;
            return null;
        }
    }
    public List<EquipMentType> GetKeys()
    {
        List<EquipMentType> Keys = new List<EquipMentType>(Equip.Keys);
        return Keys;
    }
    public Item PopEquip(int _equipType)
    {
        Type = (EquipMentType)_equipType;
        if (Equip.TryGetValue(Type,out item))
        {
            Stat.TakeOffStatus(item);
            Equip.Remove(Type);
            Type = EquipMentType.NONE;
            return ITEM;
        }
        else
        {
            Type = EquipMentType.NONE;
            return null;
        }
        
    }
    public Item PushEquip(int _equipType, Item _NewItem)
    {
        Type = (EquipMentType)_equipType;

        if (_NewItem.EquipType != Type|| _NewItem.itemType != ItemType.EQUIPMENT)             // 같은 타입이 아닐 경우 // 장비 아이템이 아닐 경우
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
    
    public bool IsEmpty(EquipMentType _equipType)
    {
        return !(Equip.TryGetValue(_equipType, out item));
    }
    
}
