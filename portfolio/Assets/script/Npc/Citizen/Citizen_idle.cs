using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citizen_idle : StateMachineBehaviour
{
    Npc npc;
  

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (npc == null)
            npc = animator.GetComponent<Npc>();
        npc.dialog_Done = true;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (npc.DISTANCE < 4f && Character.Player.Interaction_T == true && Character.Player.ClickObj == true)
        {

            animator.SetBool("Dialog", true);
            return;
        }

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
