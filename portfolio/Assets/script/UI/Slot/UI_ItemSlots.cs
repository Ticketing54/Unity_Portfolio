using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

[System.Serializable]
public abstract class UI_ItemSlots : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    protected ItemSlot[] itemSlots;
    
    protected ITEMLISTTYPE itemListType;

    public delegate Item GetItemInfo(int _Index);
    protected Character character;
    int rightClickindex = -1;
  
    public virtual void OnEnable()
    {
        character = GameManager.gameManager.character;
        UpdateAllSlot();
        UIManager.uimanager.itemoveEnd += this.LeftClickUp;
        UIManager.uimanager.updateUiSlot+= this.UpdateSlot;
    }
    public virtual void OnDisable()
    {
        UIManager.uimanager.itemoveEnd -= this.LeftClickUp;
        UIManager.uimanager.updateUiSlot -= this.UpdateSlot;
    }

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
        itemSlots[_index].Add(getitem.itemSpriteName, getitem.ItemCount,getitem.index);
    }

    protected void LeftClickDown(Vector2 _ClickPos)
    {
        int index = isInSlots_Down(_ClickPos);
        if (index >= 0)
        {
            UIManager.uimanager.StartDragItem(itemSlots[index], itemListType, index);
        }             
    }

    
    protected  void LeftClickUp(Vector2 _ClickPos)
    {
        int index = isInSlots_Up(_ClickPos);
        if (index >= 0)
        {
            UIManager.uimanager.ClickUpitemSlot(itemListType, index);
        }
        
    }
    protected void RightClickDown(Vector2 _ClickPos)
    {
        rightClickindex = -1;               // 초기화

        int index = isInSlots_Down(_ClickPos);
        if (index >= 0)
        {
            rightClickindex = index;
        }
    }


    protected void RightClickUp(Vector2 _ClickPos)
    {
        int index = isInSlots_Up(_ClickPos);
        if (index >= 0 && rightClickindex == index)
        {
            GameManager.gameManager.character.ItemMove_Auto(itemListType, index);
        }

    }
    int isInSlots_Down(Vector2 _ClickPos)
    {
        for (int itemMoveUiIndex = 0; itemMoveUiIndex < itemSlots.Length; itemMoveUiIndex++)
        {
            if (itemSlots[itemMoveUiIndex].isInRect(_ClickPos) && !itemSlots[itemMoveUiIndex].isEmpty())
            {
                return itemMoveUiIndex;
            }
        }

        return -1;
    }
    int isInSlots_Up(Vector2 _ClickPos)
    {
        for (int itemMoveUiIndex = 0; itemMoveUiIndex < itemSlots.Length; itemMoveUiIndex++)
        {
            if (itemSlots[itemMoveUiIndex].isInRect(_ClickPos))
            {
                return itemMoveUiIndex;
            }

        }
        return -1;
    }



    public void OnPointerDown(PointerEventData clickPoint)
    {
        if (Input.GetMouseButtonDown(0))
        {
            LeftClickDown(clickPoint.position);
        }
        if (Input.GetMouseButtonDown(1))
        {
            RightClickDown(clickPoint.position);
        }
    }

    public void OnPointerUp(PointerEventData clickPoint)
    {
        if (Input.GetMouseButtonUp(1))
        {
            RightClickUp(clickPoint.position);
        }
    }
}

