using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public Image ItemImage;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemPrice;
    public Image TouchItem;
    public RectTransform tr;

    public Item item = null;
    Rect rc;
    public Rect RC
    {
        get
        {
            rc.x = tr.position.x - tr.rect.width * 0.5f;
            rc.y = tr.position.y + tr.rect.height * 0.5f;
            return rc;
        }
    }


    void Start()
    {
        rc.x = tr.transform.position.x - tr.rect.width / 2;
        rc.y = tr.transform.position.y + tr.rect.height / 2;
        rc.xMax = tr.rect.width;
        rc.yMax = tr.rect.height;
        rc.width = tr.rect.width;
        rc.height = tr.rect.height;
    }

    public void AddItem(Item _item)
    {
        item = _item;
        ItemImage.sprite = ResourceManager.resource.GetImage(_item.itemSpriteName);
        ItemName.gameObject.SetActive(true);
        ItemName.text = item.ItemName;
        ItemPrice.gameObject.SetActive(true);
        ItemPrice.text = item.ItemPrice.ToString();
        ItemImage.gameObject.SetActive(true);        

    }


    public void SlotClear()
    {
        ItemImage.sprite = null;
        ItemImage.gameObject.SetActive(false);
        ItemName.text = string.Empty;
        ItemName.gameObject.SetActive(false);
        ItemPrice.text = string.Empty;
        ItemPrice.gameObject.SetActive(false);
        
    }
    public bool isInRect(Vector2 pos)
    {
        if (pos.x >= RC.x && pos.x <= RC.x + RC.width && pos.y >= RC.y - RC.height && pos.y <= RC.y)
        {
            
            return true;
        }
        return false;
    }

    
}
