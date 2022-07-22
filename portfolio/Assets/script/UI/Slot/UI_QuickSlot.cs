using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class UI_QuickSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField]
    QuickSlot[] quickItemSlots;
    [SerializeField]
    SkillQuickSlot[] quickSkillSlots;

    ITEMLISTTYPE itemListType;
    Character character;

    int lefItemIndex   ;
    int leftSkillSlotIndex ;
    int leftSkillIndex ;
    int rightitemIndex ;


    public void OnEnable()
    {
        character = GameManager.gameManager.character;
        UpdateAllSlot();
        UIManager.uimanager.ItemUpdateSlot          += UpdateSlot;
        UIManager.uimanager.ItemClickUp             += IsinRectLeftClick;
        UIManager.uimanager.AQuickSlotItemCooltime  += QuickSlotCooltimeUpdate;
        UIManager.uimanager.AMoveSkillQuick         += IsInRectLeftClick_Skill;
        UIManager.uimanager.AUpdateSkillSlot        += UpdateSkillSlot;
        UIManager.uimanager.AQuickSlotSkillCooltime += QuickSlotCooltimeUpdate_Skill;
    }

    public  void OnDisable()
    {
        UIManager.uimanager.ItemUpdateSlot          -= UpdateSlot;
        UIManager.uimanager.ItemClickUp             -= IsinRectLeftClick;
        UIManager.uimanager.AMoveSkillQuick         -= IsInRectLeftClick_Skill;
        UIManager.uimanager.AUpdateSkillSlot        -= UpdateSkillSlot;
        UIManager.uimanager.AQuickSlotItemCooltime  -= QuickSlotCooltimeUpdate;
        UIManager.uimanager.AQuickSlotSkillCooltime -= QuickSlotCooltimeUpdate_Skill;
    }
    private void Awake()
    {
        itemListType = ITEMLISTTYPE.QUICK;
        leftSkillSlotIndex = -1;
        leftSkillIndex = -1;
        rightitemIndex = -1;
    }


    void QuickSlotCooltimeUpdate(int _slotIndex, float _percent)
    {
        quickItemSlots[_slotIndex].SetCoolTime(_percent);
    }

    void QuickSlotCooltimeUpdate_Skill(int _slotIndex, float _percent, int _count)
    {
        quickSkillSlots[_slotIndex].SetCoolTime(_percent, _count);
    }
    public void UpdateSlot(ITEMLISTTYPE _itemListType, int _index)
    {

        if (_itemListType != itemListType)
        {
            return;
        }

        Item getitem = GameManager.gameManager.character.ItemList_GetItem(itemListType, _index);
        if (getitem == null)
        {
            quickItemSlots[_index].Clear();
            return;
        }
        quickItemSlots[_index].Add(getitem.itemSpriteName, getitem.ItemCount);
    }
    void UpdateSkillSlot(int _index)
    {
        Skill skill = GameManager.gameManager.character.skill.GetQuickSkill(_index);

        if(skill == null)
        {
            quickSkillSlots[_index].Clear();            
        }
        else
        {
            quickSkillSlots[_index].Add(skill.spriteName);
        }
    }


    public void UpdateAllSlot()
    {
        for (int i = 0; i < quickItemSlots.Length; i++)
        {
            Item quickItem = character.quickSlot.GetItem(i);
            if(quickItem == null)
            {
                quickItemSlots[i].Clear();
            }
            else
            {
                quickItemSlots[i].Add(quickItem.itemSpriteName, quickItem.ItemCount);
            }
        }

        for (int j = 0; j < quickSkillSlots.Length; j++)
        {
            UpdateSkillSlot(j);
        }

    }

    protected void LeftClickDown(Vector2 _ClickPos)
    {
        ResetClickInfo();
        for (int itemMoveUiIndex = 0; itemMoveUiIndex < quickItemSlots.Length; itemMoveUiIndex++)
        {
            if (quickItemSlots[itemMoveUiIndex].isInRect(_ClickPos) && !quickItemSlots[itemMoveUiIndex].isEmpty())
            {
                lefItemIndex = itemMoveUiIndex;
                return;
            }
        }

        for (int skillIndex = 0; skillIndex < quickSkillSlots.Length; skillIndex++)
        {
            if(quickSkillSlots[skillIndex].isInRect(_ClickPos) && character.skill.GetQuickSkill(skillIndex) != null)
            {
                leftSkillSlotIndex = skillIndex;
                leftSkillIndex = character.skill.GetQuickSkill(skillIndex).index;
                return;
            }
        }

    }


    protected void LeftClickUp(Vector2 _ClickPos)
    {
        if (lefItemIndex >= 0)
        {
            UIManager.uimanager.ItemClickEnd(itemListType, lefItemIndex, _ClickPos);            
            return;
        }

        if(leftSkillSlotIndex>= 0)
        {
            for (int skillIndex = 0; skillIndex < quickSkillSlots.Length; skillIndex++)
            {
                if (quickSkillSlots[skillIndex].isInRect(_ClickPos))
                {
                    character.skill.MoveQuickSkill(leftSkillSlotIndex, skillIndex);                    
                    return;
                }
            }
            character.skill.MoveQuickSkill(leftSkillSlotIndex, -1);            
        }        
    }
    void ResetClickInfo()
    {
        lefItemIndex = -1;
        leftSkillSlotIndex = -1;
        leftSkillIndex = -1;
    }
    protected void IsinRectLeftClick(ITEMLISTTYPE _type, int _slotNum, Vector2 _pos)
    {
        for (int index = 0; index < quickItemSlots.Length; index++)
        {
            if (quickItemSlots[index].isInRect(_pos))
            {
                character.ItemMove(_type, itemListType, _slotNum, index);
            }
        }
    }

    void IsInRectLeftClick_Skill(int _index, Vector2 _pos)
    {
        for (int slotNum = 0; slotNum < quickSkillSlots.Length; slotNum++)
        {
            if (quickSkillSlots[slotNum].isInRect(_pos))
            {   
                character.skill.AddQuickSkill(slotNum, _index);
            }
        }
    }
    protected void RightClickDown(Vector2 _ClickPos)
    {
        for (int itemMoveUiIndex = 0; itemMoveUiIndex < quickItemSlots.Length; itemMoveUiIndex++)
        {
            if (quickItemSlots[itemMoveUiIndex].isInRect(_ClickPos) && !quickItemSlots[itemMoveUiIndex].isEmpty())
            {
                rightitemIndex = itemMoveUiIndex;
            }
        }
    }


    protected void RightClickUp(Vector2 _ClickPos)
    {
        for (int index = 0; index < quickItemSlots.Length; index++)
        {
            if (quickItemSlots[index].isInRect(_ClickPos) && !quickItemSlots[index].isEmpty() && rightitemIndex == index)
            {
                character.ItemMove_Auto(itemListType, index);
                UpdateSlot(itemListType, index);
            }
        }

        rightitemIndex = -1;
    }





    public void OnPointerDown(PointerEventData clickPoint)
    {
        if (Input.GetMouseButtonDown(0))
        {
            LeftClickDown(clickPoint.position);
            return;
        }
        if (Input.GetMouseButtonDown(1))
        {
            RightClickDown(clickPoint.position);
            return;
        }
    }

    public void OnPointerUp(PointerEventData clickPoint)
    {
        if (Input.GetMouseButtonUp(0))
        {
            LeftClickUp(clickPoint.position);
            return;
        }

        if (Input.GetMouseButtonUp(1))
        {
            RightClickUp(clickPoint.position);
            return;
        }
    }

    public void OnDrag(PointerEventData _dragdata)
    {
        if (0 <= lefItemIndex)
        {
            UIManager.uimanager.MoveItemIcon(itemListType, lefItemIndex, _dragdata.position);
        }
        if (0 <= leftSkillSlotIndex)
        {
            UIManager.uimanager.AMoveSkillIcon(leftSkillIndex, _dragdata.position);
        }
    }
}
