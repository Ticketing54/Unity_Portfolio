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

        enermy_Name.text = _Monster.UnitName();                                                   // Name

        enermy_Hp.fillAmount = (float)(_Monster.HpCur / _Monster.HpMax);                            // Hpbar

        enermy_Hp_Text.text = _Monster.HpCur.ToString() + " / " + _Monster.HpMax.ToString();     // Hpbar Text
    }
    
}
