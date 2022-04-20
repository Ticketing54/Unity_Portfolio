using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charater_Combat : StateMachineBehaviour
{
    Character Player;
    bool Attack = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        Attack = false;
        animator.SetFloat("atkNum", 0);        
        animator.SetBool("SkillDone", false);        
        
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {        

        //if (Player.ClickObj == true && Player.Interaction_B == true && Attack == false)
        //{
        //    if (Character.Player.Target != null && Character.Player.Target.tag != "Monster")
        //        return;
        //    Attack = true;
        //    animator.SetTrigger("Attack");
        //    return;
        //}
       

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
