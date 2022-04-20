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
        Hp_bar.fillAmount = GameManager.gameManager.character.stat.HP / GameManager.gameManager.character.stat.MAXHP;
        Hp_text.text = ((int)GameManager.gameManager.character.stat.HP).ToString() + " / " + ((int)GameManager.gameManager.character.stat.HP).ToString();
        Mp_bar.fillAmount = GameManager.gameManager.character.stat.MP / GameManager.gameManager.character.stat.MAXMP;
        Mp_text.text = ((int)GameManager.gameManager.character.stat.MP).ToString() + " / " + ((int)GameManager.gameManager.character.stat.MAXMP).ToString();        

        Lev.text = "Level : " + GameManager.gameManager.character.stat.LEVEL.ToString();
        Exp_Text.text = GameManager.gameManager.character.stat.EXP.ToString() + " / " + GameManager.gameManager.character.stat.MAXEXP.ToString();
        Exp_bar.fillAmount = GameManager.gameManager.character.stat.EXP / GameManager.gameManager.character.stat.MAXEXP;
    }

}
