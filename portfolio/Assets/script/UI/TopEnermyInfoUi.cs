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

    public void Top_EnermyInfoUi(MonsterUiControl _Monster)
    {        

        if (_Monster.mightyEnermy())
        {
            enermy_Name.color = Color.red;
        }
        else
        {
            enermy_Name.color = Color.white;
        }

        enermy_Name.text = _Monster.NickName;                                                   // Name

        enermy_Hp.fillAmount = (float)(_Monster.HP_Current / _Monster.HP_Max);                  // Hpbar

        enermy_Hp_Text.text = _Monster.HP_Current.ToString() + " / " + _Monster.ToString();     // Hpbar Text

        StartCoroutine(Close_Top_EnermyInfoUi(_Monster));                                        // Ui On / Off 관리
    }
    IEnumerator Close_Top_EnermyInfoUi(BattleUiControl _Monster)
    {
        while (_Monster.isClick())
        {
            yield return null;
        }

        this.gameObject.SetActive(false);
        yield break;
    }
}
