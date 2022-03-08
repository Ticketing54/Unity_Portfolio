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
    TextMeshProUGUI Property;


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
    public void SetMiniInfo(Item _item, Vector2 Pos)    //정보 표시
    {
        if (_item == null)
            return;
        if(Pos.y <= Rect.rect.height)
        {
            transform.position = Pos + Preset_Down;
        }
        else
        {
            transform.position = Pos + Preset_Up;
        }
        
        ItemImage.sprite = GameManager.gameManager.resource.GetImage(_item.itemSpriteName);
        ItemName.text = _item.ItemName;
        ItemType.text = _item.itemType.ToString();
        ExPlain.text = _item.ItemExplain;

        if (_item.ItemProperty == "")
        {
            Property.text = "";
        }            
        else
        {
            string[] tmp = _item.ItemProperty.Split('/');
            Property.text = _item.ItemProperty;
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
