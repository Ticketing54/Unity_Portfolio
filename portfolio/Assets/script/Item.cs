using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


[System.Serializable]
public class Item
{
    public enum ItemType
    {
        Equipment,
        Used,
        Etc
    }
    public enum EquipMentType
    {
        None ,
        HEAD ,
        Armor,
        RightArm,
        LeftArm,
        Shose
    }


    public int SlotNum;

    public EquipMentType EquipType { get; }
    public ItemType itemType { get; set; }
    public int Index { get; }
    public string ItemName { get; }
    public string ItemExplain { get; }
    public string ItemProperty { get; }
    public string itemSpriteName { get; }
    public int ItemPrice { get; }
    public int ItemCount { get; set; }    
    public Item(int _Index, int _ItemType,string _ItemName, string _ItemSpriteName,string _ItemExplain, string _ItemProperty, int _ItemPrice, int _E_Index=0,int _ItemCount=1)
    {
        Index = _Index;        
        itemType = (ItemType)_ItemType;
        ItemName = _ItemName;
        itemSpriteName = _ItemSpriteName;
        ItemExplain = _ItemExplain;
        ItemProperty = _ItemProperty;
        ItemPrice = _ItemPrice;   
        EquipType = (EquipMentType)_E_Index;
        ItemCount = _ItemCount;
    }
    public void UseItem()
    {
        if (itemType == Item.ItemType.Used)
        {
            string[] Data = ItemProperty.Split('/');

            switch (Data[0])
            {
                case "Hp":
                    if (Character.Player.isrecovery_Hp == true || Character.Player.Stat.MAXHP <= Character.Player.Stat.HP)
                        return;
                    //SetSlotCount();
                    //StartCoroutine(buf_character(Data[0], float.Parse(Data[1]), float.Parse(Data[2]), float.Parse(Data[3])));
                    ItemCount -= 1;
                    break;
                case "Mp":
                    if (Character.Player.isrecovery_Mp == true || Character.Player.Stat.MAXMP <= Character.Player.Stat.MP)
                        return;
                    //SetSlotCount();
                    //StartCoroutine(buf_character(Data[0], float.Parse(Data[1]), float.Parse(Data[2]), float.Parse(Data[3])));
                    ItemCount -= 1;
                    break;
                default:
                    Debug.Log("사용아이템이 아닙니다.");
                    break;
            }
        }
    }








}
