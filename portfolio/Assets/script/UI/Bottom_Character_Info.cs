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
        Hp_bar.fillAmount = Character.Player.STAT.HP / Character.Player.STAT.MAXHP;
        Hp_text.text = ((int)Character.Player.STAT.HP).ToString() + " / " + ((int)Character.Player.STAT.HP).ToString();
        Mp_bar.fillAmount = Character.Player.STAT.MP / Character.Player.STAT.MAXMP;
        Mp_text.text = ((int)Character.Player.STAT.MP).ToString() + " / " + ((int)Character.Player.STAT.MAXMP).ToString();        

        Lev.text = "Level : " + Character.Player.STAT.LEVEL.ToString();
        Exp_Text.text = Character.Player.STAT.EXP.ToString() + " / " + Character.Player.STAT.MAXEXP.ToString();
        Exp_bar.fillAmount = Character.Player.STAT.EXP / Character.Player.STAT.MAXEXP;
    }

}
