using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Equipment : ItemMove
{    
    [SerializeField]
    Status Stat= null;    
    [SerializeField]
    Item[] Equip = new Item[5];

    Action openEquip;

    Character character;
    
    public Equipment(Character _character)
    {
        character = _character;        
        Stat = character.stat;
        _character.AddKeyBoardSortCut(KeyCode.P, TryOpenEquipment);
    }

    #region Open/Close
    bool equipmentActive = false;
    void TryOpenEquipment()
    {
        equipmentActive = !equipmentActive;
        if (equipmentActive)
        {
            UIManager.uimanager.AOpenEquipment();
        }
        else
        {
            UIManager.uimanager.ACloseEquipment();
        }
    }
    
    #endregion
    public Item GetItem(int _equipType)
    {
        return  Equip[_equipType];
    }

    public Item PopItem(int _Index)
    {
        Item popItem = GetItem(_Index);
        Equip[_Index] = null;
        Stat.TakeOffStatus(popItem);
        return popItem;
    }
    public void AddItem(int _index,Item _NewItem)
    {
        if (_NewItem != null)
        {
            if (_NewItem.itemType != ITEMTYPE.EQUIPMENT )
            {
                Debug.LogError("잘못된 아이템을 장비로 장착하려합니다");
                return;
            }
            Stat.EquipStatus(_NewItem);
        }        

        Equip[_index] = _NewItem;   
        
        
    }

    public bool PossableMoveItem(int _index, Item _MoveItem)
    {
        if (_MoveItem == null)
        {
            return true;
        }
        else
        {
            if(_MoveItem.itemType != ITEMTYPE.EQUIPMENT)
            {
                return false;
            }
            else if((int)_MoveItem.equipType != _index)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    public string EqipInfo()
    {
        string Data = string.Empty;
        for (int i = 0; i < Equip.Length; i++)
        {

            if (Equip[i] != null)
            {
                if (Data != string.Empty)
                {
                    Data += "/";
                }
                Data += Equip[i].index + "," + i;
            }
        }
        return Data;

    }

}
