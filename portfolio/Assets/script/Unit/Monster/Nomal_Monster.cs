using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nomal_Monster : Monster
{
    public override void Damaged(DAMAGE _type, float _dmg)
    {
        if(Hp_Curent <= 0)
        {
            return;
        }
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
                    Hp_Curent -= finalyDmg;
                }
                break;
            case DAMAGE.CRITICAL:
                {
                    finalyDmg = _dmg * 2;
                    Hp_Curent -= finalyDmg;
                }
                break;
        }
       
        UIManager.uimanager.uiEffectManager.LoadDamageEffect(finalyDmg, this.gameObject, _type);
        anim.SetTrigger("Damaged");
    }

   
}
