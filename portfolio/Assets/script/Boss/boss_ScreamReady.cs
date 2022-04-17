using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_ScreamReady : StateMachineBehaviour
{
    Monster mob;
    float Timer = 0;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (mob == null)
            mob = animator.GetComponent<Monster>();
        
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Timer += Time.deltaTime;
        if(Timer >= 3f)
        {
            animator.SetBool("Scream", true);
        }
        
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("ScreamReady", false);
        animator.SetBool("Scream", false);
        Timer = 0;
        
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
