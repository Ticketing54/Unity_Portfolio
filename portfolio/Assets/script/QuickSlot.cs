﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlot 
{
    Dictionary<int, Item[]> ItemSlot;
    Dictionary<int, Skill[]> SkillSlot;
    Item[] quickItem;
    Skill[] quickSkill;
    int ItemSlotNum = 0;
    int SkillSlotNum = 0;
    
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
    public Item StartItemMove(int _Start)  // 시작
    {
        quickItem = ItemSlot[ItemSlotNum];
        if (quickItem[_Start] == null)
            return null;
        else
        {
            Item temp = quickItem[_Start];
            quickItem[_Start] = null;
            return temp;
        }
    }       
    bool IsEmpty_Item(int _Num)
    {
        quickItem = ItemSlot[ItemSlotNum];
        return (quickItem[_Num] == null || quickItem[_Num].itemType == Item.ItemType.None);      
    }
    bool IsEmpty_Skill(int _Num)
    {
        quickSkill = SkillSlot[SkillSlotNum];
        return (quickSkill[_Num] == null || quickSkill[_Num].skillType == Skill.SkillType.None);
    }
    public Item GetItem(int _Num)
    {
        quickItem = ItemSlot[ItemSlotNum];
        if (IsEmpty_Item(_Num))
        {
            Debug.Log("아이템이 없습니다.");
            return null;
        }            
        return quickItem[_Num];
    }
    public Skill GetSkill(int _SlotNum)
    {
        quickSkill= SkillSlot[SkillSlotNum];
        if (IsEmpty_Skill(_SlotNum))
        {
            Debug.Log("스킬이 없습니다.");
            return null;
        }            
        return quickSkill[_SlotNum];
    }
    public void AddItem(int _SlotNum,Item _Item)
    {
        quickItem = ItemSlot[ItemSlotNum];
        quickItem[_SlotNum] = _Item;        
    }   
    public void AddSkill(int _ListNum, int _SlotNum, Skill _Skill)
    {
        quickSkill = SkillSlot[SkillSlotNum];
        quickSkill[_SlotNum] = _Skill;
    }
    public void RemoveItem(int _SlotNum)
    {
        quickItem = ItemSlot[ItemSlotNum];
        if (quickItem[_SlotNum] == null)
            return;
        else
            quickItem[_SlotNum].itemType = Item.ItemType.None;        
    }
    public void RemoveSkill(int _SlotNum)
    {
        quickSkill = SkillSlot[SkillSlotNum];
        if (quickSkill[_SlotNum] == null)
            return;
        else quickItem[_SlotNum].itemType = Item.ItemType.None;
    }
}
