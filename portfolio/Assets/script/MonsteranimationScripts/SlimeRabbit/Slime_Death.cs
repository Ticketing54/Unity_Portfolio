using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Death : StateMachineBehaviour
{
    Monster mob;
    bool OpenDropBox = false;
    DropBox dropBox;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (mob == null)
        {
            mob = animator.GetComponent<Monster>();
        }
        animator.SetBool("DropItem", true);
        OpenDropBox = false;
        mob.tag = "Item";
        if (mob.isQuestMob == true)
            QuestManager.questManager.questUpdate(QuestType.BATTLE, mob.Index);
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Character.Player.Interaction_L == true && OpenDropBox == false && Character.Player.ClickObj == true&&Character.Player.Target == mob.gameObject)
        {
            OpenDropBox = true;
            UIManager.uimanager.dropBox.gameObject.SetActive(true);
            dropBox = UIManager.uimanager.dropBox;
            //dropBox.AddItem(mob.Item);
            return;

        }


        if (OpenDropBox == true)
        {
            OpenDropBox = false;
            mob.tag = "Item";
            mob.ItemDropState();
            mob.gameObject.SetActive(false);
            animator.SetBool("DropItem", true);


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
