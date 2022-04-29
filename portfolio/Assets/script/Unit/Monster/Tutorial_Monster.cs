using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Monster : Monster
{
    public override void Damaged(DAMAGE _type,float _dmg)
    {
        if (GameManager.gameManager.character.quest.isQuestMonster(index))
        {
            GameManager.gameManager.character.quest.UpdatePlayingQuest(2,1);
        }
        float finalyDmg = 0;
        switch (_type)
        {
            case DAMAGE.NOMAL:
                {
                    finalyDmg = _dmg;                    
                }
                break;
            case DAMAGE.CRITICAL:
                {
                    finalyDmg = _dmg * 2;                    
                }
                break;
        }
        UIManager.uimanager.uiEffectManager.LoadDamageEffect(finalyDmg, this.gameObject, _type);
    }
}
