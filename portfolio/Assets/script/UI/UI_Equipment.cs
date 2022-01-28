using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class UI_Equipment : MonoBehaviour
{
    [SerializeField]
    ItemSlot[] Equip = new ItemSlot[5];    
    public delegate void Setting(int _SlotNum, ITEMLISTTYPE _ListType, Sprite _Sprite);
    public delegate void EndSetting(int _SlotNum, ITEMLISTTYPE _ListType);
    public bool ClickDownEquip(Setting _Setting, Vector2 _ClickPos)
    {
        for (int i = 0; i < Equip.Length; i++)
        {            
            if (Equip[i].isInRect(_ClickPos) && !Character.Player.Equip.IsEmpty((EQUIPTYPE)i))
            {
                _Setting(i, ITEMLISTTYPE.EQUIP, Equip[i].ICON);                
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
                _Setting(i, ITEMLISTTYPE.EQUIP);                
                return true;
            }

        }
        return false;
    }
    public void UpdateSlot(int _Num)
    {
        Equip[_Num].Add(ITEMLISTTYPE.EQUIP);
    }
    public void UpdateEquip()
    {
        List<EQUIPTYPE> Items = Character.Player.Equip.GetKeys();
        Item item;
        foreach (EQUIPTYPE one in Items)
        {
            Equip[(int)one].Add(ITEMLISTTYPE.EQUIP);
        }
    }
}
