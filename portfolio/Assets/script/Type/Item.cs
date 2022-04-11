using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


[System.Serializable]
public class Item
{
    [SerializeField]
    string name;
    [SerializeField]
    int count;
    public EQUIPTYPE equipType { get; }
    public ITEMTYPE itemType { get; }
    public int index { get; }    
    public string itemName { get => name; }
    public string itemExplain { get; }
    public string itemProperty { get; }
    public string itemSpriteName { get; }
    public int itemPrice { get; }
    public int SellPrice
    {
        get
        {
            int tmp = itemPrice / 2;
            if (tmp <= 0)
                return 1;
            return tmp;
        }
    }
    
    public int ItemCount { get => count; set => count = value; }    
    public Item(int _index,int _count=1)
    {
        List<string> itemTable = ResourceManager.resource.GetTable_Index("ItemTable", _index);
        if (itemTable == null)
        {
            Debug.LogError("테이블에 아이템정보가 없습니다.");
        }
        int t_index;
        if(int.TryParse(itemTable[0],out t_index))
        {
            index = t_index;
        }
        else
        {
            Debug.LogError("아이템 인덱스 변환이 안됬습니다.");
        }

        ITEMTYPE t_itemType;
        if(Enum.TryParse(itemTable[1],out t_itemType))
        {
            itemType= t_itemType;
        }
                
        name = itemTable[2];
        itemSpriteName = itemTable[3];
        itemExplain = itemTable[4];
        itemProperty = itemTable[5];

        int t_itemPrice;
        if (int.TryParse(itemTable[6], out t_itemPrice))
        {
            itemPrice = t_itemPrice;
        }
        else
        {
            Debug.LogError("아이템 가격 변환이 안됬습니다.");        
        }

        count = _count;

        if (string.IsNullOrEmpty(itemTable[7]))
        {
            return;
        }

        EQUIPTYPE t_E_Index;
        if (Enum.TryParse(itemTable[7],out t_E_Index))
        {
            equipType = t_E_Index;
        }
        else
        {
            Debug.LogError("장비 형식변환이 안됬습니다.");
        }        
    }
    public void UseItem()
    {
        //if (itemType == ITEMTYPE.USED)
        //{
        //    string[] Data = itemProperty.Split('/');

        //    switch (Data[0])
        //    {
        //        case "Hp":
        //            if (Character.Player.isrecovery_Hp == true || Character.Player.stat.MAXHP <= Character.Player.stat.HP)
        //                return;
        //            //SetSlotCount();
        //            //StartCoroutine(buf_character(Data[0], float.Parse(Data[1]), float.Parse(Data[2]), float.Parse(Data[3])));
        //            ItemCount -= 1;
        //            break;
        //        case "Mp":
        //            if (Character.Player.isrecovery_Mp == true || Character.Player.stat.MAXMP <= Character.Player.stat.MP)
        //                return;
        //            //SetSlotCount();
        //            //StartCoroutine(buf_character(Data[0], float.Parse(Data[1]), float.Parse(Data[2]), float.Parse(Data[3])));
        //            ItemCount -= 1;
        //            break;
        //        default:
        //            Debug.Log("사용아이템이 아닙니다.");
        //            break;
        //    }
        //}
    }








}
