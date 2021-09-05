using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Durmmy_Damage : StateMachineBehaviour
{
    Quest quest;
    Npc npc;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (quest == null)
        {
            foreach (Npc tmp in Character.Player.npcList)
            {
                if (tmp.index == 1)
                {
                    npc = tmp;
                    foreach (Quest one in tmp.quest_list)
                    {
                        if (one.Index == 1)
                            quest = one;
                    }
                }

            }
        }

    }



    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (quest != null && quest.QuestComplete == 1)
        {
            quest.QuestComplete = 2;
            npc.QuestMarkerNum = 2;
            QuestManager.questManager.FindMiniQuestSlot(quest.Index).finishQuest();

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
