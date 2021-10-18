using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UI_QuickSlot : MonoBehaviour
{
    [SerializeField]
    ItemSlot[] QuickItem = new ItemSlot[4];
    
    public delegate void StartSetting(int _SlotNum, ItemListType _ListType, Sprite _Sprite);
    public delegate void EndSetting(int _SlotNum, ItemListType _ListType);
    
    public bool ClickDownQuick_Item(StartSetting _Setting, Vector2 _ClickPos)
    {
        for (int i = 0; i < QuickItem.Length; i++)
        {
            if (QuickItem[i].isInRect(_ClickPos) && Character.Player.Quick.IsEmpty_Item(i))
            {
                _Setting(i, ItemListType.QUICK, QuickItem[i].ICON);
                QuickItem[i].Clear();
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
                _Setting(i, ItemListType.QUICK);
                return true;
            }

        }
        return false;
    }
    public void UpdateSlot(int _Num)
    {
        QuickItem[_Num].Add(ItemListType.QUICK);
    }
    public void UpdateClear(int _Num)
    {
        QuickItem[_Num].Clear();
    }
   
}
