using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_R_idle : StateMachineBehaviour
{
    Monster mob;
    int attacknum = 0;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (mob == null)
            mob = animator.GetComponent<Monster>();
        attacknum = Random.Range(0, 100);
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (mob.DiSTANCE < 2f)
        {
            mob.nav.destination = mob.transform.position;
            Vector3 dir = Character.Player.transform.position - mob.transform.position;
            mob.transform.forward = dir;            
            if (attacknum < 80)
            {
                animator.SetFloat("Attack_num", 1);
            }
            else
            {
                animator.SetFloat("Attack_num", 0);
            }
            animator.SetTrigger("Attack");

        }
        else if (mob.DiSTANCE >= 2f && mob.DiSTANCE < 10f)
        {
            mob.isTarget = true;
            animator.SetBool("IsMove", true);
        }
        else if (mob.DiSTANCE >= 10f)
        {
            mob.isTarget = false;
            mob.nav.SetDestination(mob.transform.position);
        }






    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
