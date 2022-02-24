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
    TextMeshProUGUI ItemName;
    [SerializeField]
    TextMeshProUGUI ItemType;
    [SerializeField]
    TextMeshProUGUI ExPlain;
    [SerializeField]
    TextMeshProUGUI Property;


    public void MiniInfoUpdate(Item _item, Vector3 Pos, Sprite _Image)    //정보 표시
    {
        if (_item == null)
            return;
        transform.position = Pos;
        ItemImage.sprite = _Image;
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
            Property.text = tmp[0]+" + "+tmp[1];
        }
    }



}
