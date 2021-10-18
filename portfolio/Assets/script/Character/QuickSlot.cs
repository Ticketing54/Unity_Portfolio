using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuickSlot 
{
    [SerializeField]
    Dictionary<int, Item[]> ItemSlot;
    [SerializeField]
    Dictionary<int, Skill[]> SkillSlot;
    Item[] quickItem;
    Skill[] quickSkill;
    int ItemSlotNum = 0;
    int SkillSlotNum = 0;
    public delegate void UpdateUI(int _num);
    
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
    public void AllItemUpdateUi(UpdateUI _Update, UpdateUI _EmptySlot)
    {
        quickItem = ItemSlot[ItemSlotNum];

        for (int i = 0; i < quickItem.Length; i++)
        {
            if (IsEmpty_Item(i))
            {
                _EmptySlot(i);
            }
            else
            {
                _Update(i);
            }
        }
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
    public bool IsEmpty_Item(int _Num)
    {
        quickItem = ItemSlot[ItemSlotNum];
        return (quickItem[_Num] == null);      
    }
    public bool IsEmpty_Skill(int _Num)
    {
        quickSkill = SkillSlot[SkillSlotNum];
        return (quickSkill[_Num] == null);
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
