using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class UI_Equipment : MonoBehaviour
{
    [SerializeField]
    ItemSlot[] Equip = new ItemSlot[5];
    public delegate bool Click();
    public delegate void Setting(int _SlotNum, ItemListType _ListType, Sprite _Sprite);
    public bool ClickEquip(Setting _Setting, Vector2 _ClickPos)
    {
        for (int i = 0; i < Equip.Length; i++)
        {
            if (Equip[i].isInRect(_ClickPos) && Character.Player.Equip.IsEmpty(i))
            {
                _Setting(i, ItemListType.INVEN, Equip[i].ICON);
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
