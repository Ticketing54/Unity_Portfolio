using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UI_Inventory: MonoBehaviour
{
    [SerializeField]
    ItemSlot[] Inven = new ItemSlot[18];    
    public delegate void Setting(int _SlotNum, ItemListType _ListType, Sprite _Sprite);
    public delegate void EndSetting(int _SlotNum, ItemListType _ListType);
    public bool ClickdownInven(Setting _Setting, Vector2 _ClickPos)
    {
        for (int i = 0; i < Inven.Length; i++)
        {
            if (Inven[i].isInRect(_ClickPos) && !Character.Player.Inven.IsEmpty(i))
            {
                _Setting(i, ItemListType.INVEN, Inven[i].ICON);                
                return true;
            }

        }
        return false;
    }
    public bool ClickUpInven(EndSetting _Setting, Vector2 _ClickPos)
    {
        for (int i = 0; i < Inven.Length; i++)
        {
            if (Inven[i].isInRect(_ClickPos))
            {
                _Setting(i, ItemListType.INVEN);                
                return true;
            }

        }
        return false;
    }
    public void UpdateSlot(int _Num)
    {
        Inven[_Num].Add(ItemListType.INVEN);
    }
    public void UpdateInven()
    {
        List<int> Items = Character.Player.Inven.GetKeys();
        Item item;
        foreach(int one in Items)
        {
            Inven[one].Add(ItemListType.INVEN);
        }
    }
}
