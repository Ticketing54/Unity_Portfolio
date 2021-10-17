using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UI_QuickSlot : MonoBehaviour
{
    [SerializeField]
    ItemSlot[] QuickItem = new ItemSlot[18];
    
    public delegate void Setting(int _SlotNum, ItemListType _ListType, Sprite _Sprite);
    
    public bool ClickDownQuick(Setting _Setting, Vector2 _ClickPos)
    {
        for (int i = 0; i < QuickItem.Length; i++)
        {
            if (QuickItem[i].isInRect(_ClickPos) && Character.Player.Inven.IsEmpty(i))
            {
                _Setting(i, ItemListType.INVEN, QuickItem[i].ICON);
                QuickItem[i].Clear();
                return true;
            }

        }       
        return false;
    }
    public bool ClickUpQuick(Vector2 _ClickPos)
    {
        for (int i = 0; i < QuickItem.Length; i++)
        {
            if (QuickItem[i].isInRect(_ClickPos))
            {              
                return true;
            }

        }
        return false;
    }
    public void UpdateSlot(int _Num)
    {
        QuickItem[_Num].Add(ItemListType.QUICK);
    }
   
}
