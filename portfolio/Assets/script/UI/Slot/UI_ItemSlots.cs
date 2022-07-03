using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

[System.Serializable]
public abstract class UI_ItemSlots : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField]
    protected ItemSlot[] itemSlots;
    protected Character character;
    protected ITEMLISTTYPE itemListType;    

    public delegate Item GetItemInfo(int _Index);

    int leftClickIndex = -1;
    int rightClickindex = -1;
  
    public virtual void OnEnable()
    {
        character = GameManager.gameManager.character;
        UpdateAllSlot();
        UIManager.uimanager.ItemUpdateSlot += UpdateSlot;
        UIManager.uimanager.ItemClickUp += IsinRectLeftClick;
        
    }
    public virtual void OnDisable()
    {
        UIManager.uimanager.ItemUpdateSlot -= UpdateSlot;
        UIManager.uimanager.ItemClickUp -= IsinRectLeftClick;
    }
    #region UpdateSlot
    public abstract void UpdateAllSlot();
    
    public virtual void UpdateSlot(ITEMLISTTYPE _itemListType, int _index)
    {

        if (_itemListType != itemListType)
        {
            return;
        }

        Item getitem = GameManager.gameManager.character.ItemList_GetItem(itemListType, _index);
        if (getitem == null)
        {
            itemSlots[_index].Clear();
            return;
        }
        itemSlots[_index].Add(getitem.itemSpriteName, getitem.ItemCount);
    }
    #endregion
    protected void LeftClickDown(Vector2 _ClickPos)
    {   
        for (int itemMoveUiIndex = 0; itemMoveUiIndex < itemSlots.Length; itemMoveUiIndex++)
        {
            if (itemSlots[itemMoveUiIndex].isInRect(_ClickPos) && !itemSlots[itemMoveUiIndex].isEmpty())
            {
                leftClickIndex = itemMoveUiIndex;
            }
        }
    }

    
    protected  void LeftClickUp(Vector2 _ClickPos)
    {
        if(leftClickIndex >= 0)
        {
            UIManager.uimanager.ItemClickEnd(itemListType, leftClickIndex, _ClickPos);
        }

        leftClickIndex = -1;
    }

    protected void IsinRectLeftClick(ITEMLISTTYPE _type,int _slotNum, Vector2 _pos)
    {
        for (int index = 0; index < itemSlots.Length; index++)
        {
            if (itemSlots[index].isInRect(_pos))
            {
                character.ItemMove(_type, itemListType, _slotNum, index);
            }
        }
    }

    protected void RightClickDown(Vector2 _ClickPos)
    {
        for (int itemMoveUiIndex = 0; itemMoveUiIndex < itemSlots.Length; itemMoveUiIndex++)
        {
            if (itemSlots[itemMoveUiIndex].isInRect(_ClickPos) && !itemSlots[itemMoveUiIndex].isEmpty())
            {
                rightClickindex = itemMoveUiIndex;
            }
        }
    }


    protected void RightClickUp(Vector2 _ClickPos)
    {
        for (int index = 0; index < itemSlots.Length; index++)
        {
            if (itemSlots[index].isInRect(_ClickPos) && !itemSlots[index].isEmpty() && rightClickindex == index)
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
        if(0 < leftClickIndex)
        {
            UIManager.uimanager.MoveItemIcon(itemListType, leftClickIndex, _dragdata.position);
        }
    }
}

