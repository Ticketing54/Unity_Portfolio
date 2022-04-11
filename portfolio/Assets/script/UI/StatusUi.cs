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
        Lv.text = Character.Player.stat.LEVEL.ToString();
        Exp.text = Character.Player.stat.EXP.ToString() + " / " + Character.Player.stat.MAXEXP.ToString();        
        Hp.text = ((int)Character.Player.stat.HP).ToString() + " / " + Character.Player.stat.MAXHP.ToString();
        Mp.text = ((int)Character.Player.stat.MP).ToString() + " / " + Character.Player.stat.MAXMP.ToString();
        Atk.text = Character.Player.stat.ATK.ToString() + "<color=#0000ffff> + " + Character.Player.stat.ATK.ToString() + "</color>"//장비 능력치 추가 할 것
            + " ( " + ((int)((Character.Player.stat.ATK + Character.Player.stat.ATK) * 0.8)).ToString() + " ~ " +
            ((int)((Character.Player.stat.ATK + Character.Player.stat.ATK) * 1.2)).ToString()+" ) ";        
        
    }
}
