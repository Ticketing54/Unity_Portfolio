using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuickSlot :ItemMove
{
    [SerializeField]
    Dictionary<int, Item[]> ItemSlot = new Dictionary<int,Item[]>();
    [SerializeField]
    Dictionary<int, Skill[]> SkillSlot = new Dictionary<int, Skill[]>();
    Item[] quickItem;
    Skill[] quickSkill;

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
    void CreateSlots<T>(Dictionary<int,T[]> _Slot)
    {
        for (int i = 0; i < 4; i++)
        {
            _Slot.Add(i, new T[4]);
        }        
    }

    public  Item GetItem(int _SlotNum)
    {
        quickItem = ItemSlot[ITEMSLOTNUM];

        
        return  quickItem[_SlotNum]; 
    }
    public Item PopItem(int _SlotNum)
    {
        quickItem = ItemSlot[ITEMSLOTNUM];
        Item PopItem = quickItem[_SlotNum];
        quickItem[_SlotNum] = null;
        quickItem = null;
        return PopItem;
    }

    public Item Exchange(int _Index, Item _NewItem)
    {
        Item popItem =PopItem(_Index);

        if(popItem != null && popItem.Index == _NewItem.Index)
        {
            _NewItem.ItemCount += popItem.ItemCount;
            popItem = null;
        }

        AddItem(_Index, _NewItem);

        return popItem;
    }
    public bool PossableMoveItem(int _index, Item _MoveItem)
    {
        if(_MoveItem== null)
        {
            return true;
        }

        if(_MoveItem.itemType != ITEMTYPE.USED)
        {
            return false;
        }

        return true;
    }

    public void AddItem(int _listNum, int _SlotNum, Item _NewItem)
    {
        quickItem = ItemSlot[_listNum];
        quickItem[_SlotNum] = _NewItem;
        quickItem = null;
    }
    public void AddItem(int _SlotNum, Item _NewItem)
    {
        quickItem = ItemSlot[ITEMSLOTNUM];
        quickItem[_SlotNum] = _NewItem;
        quickItem = null;
    }
    public bool IsEmpty_Item(int _Num)
    {
        quickItem = ItemSlot[ItemSlotNum];
        Item item = quickItem[_Num];
        quickItem = null;


        return item == null;
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
        Skill skill= quickSkill[_Num];
        quickSkill = null;


        return skill == null;        
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
        else quickItem[_SlotNum].itemType = ITEMTYPE.NONE;
    }

 
}
