using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UI_QuickSlot : MonoBehaviour
{
    [SerializeField]
    ItemSlot[] QuickItem = new ItemSlot[4];
    
    public delegate void StartSetting(int _SlotNum, ITEMLISTTYPE _ListType, Sprite _Sprite);
    public delegate void EndSetting(int _SlotNum, ITEMLISTTYPE _ListType);
    private void OnEnable()
    {
        UIManager.uimanager.dragEndItem += ClickUpItem;
    }
    private void OnDisable()
    {
        UIManager.uimanager.dragEndItem -= ClickUpItem;
    }
    int? isInRect(Vector2 _ClickPos)
    {
        for (int i = 0; i < QuickItem.Length; i++)
        {
            if (QuickItem[i].isInRect(_ClickPos))
            {
                return i;
            }

        }
        return null;
    }


    public void ClickUpItem(ITEMLISTTYPE _StartListType,int _StartListIndex,Vector2 _ClickPos)
    {
        int? ClickPoint = isInRect(_ClickPos);

        if(ClickPoint != null)
        {
            Character.Player.ItemMove()



            // 성공 실패 나눌것

        }
        else
        {
            return;
        }

    }
    public bool ClickDownQuick_Item(StartSetting _Setting, Vector2 _ClickPos)
    {
        for (int i = 0; i < QuickItem.Length; i++)
        {
            if (QuickItem[i].isInRect(_ClickPos) && !Character.Player.QuickSlot.IsEmpty_Item(i))
            {
                _Setting(i, ITEMLISTTYPE.QUICK, QuickItem[i].ICON);                
                return true;
            }

        }       
        return false;
    }
    public bool ClickUpQuick_Item(EndSetting _Setting ,Vector2 _ClickPos)
    {
        for (int i = 0; i < QuickItem.Length; i++)
        {
            if (QuickItem[i].isInRect(_ClickPos))
            {
                _Setting(i, ITEMLISTTYPE.QUICK);
                return true;
            }

        }
        return false;
    }
    public void UpdateSlot(int _Num)
    {
        QuickItem[_Num].Add(ITEMLISTTYPE.QUICK);
    }
    public void UpdateClear(int _Num)
    {
        QuickItem[_Num].Clear();
    }
   
}
