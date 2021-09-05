using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charater_Attack : StateMachineBehaviour
{
    Character Player;
    float AttackNum = 0;
    bool Attack = false;    

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player = Character.Player;
        Attack = true;
        AttackNum = animator.GetFloat("atkNum");
        
    }




    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Player.isMove)
        {
            AttackNum = 0;
            animator.SetBool("Move", true);
            return;            
        }
            
        
        if (Player.ClickObj == true && Player.Interaction_B == true && Player.isCombo == true && Attack == true)
        {
            AttackNum++;
            if (AttackNum > 1)
                AttackNum = 0;
            animator.SetFloat("atkNum", AttackNum);            
            Character.Player.isCombo = false;                        
            Attack = false;
            animator.SetTrigger("Attack");
            

        }
        
    }


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
