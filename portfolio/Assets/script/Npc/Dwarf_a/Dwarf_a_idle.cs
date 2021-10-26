﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dwarf_a_idle : StateMachineBehaviour
{
    Npc npc;
    bool QuestMark = false;
    


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (npc == null)
            npc = animator.GetComponent<Npc>();
        
        
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(npc.DISTANCE < 4f && Character.Player.Interaction_T == true && Character.Player.ClickObj == true)
        {            
            UIManager.uimanager.DialogControl();
            UIManager.uimanager.dialog.npc = npc;
            animator.SetBool("Dialog", true);
            return;
        }
        for (int I = 0; I < npc.quest_list.Count; I++)
        {
            if (npc.quest_list[I].Index == 0 && npc.quest_list[I].State == 0 )
            {                
                npc.QuestMarkerNum = 0;

            }
            //else if (npc.quest_list[I].Index == 0 && npc.quest_list[I].State == 1)
            //{
            //    npc.quest_list[I].State = 2;
            //    npc.QuestMarkerNum = 1;

            //}
            else
            {
                npc.QuestMarkerNum = 2;
            }


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
