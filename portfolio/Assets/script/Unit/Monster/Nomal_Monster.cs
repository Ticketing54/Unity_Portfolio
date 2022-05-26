using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nomal_Monster : Monster
{
    public override void Damaged(DAMAGE _type, float _dmg)
    {
        if(action != null)
        {
            StopCoroutine(action);
        }

        action = StartCoroutine(CoDamaged());

        float finalyDmg = 0;
        switch (_type)
        {
            case DAMAGE.NOMAL:
                {
                    finalyDmg = _dmg;
                    this.hp_Cur -= finalyDmg;
                }
                break;
            case DAMAGE.CRITICAL:
                {
                    finalyDmg = _dmg * 2;
                    this.hp_Cur -= finalyDmg;
                }
                break;
        }
        UIManager.uimanager.uiEffectManager.LoadDamageEffect(finalyDmg, this.gameObject, _type);

        if (hp_Cur > 0)
        {
            anim.SetTrigger("Damaged");
        }
        else
        {
            anim.SetTrigger("Die");
        }
    }

   
}
