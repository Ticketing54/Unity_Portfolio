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
    SkillSlot[] quickSkillSlots;

    ITEMLISTTYPE itemListType;
    Character character;

    int leftClickIndex = -1;
    int rightClickindex = -1;


    public void OnEnable()
    {
        character = GameManager.gameManager.character;
        UpdateAllSlot();
        UIManager.uimanager.ItemUpdateSlot += UpdateSlot;
        UIManager.uimanager.ItemClickUp += IsinRectLeftClick;
        UIManager.uimanager.AQuickSlotItemCooltime += QuickSlotCooltimeUpdate;
    }

    public  void OnDisable()
    {
        UIManager.uimanager.ItemUpdateSlot -= UpdateSlot;
        UIManager.uimanager.ItemClickUp -= IsinRectLeftClick;
        UIManager.uimanager.AQuickSlotItemCooltime -= QuickSlotCooltimeUpdate;
    }
    private void Awake()
    {
        itemListType = ITEMLISTTYPE.QUICK;        
    }


    void QuickSlotCooltimeUpdate(int _itemIndex, float _percent)
    {
        List<int> sameItemIndex = character.quickSlot.GetSameItemIndexList(_itemIndex);
        if(sameItemIndex == null)
        {
            return;
        }
        else
        {
            for (int i = 0; i < sameItemIndex.Count; i++)
            {
                quickItemSlots[sameItemIndex[i]].SetCoolTime(_percent);
            }
        }
    }

    void QucikSlotCooltimeUpdate_Skill(int _itemIndex, float _percent)
    {

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
    }

    protected void LeftClickDown(Vector2 _ClickPos)
    {
        for (int itemMoveUiIndex = 0; itemMoveUiIndex < quickItemSlots.Length; itemMoveUiIndex++)
        {
            if (quickItemSlots[itemMoveUiIndex].isInRect(_ClickPos) && !quickItemSlots[itemMoveUiIndex].isEmpty())
            {
                leftClickIndex = itemMoveUiIndex;
            }
        }
    }


    protected void LeftClickUp(Vector2 _ClickPos)
    {
        if (leftClickIndex >= 0)
        {
            UIManager.uimanager.ItemClickEnd(itemListType, leftClickIndex, _ClickPos);
        }

        leftClickIndex = -1;
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

    protected void RightClickDown(Vector2 _ClickPos)
    {
        for (int itemMoveUiIndex = 0; itemMoveUiIndex < quickItemSlots.Length; itemMoveUiIndex++)
        {
            if (quickItemSlots[itemMoveUiIndex].isInRect(_ClickPos) && !quickItemSlots[itemMoveUiIndex].isEmpty())
            {
                rightClickindex = itemMoveUiIndex;
            }
        }
    }


    protected void RightClickUp(Vector2 _ClickPos)
    {
        for (int index = 0; index < quickItemSlots.Length; index++)
        {
            if (quickItemSlots[index].isInRect(_ClickPos) && !quickItemSlots[index].isEmpty() && rightClickindex == index)
            {
                character.ItemMove_Auto(itemListType, index);
                UpdateSlot(itemListType, index);
            }
        }

        rightClickindex = -1;
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
        if (0 < leftClickIndex)
        {
            UIManager.uimanager.MoveItemIcon(itemListType, leftClickIndex, _dragdata.position);
        }
    }
}
