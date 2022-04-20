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
        
        NickName.text = GameManager.gameManager.character.name;
        Lv.text = GameManager.gameManager.character.stat.LEVEL.ToString();
        Exp.text = GameManager.gameManager.character.stat.EXP.ToString() + " / " + GameManager.gameManager.character.stat.MAXEXP.ToString();        
        Hp.text = ((int)GameManager.gameManager.character.stat.HP).ToString() + " / " + GameManager.gameManager.character.stat.MAXHP.ToString();
        Mp.text = ((int)GameManager.gameManager.character.stat.MP).ToString() + " / " + GameManager.gameManager.character.stat.MAXMP.ToString();
        Atk.text = GameManager.gameManager.character.stat.AttackDamage.ToString() + "<color=#0000ffff> + " + GameManager.gameManager.character.stat.AttackDamage.ToString() + "</color>"//장비 능력치 추가 할 것
            + " ( " + ((int)((GameManager.gameManager.character.stat.AttackDamage + GameManager.gameManager.character.stat.AttackDamage) * 0.8)).ToString() + " ~ " +
            ((int)((GameManager.gameManager.character.stat.AttackDamage + GameManager.gameManager.character.stat.AttackDamage) * 1.2)).ToString()+" ) ";        
        
    }
}
