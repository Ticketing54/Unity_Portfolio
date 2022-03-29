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

    private void Update()
    {
        GetCharacterData();
    }

    public void GetCharacterData()
    {
        
        NickName.text = Character.Player.name;
        Lv.text = Character.Player.STAT.LEVEL.ToString();
        Exp.text = Character.Player.STAT.EXP.ToString() + " / " + Character.Player.STAT.MAXEXP.ToString();        
        Hp.text = ((int)Character.Player.STAT.HP).ToString() + " / " + Character.Player.STAT.MAXHP.ToString();
        Mp.text = ((int)Character.Player.STAT.MP).ToString() + " / " + Character.Player.STAT.MAXMP.ToString();
        Atk.text = Character.Player.STAT.ATK.ToString() + "<color=#0000ffff> + " + Character.Player.STAT.ATK.ToString() + "</color>"//장비 능력치 추가 할 것
            + " ( " + ((int)((Character.Player.STAT.ATK + Character.Player.STAT.ATK) * 0.8)).ToString() + " ~ " +
            ((int)((Character.Player.STAT.ATK + Character.Player.STAT.ATK) * 1.2)).ToString()+" ) ";        
        
    }
}
