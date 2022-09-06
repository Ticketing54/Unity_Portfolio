using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
    
public class TopEnermyInfoUi : MonoBehaviour
{   
    [SerializeField]
    Text enermy_Name;
    [SerializeField]
    Image enermy_Hp;
    [SerializeField]
    TextMeshProUGUI enermy_Hp_Text;

    private void Start()
    {
        UIManager.uimanager.aOpenTopInfoUi += () => { gameObject.SetActive(true); };
        UIManager.uimanager.aCloseTopInfoUi += () => { gameObject.SetActive(false); };
        UIManager.uimanager.aUpdateTopinfo += Top_EnermyInfoUi;
        gameObject.SetActive(false);
    }



    void Top_EnermyInfoUi(Monster _Monster)
    {        
        if (_Monster.MightyEnermy())
        {
            enermy_Name.color = Color.red;
        }
        else
        {
            enermy_Name.color = Color.white;
        }


        enermy_Name.text = _Monster.NickName;                                                   // Name

        enermy_Hp.fillAmount = (float)(_Monster.Hp_Curent / _Monster.Hp_Max);                  // Hpbar

        enermy_Hp_Text.text = _Monster.Hp_Curent.ToString() + " / " + _Monster.Hp_Max.ToString();     // Hpbar Text
    }
    
}
