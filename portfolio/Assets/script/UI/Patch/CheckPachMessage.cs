using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CheckPachMessage : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI dotsText;

    string[] dots;
    float timer = 0;
    int dotscount = 0;
    private void Start()
    {
        dots = new string[3];
        dots[0] = ".";
        dots[1] = "..";
        dots[2] = "...";
    }
    void TextingDots()
    {
        if (dotscount > 2)
        {
            dotscount = 0;
        }

        dotsText.text = dots[dotscount];
        dotscount++;

    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 1f)
        {
            timer = 0;
            TextingDots();
        }
        
    }
}
