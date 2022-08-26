using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hostile_Monster : Nomal_Monster
{
    bool contact = false;
    public override void OnEnable()
    {   
        base.OnEnable();
        StartCoroutine(CoCombat());
    }
    
    protected override IEnumerator CoCombat()
    {
        float agro = 5f;
        while (true)
        {
            if (DISTANCE < 1f)
            {
                contact = true;
                agro = 5f;
                nav.SetDestination(transform.position);
                anim.SetBool("IsMove", false);


                transform.LookAt(GameManager.gameManager.character.transform.position);

                anim.SetTrigger("Attack");
                yield return new WaitForSeconds(1f);
            }
            else if (DISTANCE >= 1f && DISTANCE < 10f)
            {
                contact = true;
                agro = 5f;
                anim.SetBool("IsMove", true);
                nav.SetDestination(GameManager.gameManager.character.transform.position);
            }
            else if (DISTANCE >= 10f && contact == true)
            {
                agro -= Time.deltaTime;

                if (agro <= 0)
                {
                    contact = false;
                    action = StartCoroutine(CoResetMonster());
                    yield break;
                }

                if (nav.destination != transform.position)
                {
                    nav.SetDestination(transform.position);
                    anim.SetBool("IsMove", false);
                }

            }


            yield return null;
        }
    }
   
}

