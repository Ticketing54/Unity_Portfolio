using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems

[System.Serializable]
public abstract class UI_ItemSlots : MonoBehaviour,ItemUiContorl, IPointerDownHandler,IPointerUpHandler
{
    [SerializeField]
    protected ItemSlot[] ItemSlots;
    [SerializeField]
    protected int Count;
    [SerializeField]
    protected ItemMove itemMove;

   
    public virtual void OnEnable()
    {
        UpdateItemSlots();
        UIManager.uimanager.dragStartItem += ClickdownItemSlot;
        UIManager.uimanager.dragEndItem   += ClickUpitemSlot;       

    }
    public virtual void OnDisable()
    {
        UIManager.uimanager.dragStartItem -= ClickdownItemSlot;
        UIManager.uimanager.dragEndItem   -= ClickUpitemSlot;
    }
    

    public void UpdateSlot(ItemMove _itemMove, int _index)
    {
        if(itemMove != _itemMove)
        {
            return;
        }
        else
        {
            ItemSlots[_index].Add(itemMove.GetImage(_index), itemMove.GetItemCount(_index));
        }
    }

    int? isInRect_Down(Vector2 _ClickPos)
    {
        for (int invenUiIndex = 0; invenUiIndex < ItemSlots.Length; invenUiIndex++)
        {
            if (ItemSlots[invenUiIndex].isInRect(_ClickPos) && !ItemSlots[invenUiIndex].isEmpty())      
            {
                return invenUiIndex;
            }

        }
        return null;
    }
    int? isInRect_Up(Vector2 _ClickPos)
    {
        for (int invenUiIndex = 0; invenUiIndex < ItemSlots.Length; invenUiIndex++)
        {
            if (ItemSlots[invenUiIndex].isInRect(_ClickPos))      
            {
                return invenUiIndex;
            }

        }
        return null;
    }
    public void ClickdownItemSlot(Vector2 _ClickPos)
    {
        int? ClickPoint = isInRect_Down(_ClickPos);
        if (ClickPoint != null)
        {
            ItemSlots[(int)ClickPoint].ClickedSlot_Start();
            UIManager.uimanager.StartDragItem(ITEMLISTTYPE.INVEN, (int)ClickPoint, _ClickPos, ItemSlots[(int)ClickPoint]);
            
        }
    }
    public void ClickUpitemSlot(ITEMLISTTYPE _StartListType, int _StartListIndex, Vector2 _Pos)
    {
        if (UIManager.uimanager.SameClickPos(_Pos))
        {
            UIManager.uimanager.LeftClick();
            return;
        }
        int? ClickPoint = isInRect_Up(_Pos);
        if (ClickPoint != null)
        {
            int index = (int)ClickPoint;
            if (Character.Player.ItemMove(_StartListType, ITEMLISTTYPE.INVEN, _StartListIndex, index))
            {
                ItemSlots[index].Add(itemMove.GetImage(index), itemMove.GetItemCount(index));
            }
            else
            {
                UIManager.uimanager.MovingFail();
            }
        }
    }

    public abstract void SetItemMove();
    
    public virtual void UpdateItemSlots()
    {

        if(itemMove == null)
        {
            SetItemMove();
        }

        for (int itemIndex = 0; itemIndex < Count-1; itemIndex++)
        {
            ItemSlots[itemIndex].Add(itemMove.GetImage(itemIndex), itemMove.GetItemCount(itemIndex));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(0))
        {
            UIManager.uimanager.StartDragItem()
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Input.GetMouseButtonUp(0))
        {

        }
    }

    public virtual void UpdateItemSlot(int _Index, string _SprtieName, int _ItemCount)
    {
        throw new System.NotImplementedException();
    }

    public virtual void ClearSlot(int _Index)
    {
        ItemSlots[_Index].Clear();
    }
}

