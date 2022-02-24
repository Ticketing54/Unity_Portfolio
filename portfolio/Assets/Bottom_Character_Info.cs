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
        Hp_bar.fillAmount = Character.Player.Stat.HP / Character.Player.Stat.MAXHP;
        Hp_text.text = ((int)Character.Player.Stat.HP).ToString() + " / " + ((int)Character.Player.Stat.HP).ToString();
        Mp_bar.fillAmount = Character.Player.Stat.MP / Character.Player.Stat.MAXMP;
        Mp_text.text = ((int)Character.Player.Stat.MP).ToString() + " / " + ((int)Character.Player.Stat.MAXMP).ToString();        

        Lev.text = "Level : " + Character.Player.Stat.LEVEL.ToString();
        Exp_Text.text = Character.Player.Stat.EXP.ToString() + " / " + Character.Player.Stat.MAXEXP.ToString();
        Exp_bar.fillAmount = Character.Player.Stat.EXP / Character.Player.Stat.MAXEXP;
    }

}
