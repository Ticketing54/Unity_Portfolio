using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class UI_Equipment : MonoBehaviour
{
    [SerializeField]
    ItemSlot[] Equip = new ItemSlot[5];    
    public delegate void Setting(int _SlotNum, ItemListType _ListType, Sprite _Sprite);
    public delegate void EndSetting(int _SlotNum, ItemListType _ListType);
    public bool ClickDownEquip(Setting _Setting, Vector2 _ClickPos)
    {
        for (int i = 0; i < Equip.Length; i++)
        {            
            if (Equip[i].isInRect(_ClickPos))
            {
                _Setting(i, ItemListType.EQUIP, Equip[i].ICON);                
                return true;
            }

        }
        return false;
    }
    public bool CLickUpEquip(EndSetting _Setting, Vector2 _ClickPos)
    {
        for (int i = 0; i < Equip.Length; i++)
        {            
            if (Equip[i].isInRect(_ClickPos))            
            {
                _Setting(i, ItemListType.EQUIP);                
                return true;
            }

        }
        return false;
    }
    public void UpdateSlot(int _Num)
    {
        Equip[_Num].Add(ItemListType.EQUIP);
    }
}
