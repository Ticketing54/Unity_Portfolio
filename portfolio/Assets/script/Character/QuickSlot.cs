using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuickSlot 
{
    [SerializeField]
    Dictionary<int, Dictionary<int, Item>> ItemSlot = new Dictionary<int, Dictionary<int, Item>>();
    [SerializeField]
    Dictionary<int, Dictionary<int, Skill>> SkillSlot = new Dictionary<int, Dictionary<int, Skill>>();
    Dictionary<int, Item> quickItem;
    Dictionary<int, Skill> quickSkill;

    int ItemSlotNum = 0;
    int SkillSlotNum = 0;

    Item item;
    Item ITEM { get { return item; } }
    Skill skill;
    public int ITEMSLOTNUM
    {
        get
        {
            return ItemSlotNum;
        }
        set
        {
            if (value >= 4)
                ItemSlotNum = 0;
            else
                ItemSlotNum = value;
        }
    }
    
    public delegate void UpdateUI(int _num);
    
    public QuickSlot()
    {
        CreateSlots<Item>(ItemSlot);
        CreateSlots<Skill>(SkillSlot);
    }
    void CreateSlots<T>(Dictionary<int,Dictionary<int,T>> _Slot)
    {
        for (int i = 0; i < 4; i++)
        {
            _Slot.Add(i, new Dictionary<int, T>());
        }        
    }

    public Item GetItem(int _SlotNum)
    {
        quickItem = ItemSlot[ITEMSLOTNUM];
        if(quickItem.TryGetValue(_SlotNum, out item))
        {
            return ITEM;
        }
        else
        {
            return null;
        }
    }
    public Item PopItem(int _SlotNum)
    {
        quickItem = ItemSlot[ITEMSLOTNUM];
        if (quickItem.TryGetValue(_SlotNum, out item))
        {
            quickItem.Remove(_SlotNum);
            return ITEM;
        }
        else
        {
            return null;
        }
    }
    public Item AddItem(int _SlotNum, Item _NewItem)
    {
        quickItem = ItemSlot[ITEMSLOTNUM];

        if (_NewItem.itemType != Item.ItemType.Used)        // 소모 아이템이 아닐경우
            return _NewItem;

        if (quickItem.TryGetValue(_SlotNum, out item))
        {
            if(quickItem[_SlotNum].Index == _NewItem.Index)
            {
                quickItem[_SlotNum].ItemCount += _NewItem.ItemCount;
                return null;
            }

            quickItem.Remove(_SlotNum);
            quickItem.Add(_SlotNum, _NewItem);
            return ITEM;
        }
        else
        {
            quickItem.Add(_SlotNum, _NewItem);
            return null;
        }
    }
    public bool IsEmpty_Item(int _Num)
    {
        quickItem = ItemSlot[ItemSlotNum];
        return !(quickItem.TryGetValue(_Num,out item));
    }



    public void AllItemUpdateUi(UpdateUI _Update, UpdateUI _EmptySlot)
    {
        quickItem = ItemSlot[ItemSlotNum];

        for (int i = 0; i < 4; i++)
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
   
    public bool IsEmpty_Skill(int _Num)
    {
        quickSkill = SkillSlot[SkillSlotNum];
        return (quickSkill[_Num] == null);
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
   
    public void AddSkill(int _ListNum, int _SlotNum, Skill _Skill)
    {
        quickSkill = SkillSlot[SkillSlotNum];
        quickSkill[_SlotNum] = _Skill;
    }   
    public void RemoveSkill(int _SlotNum)
    {
        quickSkill = SkillSlot[SkillSlotNum];
        if (quickSkill[_SlotNum] == null)
            return;
        else quickItem[_SlotNum].itemType = Item.ItemType.None;
    }
}
