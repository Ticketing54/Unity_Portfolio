using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BuffImage : Image
{
    public Image bufsprite;
    public Image cooltime_image;
    public TextMeshProUGUI cooltime_num;


    public void Clear()
    {
        bufsprite = null;
        cooltime_image = null;
        cooltime_num.text = "0";
    }
}
