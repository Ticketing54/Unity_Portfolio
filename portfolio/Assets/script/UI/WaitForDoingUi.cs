using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WaitForDoingUi : MonoBehaviour
{
    [SerializeField]
    Image gauge;
    [SerializeField]
    TextMeshProUGUI gaugeText;

    public void SetGauge(string _text)
    {
        gaugeText.text = _text;
        gauge.fillAmount = 0;
    }


    public void SetGauge(float _percent)
    {
        gauge.fillAmount = _percent;
    }
}
