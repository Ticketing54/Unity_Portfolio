using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic_Battle : StateMachineBehaviour
{
    Monster mob;
    float Atk_num;
    float Timer = 0;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (mob == null)
            mob = animator.GetComponent<Monster>();

        Atk_num = Random.Range(1, 100);
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        Timer += Time.deltaTime;

        if(mob.DiSTANCE < 3f && Timer >=2f)
        {
            mob.nav.destination = mob.transform.position;
            Vector3 dir = Character.Player.transform.position - mob.transform.position;
            mob.transform.forward = dir;
            
            if (Atk_num <= 20)
            {
                
                animator.SetFloat("Atk_Num", 1);
                Timer = 0;
                animator.SetTrigger("Attack");
            }
            else
            {
                animator.SetFloat("Atk_Num", 0);
                Timer = 0;
                animator.SetTrigger("Attack");
            }
            
            
        }
        else if(mob.DiSTANCE >= 3f)
        {
            
            animator.SetBool("IsMove", true);
        }
        
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
