using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class Character_Quick : ItemMove
{
    Character character;

    [SerializeField]
    Dictionary<int, List<Item>> itemSlots;
    [SerializeField]
    Dictionary<int, List<Skill>> skillSlots;
    
    


    HashSet<int> runningUseItem;
    HashSet<int> runningUseSkill;


    int itemSlotNum = 0;
    int skillSlotNum = 0;

   
    public Character_Quick(Character _character)
    {
        character           = _character;
        runningUseItem      = new HashSet<int>();
        runningUseSkill     = new HashSet<int>();

        UIManager.uimanager.AddKeyBoardSortCut(KeyCode.Alpha1, ItemSlot_First);
        UIManager.uimanager.AddKeyBoardSortCut(KeyCode.Alpha2, ItemSlot_Second);
        UIManager.uimanager.AddKeyBoardSortCut(KeyCode.Alpha3, ItemSlot_Third);
        UIManager.uimanager.AddKeyBoardSortCut(KeyCode.Alpha4, ItemSlot_Fourth);

        itemSlots = new Dictionary<int, List<Item>>()
        {
            {0, new List<Item>()
                {
                    null,null,null,null
                }
            },
            {1, new List<Item>()
                {
                    null,null,null,null
                }
            },
            {2, new List<Item>()
                {
                    null,null,null,null
                }
            },
            {3, new List<Item>()
                {
                    null,null,null,null
                }
            }
        };

        skillSlots = new Dictionary<int, List<Skill>>()
        {
            {0,new List<Skill>()
                {
                    null,null,null,null
                }
            },
            {1,new List<Skill>()
                {
                    null,null,null,null
                }
            },
            {2,new List<Skill>()
                {
                    null,null,null,null
                }
            },
            {3,new List<Skill>()
                {
                    null,null,null,null
                }
            },
        };

        
    }

    #region KeyboardShorcut
    public void ItemSlot_First()
    {
        UseItem(0);
    }

    public void ItemSlot_Second()
    {
        UseItem(1);
    }

    public void ItemSlot_Third()
    {
        UseItem(2);
    }

    public void ItemSlot_Fourth()
    {
        UseItem(3);
    }

    public void SkillSlot_Q()
    {
        UseSkill(0);
    }

    public void SkillSlot_W()
    {
        UseSkill(1);
    }

    public void SkillSlot_E()
    {
        UseSkill(2);
    }

    public void SkillSlot_R()
    {   
        UseSkill(3);
    }
    #endregion

    public int ItemSlotNum
    {
        get => skillSlotNum;        
        set
        {
            if (value >= 4)
                itemSlotNum = 0;
            else
                itemSlotNum = value;
        }
    }
    public int SkillSlotNum
    {
        get => skillSlotNum;
        set
        {
            if(value >= 4)
            {
                skillSlotNum = 0;
            }
            else
            {
                skillSlotNum = value;
            }
        }
    }

    public void UseSkill(int _slotNum)
    {
        Skill skill = GetSkill(_slotNum);

        if(skill == null)
        {
            return;
        }
        else
        {

            //character.skill.UseSkill(skill.index);
            //if (runningUseSkill.Contains(skill.index))
            //{
            //    return;
            //}
            //else
            //{
            //    runningUseSkill.Add(skill.index);
            //    character.StartCoroutine(CoCoolTimecheck_Skill(skill.index, skill.coolTime));
            //    character.skill.UseSkill(skill.index);
            //}
        }
    }
    public void UseItem(int _index)
    {
        Item item = GetItem(_index);
        if(item == null)
        {
            return;
        }

        string[] propertys = item.itemProperty.Split('#');

        for (int i = 0; i < propertys.Length; i++)
        {
            string[] itemproperty = propertys[i].Split('/');
            string usedItemType = itemproperty[0];                          // 소모아이템 타입
            float totalAmount   = float.Parse(itemproperty[1]);             // 총량
            float duration      = float.Parse(itemproperty[2]);             // 지속시간


            switch (usedItemType)
            {
                case "Hp":
                    {
                        if (runningUseItem.Contains(item.index))
                        {
                            return;
                        }
                        else
                        {
                            runningUseItem.Add(item.index);
                            character.StartCoroutine(CoCoolTimecheck_item(item.index,3f));
                            character.stat.RecoveryHp_Potion(totalAmount, duration);
                            break;
                        }
                    }
                case "Mp":
                    {
                        if (runningUseItem.Contains(item.index))
                        {
                            return;
                        }
                        else
                        {
                            runningUseItem.Add(item.index);
                            character.StartCoroutine(CoCoolTimecheck_item(item.index,3f));
                            character.stat.RecoveryMp_Potion(totalAmount, duration);
                            break;
                        }
                    }
                default:
                    return;
            }
        }

        item.ItemCount--;

        if (item.ItemCount == 0)
        {
            itemSlots[ItemSlotNum][_index] = null;
        }

        UIManager.uimanager.ItemUpdateSlot(ITEMLISTTYPE.QUICK, _index);        
    }
    IEnumerator CoCoolTimecheck_item(int _itemInex ,float _coolTime)
    {
        float timer = 0.0001f;        
        while(timer <= _coolTime)
        {
            timer += Time.deltaTime;
            float percent = timer / _coolTime;
            CoolTimeCheck_Item(_itemInex, percent);
            yield return null;
        }
        runningUseItem.Remove(_itemInex);
    }
    IEnumerator CoCoolTimecheck_Skill(int _skillIndex,float _coolTime)
    {
        float timer = 0.0001f;
        
        while (timer <= _coolTime)
        {
            timer += Time.deltaTime;
            float maxCoolTime = _coolTime;
            maxCoolTime -= timer;
            float percent = timer / _coolTime;
            CoolTimeCheck_SKill(_skillIndex, percent, (int)maxCoolTime);
            yield return null;
        }
        runningUseSkill.Remove(_skillIndex);
    }
    
    void CoolTimeCheck_Item(int _itemIndex,float _percent)
    {
        List<Item> quickslotItems = itemSlots[ItemSlotNum];

        for (int i = 0; i < quickslotItems.Count; i++)
        {
            if(quickslotItems[i] != null  && quickslotItems[i].index == _itemIndex)
            {
                UIManager.uimanager.AQuickSlotItemCooltime(i,_percent);
            }
        }
    }
    void CoolTimeCheck_SKill(int _skillIndex, float _percent,int _coolTime)
    {
        List<Skill> quickslotItems = skillSlots[SkillSlotNum];

        for (int i = 0; i < quickslotItems.Count; i++)
        {
            if (quickslotItems[i] != null && quickslotItems[i].index == _skillIndex)
            {
                UIManager.uimanager.AQuickSlotSkillCooltime(i, _percent,_coolTime);
            }
        }
    }
    public  Item GetItem(int _SlotNum)
    {
        List<Item> quickItem = itemSlots[ItemSlotNum];

        
        return  quickItem[_SlotNum]; 
    }
    public Item PopItem(int _SlotNum)
    {
        List<Item> quickItem = itemSlots[ItemSlotNum];
        Item PopItem = quickItem[_SlotNum];
        quickItem[_SlotNum] = null;
        quickItem = null;
        return PopItem;
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
    
    public void AddItem(int _slotNum, Item _NewItem)
    {
        List<Item> quickItem = itemSlots[ItemSlotNum];
        quickItem[_slotNum] = _NewItem;
        quickItem = null;
        UIManager.uimanager.ItemUpdateSlot(ITEMLISTTYPE.QUICK, _slotNum);

    }
    public bool IsEmpty_Item(int _Num)
    {
        List<Item> quickItem = itemSlots[itemSlotNum];
        Item item = quickItem[_Num];
        quickItem = null;


        return item == null;
    }


    public bool IsEmpty_Skill(int _Num)
    {
        List<Skill> quickSkill = skillSlots[skillSlotNum];
        
        if(quickSkill[_Num] != null)
        {
            return false;
        }
        else
        {
            return true;
        }        
    }  
    public Skill GetSkill(int _SlotNum)
    {
        List<Skill> quickSkill = skillSlots[skillSlotNum];

        if(quickSkill[_SlotNum] == null)
        {
            Debug.Log("스킬이 없습니다.");
            return null;
        }            
        return quickSkill[_SlotNum];
    }
   
    //public void AddSkill(int _SlotNum,int _skillIndex)
    //{
    //    List<Skill> quickSkill = skillSlots[skillSlotNum];
    //    for (int i = 0; i < quickSkill.Count; i++)
    //    {
    //        if(quickSkill[i] != null &&quickSkill[i].index == _skillIndex)
    //        {
    //            quickSkill[i] = null;
    //            UIManager.uimanager.AUpdateSkillSlot(i);
    //        }
    //    }        
    //    quickSkill[_SlotNum] = new Skill(_skillIndex);
    //    UIManager.uimanager.AUpdateSkillSlot(_SlotNum);
    //}   
    //public void RemoveSkill(int _SlotNum)
    //{
    //    List<Skill> quickSkill = skillSlots[skillSlotNum];
    //    quickSkill[_SlotNum] = null;
    //}
   
    

    public List<int> GetSameItemIndexList(int _itemIndex)
    {
        List<int> sameItemList = new List<int>();
        List<Item> quickItemList = itemSlots[ItemSlotNum];

        for (int i = 0; i < quickItemList.Count; i++)
        {
            if(quickItemList[i]!= null && quickItemList[i].index == _itemIndex)
            {
                sameItemList.Add(i);
            }
        }

        if(sameItemList.Count == 0)
        {
            return null;
        }
        else
        {
            return sameItemList;
        }        
    }
    public int GetSkillSlotIndex(int _skillIndex)
    {
        List<int> sameItemList = new List<int>();
        List<Skill> quickSkillSlots = skillSlots[skillSlotNum];        
        for (int i = 0; i < quickSkillSlots.Count; i++)
        {
            if(quickSkillSlots[i] != null && quickSkillSlots[i].index == _skillIndex)
            {
                return i;
            }
        }
        return -1;
    }
    //public void MoveQuickSkill(int _startIndex,int _endIndex)
    //{
    //    if (_endIndex == -1)
    //    {
    //        skillSlots[skillSlotNum][_startIndex] = null;
    //        UIManager.uimanager.AUpdateSkillSlot(_startIndex);
    //        return;
    //    }
    //    Skill skill = skillSlots[skillSlotNum][_startIndex];
    //    skillSlots[skillSlotNum][_startIndex] = skillSlots[skillSlotNum][_endIndex];
    //    skillSlots[skillSlotNum][_endIndex] = skill;

    //    UIManager.uimanager.AUpdateSkillSlot(_startIndex);
    //    UIManager.uimanager.AUpdateSkillSlot(_endIndex);        
    //}
}
