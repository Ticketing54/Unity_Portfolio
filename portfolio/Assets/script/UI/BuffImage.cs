using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BuffImage : Image
{
    [SerializeField]
    Image bufImage;
    [SerializeField]
    Image cooltimeImage;
    [SerializeField]
    TextMeshProUGUI cooltimeText;

    public void OnBuffImage(string _spriteName)
    {
        cooltimeImage.fillAmount = 1;
        cooltimeText.text = "0";
    }

    public void SetBuff(float _fillamount,int _holdTime)
    {
        cooltimeImage.fillAmount = _fillamount;
        cooltimeText.text = _holdTime.ToString();
    }
    public void Clear()
    {
        bufImage = null;
        cooltimeImage = null;
        cooltimeText.text = "0";
    }
}
