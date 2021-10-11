using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlot 
{
    Dictionary<int, Item[]> ItemSlot;
    Dictionary<int, Skill[]> SkillSlot;
    Item[] quickItem;
    Skill[] quickSkill;
    public QuickSlot()
    {
        CreateSlots<Item>(ItemSlot);
        CreateSlots<Skill>(SkillSlot);
    }
    void CreateSlots<T>(Dictionary<int,T[]> _temp)
    {
        _temp = new Dictionary<int, T[]>();
        _temp.Add(0, new T[4]);
        _temp.Add(1, new T[4]);
        _temp.Add(2, new T[4]);
        _temp.Add(3, new T[4]);
        
    }
    bool IsEmpty(Item[] _list, int _SlotNum)
    {
        return (_list[_SlotNum] == null || _list[_SlotNum].itemType == Item.ItemType.None);      
    }
    bool IsEmpty(Skill[] _list, int _SlotNum)
    {
        return (_list[_SlotNum] == null || _list[_SlotNum].skillType == Skill.SkillType.None);
    }
    public Item GetItem(int _ListNum, int _SlotNum)
    {
        quickItem = ItemSlot[_ListNum];
        if (IsEmpty(quickItem, _SlotNum))
        {
            Debug.Log("아이템이 없습니다.");
            return null;
        }
            
        return quickItem[_SlotNum];
    }
    public Skill GetSkill(int _ListNum, int _SlotNum)
    {
        quickSkill= SkillSlot[_ListNum];
        if (IsEmpty(quickSkill, _SlotNum))
        {
            Debug.Log("스킬이 없습니다.");
            return null;
        }
            
        return quickSkill[_SlotNum];
    }
    public void AddItem(int _ListNum,int _SlotNum,Item _Item)
    {
        if (_ListNum < 0 || _ListNum > 2)
        {
            Debug.LogError("리스트 번호가 초과하였습니다.");
            return;
        }
        if (_SlotNum < 0 || _SlotNum > 2)
        {
            Debug.LogError("슬롯 번호가 초과하였습니다.");
            return;
        }
        switch (_ListNum)
        {
            case 0:
                quickItem = ItemSlot[0];
                quickItem[_SlotNum] = _Item;
                break;
            case 1:
                quickItem = ItemSlot[1];
                quickItem[_SlotNum] = _Item;
                break;
            case 2:
                quickItem = ItemSlot[2];
                quickItem[_SlotNum] = _Item;
                break;
        }
    }   
    public void AddSkill(int _ListNum, int _SlotNum, Skill _Skill)
    {
        if (_ListNum < 0 || _ListNum > 2)
        {
            Debug.LogError("리스트 번호가 초과하였습니다.");
            return;
        }
        if (_SlotNum < 0 || _SlotNum > 2)
        {
            Debug.LogError("슬롯 번호가 초과하였습니다.");
            return;
        }
        switch (_ListNum)
        {
            case 0:
                quickSkill = SkillSlot[0];
                quickSkill[_SlotNum] = _Skill;
                break;
            case 1:
                quickSkill = SkillSlot[1];
                quickSkill[_SlotNum] = _Skill;
                break;
            case 2:
                quickSkill = SkillSlot[2];
                quickSkill[_SlotNum] = _Skill;
                break;
        }
    }
    public void RemoveItem(int _ListNum, int _SlotNum)
    {
        quickItem = ItemSlot[_ListNum];
        if (quickItem[_SlotNum] == null)
            return;
        else
            quickItem[_SlotNum].itemType = Item.ItemType.None;        
    }
    public void RemoveSkill(int _ListNum, int _SlotNum)
    {
        quickSkill = SkillSlot[_ListNum];
        if (quickSkill[_SlotNum] == null)
            return;
        else quickItem[_SlotNum].itemType = Item.ItemType.None;
    }
}
