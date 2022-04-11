using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Bottom_Character_Info : MonoBehaviour
{
    [SerializeField]
    Image Hp_bar;                       // Hpbar_Image
    [SerializeField]
    Image Mp_bar;                       // Mpbar_Image
    [SerializeField]
    TextMeshProUGUI Hp_text;            // Hpbar_Text
    [SerializeField]
    TextMeshProUGUI Mp_text;            // Mpbar_Text
    [SerializeField]
    Image Exp_bar;                      // Exp_bar_Image
    [SerializeField]
    TextMeshProUGUI Exp_Text;           // Exp_Text
    [SerializeField]
    TextMeshProUGUI Lev;                // Lev_Text


    public void InfoUpdate()
    {
        Hp_bar.fillAmount = Character.Player.stat.HP / Character.Player.stat.MAXHP;
        Hp_text.text = ((int)Character.Player.stat.HP).ToString() + " / " + ((int)Character.Player.stat.HP).ToString();
        Mp_bar.fillAmount = Character.Player.stat.MP / Character.Player.stat.MAXMP;
        Mp_text.text = ((int)Character.Player.stat.MP).ToString() + " / " + ((int)Character.Player.stat.MAXMP).ToString();        

        Lev.text = "Level : " + Character.Player.stat.LEVEL.ToString();
        Exp_Text.text = Character.Player.stat.EXP.ToString() + " / " + Character.Player.stat.MAXEXP.ToString();
        Exp_bar.fillAmount = Character.Player.stat.EXP / Character.Player.stat.MAXEXP;
    }

}
