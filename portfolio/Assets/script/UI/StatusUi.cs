using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class StatusUi : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI NickName;
    [SerializeField]
    TextMeshProUGUI Lv;
    [SerializeField]
    TextMeshProUGUI Exp;    
    [SerializeField]
    TextMeshProUGUI Hp;
    [SerializeField]
    TextMeshProUGUI Mp;
    [SerializeField]
    TextMeshProUGUI Atk;
    [SerializeField]
    TextMeshProUGUI Gold;
    


    private void Update()
    {
        GetCharacterData();
    }

    public void GetCharacterData()
    {
        
        NickName.text = Character.Player.name;
        Lv.text = Character.Player.Stat.LEVEL.ToString();
        Exp.text = Character.Player.Stat.EXP.ToString() + " / " + Character.Player.Stat.MAXEXP.ToString();        
        Hp.text = ((int)Character.Player.Stat.HP).ToString() + " / " + Character.Player.Stat.MAXHP.ToString();
        Mp.text = ((int)Character.Player.Stat.MP).ToString() + " / " + Character.Player.Stat.MAXMP.ToString();
        Atk.text = Character.Player.Stat.ATK.ToString() + "<color=#0000ffff> + " + Character.Player.Stat.ATK.ToString() + "</color>"//장비 능력치 추가 할 것
            + " ( " + ((int)((Character.Player.Stat.ATK + Character.Player.Stat.ATK) * 0.8)).ToString() + " ~ " +
            ((int)((Character.Player.Stat.ATK + Character.Player.Stat.ATK) * 1.2)).ToString()+" ) ";
        Gold.text = Character.Player.Stat.GOLD.ToString() + " G";
        
    }
}
