using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class StatusInfo : MonoBehaviour
{


    public TextMeshProUGUI NickName;
    public TextMeshProUGUI Lv;
    public TextMeshProUGUI Exp;

    public TextMeshProUGUI Str;
    public TextMeshProUGUI Dex;
    public TextMeshProUGUI Int;
    public TextMeshProUGUI Luk;
    public TextMeshProUGUI Hp;
    public TextMeshProUGUI Mp;
    public TextMeshProUGUI Atk;
    public TextMeshProUGUI Gold;
    


    private void Update()
    {
        GetCharacterData();
    }

    public void GetCharacterData()
    {
        
        NickName.text = Character.Player.name;
        Lv.text = Character.Player.Lev.ToString();
        Exp.text = Character.Player.Exp_C.ToString() + " / " + Character.Player.Exp.ToString();
        Str.text = Character.Player.Str.ToString();
        Dex.text = Character.Player.Dex.ToString();
        Int.text = Character.Player.Int.ToString();
        Luk.text = Character.Player.Luk.ToString();
        Hp.text = ((int)Character.Player.Hp_C).ToString() + " / " + Character.Player.returnHp().ToString();
        Mp.text = ((int)Character.Player.Mp_C).ToString() + " / " + Character.Player.returnMp().ToString();
        Atk.text = Character.Player.Atk.ToString() + "<color=#0000ffff> + " + Character.Player.Atk_E.ToString() + "</color>"
            + " ( " + ((int)((Character.Player.Atk + Character.Player.Atk_E) * 0.8)).ToString() + " ~ " +
            ((int)((Character.Player.Atk + Character.Player.Atk_E) * 1.2)).ToString()+" ) ";
        Gold.text = Character.Player.Gold.ToString() + " G";
        
    }
}
