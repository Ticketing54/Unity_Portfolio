using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_R_Move : StateMachineBehaviour
{
    Monster mob;
    Vector3 tmpPos;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (mob == null)
            mob = animator.GetComponent<Monster>();
        tmpPos = mob.startPos;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (mob.isTarget == false)
        {
            mob.nav.destination = tmpPos;
            if (mob.transform.position == mob.nav.destination)
            {
                animator.SetBool("IsMove", false);
            }
        }
        else
        {
            if (mob.DiSTANCE < 3f)
            {
                animator.SetBool("IsMove", false);
            }
            else if (mob.DiSTANCE >= 2f && mob.DiSTANCE < 10f)
            {
                mob.nav.destination = Character.Player.transform.position;

            }
            else if (mob.DiSTANCE >= 10f)
            {
                mob.nav.destination = mob.startPos;
            }
        }
        if (mob.transform.position == mob.nav.destination)
        {
            mob.isTarget = false;
            animator.SetBool("IsMove", false);
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
