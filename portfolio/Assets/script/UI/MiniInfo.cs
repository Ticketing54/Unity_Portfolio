using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniInfo : MonoBehaviour
{
    [SerializeField]
    Image ItemImage;
    [SerializeField]
    RectTransform Rect;
    [SerializeField]
    TextMeshProUGUI ItemName;
    [SerializeField]
    TextMeshProUGUI ItemType;
    [SerializeField]
    TextMeshProUGUI ExPlain;
    [SerializeField]
    TextMeshProUGUI property;


    Vector2 Preset_Up;
    Vector2 Preset_Down;
    float width;
    float height;

    private void Awake()
    {
        width = (Rect.rect.width / 2);
        height = (Rect.rect.height / 2);
        Preset_Up = new Vector2(width, -height);
        Preset_Down = new Vector2(width, height);
        
    }
    public void SetMiniInfo(int _itemIndex, Vector2 Pos)    //정보 표시
    {

        Item item = new Item(_itemIndex);
        if (item == null)
            return;
        if(Pos.y <= Rect.rect.height)
        {
            transform.position = Pos + Preset_Down;
        }
        else
        {
            transform.position = Pos + Preset_Up;
        }
        
        ItemImage.sprite = ResourceManager.resource.GetImage(item.itemSpriteName);
        ItemName.text = item.itemName;
        ItemType.text = item.itemType.ToString();
        ExPlain.text = item.itemExplain;

        if (string.IsNullOrEmpty(item.itemProperty))
        {
            property.text = "";
        }            
        else
        {            
            string[] arrProperty = item.itemProperty.Split(',');
            string propertyText = string.Empty;
            property.text = propertyText;
            switch (item.itemType)
            {
                case ITEMTYPE.EQUIPMENT:
                    {
                        for (int i = 0; i < arrProperty.Length; i++)
                        {
                            string[] subProperty = arrProperty[i].Split('/');
                            string name = subProperty[0];
                            string ability = subProperty[1];
                            property.text += name + " "+"<color=red>"+ ability + "</color>" + '\n';                            
                        }                         
                    }
                    break;
                case ITEMTYPE.USED:
                    {
                        for (int i = 0; i < arrProperty.Length; i++)
                        {
                            string[] subProperty = arrProperty[i].Split('/');
                            string name     = subProperty[0];
                            string duration = subProperty[1];
                            string ability  = subProperty[2];
                            property.text += name + " " + "<color=red>" + ability + "</color>" + '\n';
                        }                         
                    }
                    break;
                case ITEMTYPE.ETC:
                    property.text = item.itemProperty;
                    break;
                default:
                    break;
            }
            
            
            
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.gameObject.SetActive(false);
        }
    }

}
