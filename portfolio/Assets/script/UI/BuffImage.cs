using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BuffImage : MonoBehaviour
{
    [SerializeField]
    Image bufImage;
    [SerializeField]
    Image cooltimeImage;
    [SerializeField]
    TextMeshProUGUI cooltimeText;

    public void UpdateBuff(float _timer, float _duration)
    {
        cooltimeImage.fillAmount = _timer / _duration;
        cooltimeText.text = ((int)(_duration - _timer)).ToString();
    }
    public void SetBuffImage(string _spriteName)
    {
        bufImage.sprite = ResourceManager.resource.GetImage(_spriteName);
        cooltimeImage.fillAmount = 0;
        cooltimeText.text = "0";
    }
    public void Clear()
    {
        bufImage.sprite = null;
        cooltimeImage.fillAmount = 0;        
        cooltimeText.text = "0";
    }
}
