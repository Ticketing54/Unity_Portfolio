using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citizen_dialog : StateMachineBehaviour
{
    Npc npc;         

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (npc == null)
            npc = animator.GetComponent<Npc>();

        if(npc.NpcTalk == null)
        {
            npc.NpcTalk = ObjectPoolManager.objManager.PoolingNickName();
            
        }
        npc.npctalk((int)Random.Range(0, 4));
            
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {       
        if(npc.dialog_Done == true)
        {
            npc.NpcTalk.text = "";
            animator.SetBool("Dialog", false);
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
