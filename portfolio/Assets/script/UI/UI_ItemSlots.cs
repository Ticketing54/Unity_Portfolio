using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

[System.Serializable]
public abstract class UI_ItemSlots : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    [SerializeField]
    protected ItemSlot[] itemSlots;
    [SerializeField]
    protected int Count;

    protected ITEMLISTTYPE itemListType;
    
  
    public virtual void OnEnable()
    {
        
       
    }
    public virtual void OnDisable()
    {
       
    }
    

    public void UpdateSlot(int _index,  Item _item)
    {
        
        if (_item == null)
        {
            itemSlots[_index].Clear();
            return;
        }
        itemSlots[_index].Add(_item.itemSpriteName, _item.ItemCount);
    }

    int isInRect_Down(Vector2 _ClickPos)
    {
        for (int invenUiIndex = 0; invenUiIndex < itemSlots.Length; invenUiIndex++)
        {
            if (itemSlots[invenUiIndex].isInRect(_ClickPos) && !itemSlots[invenUiIndex].isEmpty())      
            {
                return invenUiIndex;
            }

        }
        return -1;
    }
    int isInRect_Up(Vector2 _ClickPos)
    {
        for (int invenUiIndex = 0; invenUiIndex < itemSlots.Length; invenUiIndex++)
        {
            if (itemSlots[invenUiIndex].isInRect(_ClickPos))      
            {
                return invenUiIndex;
            }

        }
        return -1;
    }

    


    public void OnPointerDown(PointerEventData ClickPoint)
    {
        if (Input.GetMouseButtonDown(0))
        {
            int Index = isInRect_Down(ClickPoint.position);
            if (Index < 0)
            {
                return;
            }
            else
            {
                UIManager.uimanager.StartDragItem(this,itemListType,Index);
            }
        }
    }

    public void OnPointerUp(PointerEventData ClickPoint)
    {
        if (!UIManager.uimanager.ClickUpPossable())
        {
            return;                                             // 이전 정보가 없을때 실패
        }


        if (Input.GetMouseButtonUp(0))
        {
            int Index = isInRect_Up(ClickPoint.position);
            if (Index < 0)
            {
                return;
            }
            else
            {
                UIManager.uimanager.ClickUpitemSlot(this,itemListType, Index);
                
            }
        }
    }   
}

