using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Equipment
{    
    [SerializeField]
    Status Stat= null;
    Dictionary<Item.EquipMentType, Item> Equip = new Dictionary<Item.EquipMentType, Item>();
    Item item;
    Item ITEM { get { return item; } }
    Item.EquipMentType Type = Item.EquipMentType.None;
    public Equipment(Status _Stat)
    {        
        Stat = _Stat;
    }
    public Item GetItem(int _equipType)
    {
        Type = (Item.EquipMentType)_equipType;
        if(Equip.TryGetValue(Type,out item))
        {
            Type = Item.EquipMentType.None;
            return ITEM;
        }
        else
        {
            Type = Item.EquipMentType.None;
            return null;
        }
    }
    public List<Item.EquipMentType> GetKeys()
    {
        List<Item.EquipMentType> Keys = new List<Item.EquipMentType>(Equip.Keys);
        return Keys;
    }
    public Item PopEquip(int _equipType)
    {
        Type = (Item.EquipMentType)_equipType;
        if (Equip.TryGetValue(Type,out item))
        {
            Stat.TakeOffStatus(item);
            Equip.Remove(Type);
            Type = Item.EquipMentType.None;
            return ITEM;
        }
        else
        {
            Type = Item.EquipMentType.None;
            return null;
        }
        
    }
    public Item PushEquip(int _equipType, Item _NewItem)
    {
        Type = (Item.EquipMentType)_equipType;

        if (_NewItem.EquipType != Type|| _NewItem.itemType != Item.ItemType.Equipment)             // 같은 타입이 아닐 경우 // 장비 아이템이 아닐 경우
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
    
    public bool IsEmpty(Item.EquipMentType _equipType)
    {
        return !(Equip.TryGetValue(_equipType, out item));
    }
    
}
