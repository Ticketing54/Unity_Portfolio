using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


[System.Serializable]
public class Item
{
    public int SlotNum;     
    public int ItemCount;

    public int Index;
    public int E_Index;
    public ItemType itemType;
    public string ItemName;
    public string ItemExplain;
    public string ItemProperty;
    public string itemSpriteName;
    public int ItemPrice;
    
    

    


    
    public Item(int _Index, string _ItemType,string _ItemName, string _ItemSpriteName,string _ItemExplain, string _ItemProperty, int _ItemPrice, int _E_Index)
    {
        Index = _Index;        
        itemType = (ItemType)Enum.Parse(typeof(ItemType), _ItemType);
        ItemName = _ItemName;
        itemSpriteName = _ItemSpriteName;
        ItemExplain = _ItemExplain;
        ItemProperty = _ItemProperty;
        ItemPrice = _ItemPrice;                
        E_Index = _E_Index;
        
    }    

  
 


    public enum ItemType
    {
        Equipment,
        Used,        
        Etc
    }

 
    
}
