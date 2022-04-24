using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuickSlot :ItemMove
{
    Character character;

    [SerializeField]
    Dictionary<int, List<Item>> itemSlots;
    [SerializeField]
    Dictionary<int, List<int>> skillSlots;
    

    int itemSlotNum     = 0;
    int skillSlotNum    = 0;

    public delegate void UpdateUI(int _num);

    public QuickSlot(Character _character)
    {
        character = _character;
        
        character.keyboardShorcut.Add(KeyCode.Q, SkillSlot_Q);
        character.keyboardShorcut.Add(KeyCode.W, SkillSlot_W);
        character.keyboardShorcut.Add(KeyCode.E, SkillSlot_E);
        character.keyboardShorcut.Add(KeyCode.R, SkillSlot_R);
        character.keyboardShorcut.Add(KeyCode.Alpha1, ItemSlot_First);
        character.keyboardShorcut.Add(KeyCode.Alpha2, ItemSlot_Second);
        character.keyboardShorcut.Add(KeyCode.Alpha3, ItemSlot_Third);
        character.keyboardShorcut.Add(KeyCode.Alpha4, ItemSlot_Fourth);

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

        skillSlots = new Dictionary<int, List<int>>()
        {
            {0,new List<int>()
                {
                    -1,-1,-1,-1
                }
            },
            {1,new List<int>()
                {
                    -1,-1,-1,-1
                }
            },
            {2,new List<int>()
                {
                    -1,-1,-1,-1
                }
            },
            {3,new List<int>()
                {
                    -1,-1,-1,-1
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

    public int ITEMSLOTNUM
    {
        get
        {
            return itemSlotNum;
        }
        set
        {
            if (value >= 4)
                itemSlotNum = 0;
            else
                itemSlotNum = value;
        }
    }


    public void UseSkill(int _index)
    {
        int skillIndex = GetSkill(_index);
        if(skillIndex == -1)
        {
            return;
        }
        else
        {
            character.skill.UseSkill(skillIndex);
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
            switch (itemproperty[0])
            {
                case "Hp":
                    {

                        if (character.stat.UsingPotion_Hp)
                        {
                            return;
                        }
                        else
                        {
                            character.stat.RecoveryHp(float.Parse(itemproperty[1]), float.Parse(itemproperty[2]));
                            break;
                        }
                    }
                case "Mp":
                    {
                        if (character.stat.UsingPotion_Mp)
                        {
                            return;
                        }
                        else
                        {
                            character.stat.RecoveryMp(float.Parse(itemproperty[1]), float.Parse(itemproperty[2]));
                            break;
                        }
                    }
                default:
                    return;
            }
        }
    }

    public  Item GetItem(int _SlotNum)
    {
        List<Item> quickItem = itemSlots[ITEMSLOTNUM];

        
        return  quickItem[_SlotNum]; 
    }
    public Item PopItem(int _SlotNum)
    {
        List<Item> quickItem = itemSlots[ITEMSLOTNUM];
        Item PopItem = quickItem[_SlotNum];
        quickItem[_SlotNum] = null;
        quickItem = null;
        return PopItem;
    }

    public Item Exchange(int _Index, Item _NewItem)
    {
        Item popItem =PopItem(_Index);

        if(popItem != null && popItem.index == _NewItem.index)
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
    
    public void AddItem(int _slotNum, Item _NewItem)
    {
        List<Item> quickItem = itemSlots[ITEMSLOTNUM];
        quickItem[_slotNum] = _NewItem;
        quickItem = null;
        UIManager.uimanager.updateUiSlot(ITEMLISTTYPE.QUICK, _slotNum);

    }
    public bool IsEmpty_Item(int _Num)
    {
        List<Item> quickItem = itemSlots[itemSlotNum];
        Item item = quickItem[_Num];
        quickItem = null;


        return item == null;
    }



    public void AllItemUpdateUi(UpdateUI _Update, UpdateUI _EmptySlot)
    {
        List<Item> quickItem = itemSlots[itemSlotNum];

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
        List<Item> quickItem = itemSlots[itemSlotNum];
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
        List<int> quickSkill = skillSlots[skillSlotNum];
        
        if(quickSkill[_Num] != -1)
        {
            return false;
        }
        else
        {
            return true;
        }        
    }  
    public int GetSkill(int _SlotNum)
    {
        List<int> quickSkill = skillSlots[skillSlotNum];
        if (IsEmpty_Skill(_SlotNum))
        {
            Debug.Log("스킬이 없습니다.");
            return -1;
        }            
        return quickSkill[_SlotNum];
    }
   
    public void AddSkill(int _ListNum, int _SlotNum,int _skillIndex)
    {
        List<int> quickSkill = skillSlots[skillSlotNum];
        quickSkill[_SlotNum] = _skillIndex;
    }   
    public void RemoveSkill(int _SlotNum)
    {
        List<int> quickSkill = skillSlots[skillSlotNum];
        if (quickSkill[_SlotNum] == -1)
        {
            return;
        }
        else
        {
            quickSkill[_SlotNum] = -1;
        }            
        
    }
   

}
