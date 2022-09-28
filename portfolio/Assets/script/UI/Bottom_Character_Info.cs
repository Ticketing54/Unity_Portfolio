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

    Character character;
    private void Awake()
    {
        UIManager.uimanager.AUpdateHp    += SetHp;
        UIManager.uimanager.AUpdateMp    += SetMp;
        UIManager.uimanager.AUpdateLevel += SetLev;
        UIManager.uimanager.AUpdateExp   += SetExp;
        UIManager.uimanager.OnBaseUI += UpdateBottom;
    }
    void UpdateBottom()
    {
        character = GameManager.gameManager.character;
        SetHp();
        SetMp();
        SetLev();
        SetExp();
    }
    void SetHp()
    {   
        Hp_bar.fillAmount = character.stat.Hp / character.stat.MaxHp;
        Hp_text.text = ((int)character.stat.Hp).ToString() + " / " + ((int)character.stat.MaxHp).ToString();
    }
    void SetMp()
    {
        Mp_bar.fillAmount = character.stat.Mp / character.stat.MaxMp;
        Mp_text.text = ((int)character.stat.Mp).ToString() + " / " + ((int)character.stat.MaxMp).ToString();
    }
    void SetLev()
    {
        Lev.text = "Level : " + character.stat.Level.ToString();
    }
    void SetExp()
    {
        Exp_Text.text = character.stat.Exp.ToString() + " / " + character.stat.MaxExp.ToString();
        Exp_bar.fillAmount = (float)character.stat.Exp / (float)character.stat.MaxExp;
    }
    
}
