using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopSlot : Slot
{   
    [SerializeField]  
    TextMeshProUGUI itemName_Text;
    [SerializeField]
    TextMeshProUGUI itemPrice_Text;
    [SerializeField]
    Image touchItem;
    [SerializeField]
    Text itemCount_Text;
    
    public int itemCount;
    public string itemName;
    public int itemPrice;

   
    public void SetShopSlot(int _itemIndex)
    {
        
        List<string> itemInfo = ResourceManager.resource.GetTable_Index("ItemTable", _itemIndex);
        if(itemInfo == null)
        {
            Debug.LogError("테이블에 없는 아이템을 상정에 올릴려 합니다.");
        }
        itemName = itemInfo[2];
        icon.gameObject.SetActive(true);
        icon.sprite = ResourceManager.resource.GetImage(itemInfo[3]);
        itemName_Text.gameObject.SetActive(true);
        itemName_Text.text = itemInfo[2];

        if(!int.TryParse(itemInfo[6],out itemPrice))
        {
            itemPrice = -1;
        }
        itemPrice_Text.gameObject.SetActive(true);
        itemPrice_Text.text = itemInfo[6];
    }
    public void SetShopSlot_Inven(string _itemName,string _itemSpriteName,int _itemPrice,int _itemCount)
    {   
        itemName = _itemName;
        icon.gameObject.SetActive(true);
        icon.sprite = ResourceManager.resource.GetImage(_itemSpriteName);
        itemPrice = _itemPrice;
        itemCount = _itemCount;
        if(_itemCount > 1)
        {
            itemCount_Text.gameObject.SetActive(true);
            itemCount_Text.text = _itemCount.ToString();
        }
    }
    public void ResetSlot_Inven()
    {
        if (icon.gameObject.activeSelf == true)
        {
            icon.gameObject.SetActive(false);
        }
        if (touchItem.gameObject.activeSelf == true)
        {
            touchItem.gameObject.SetActive(false);
        }
        
        itemPrice = -1;
        itemName = string.Empty;
        if(itemCount_Text.gameObject.activeSelf == true)
        {
            itemCount_Text.gameObject.SetActive(false);
        }    
    }
    public void ResetSlot()
    {
        if(icon.gameObject.activeSelf == true)
        {
            icon.gameObject.SetActive(false);
        }
        if(itemName_Text.gameObject.activeSelf == true)
        {
            itemName_Text.gameObject.SetActive(false);
        }
        if(itemPrice_Text.gameObject.activeSelf == true)
        {
            itemPrice_Text.gameObject.SetActive(false);
        }
        if(touchItem.gameObject.activeSelf == true)
        {
            touchItem.gameObject.SetActive(false);
        }                
        itemPrice = -1;
        itemName = string.Empty;
    }
    public void ClickedEffectOn()
    {
        touchItem.gameObject.SetActive(true);
    }
    public void ClickedEffectOff()
    {
        touchItem.gameObject.SetActive(false);
    }

   
}
