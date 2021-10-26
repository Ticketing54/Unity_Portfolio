using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic_Die : StateMachineBehaviour
{
    Monster mob;
    bool OpenDropBox = false;
    DropBox dropBox;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (mob == null)
            mob = animator.GetComponent<Monster>();


        animator.SetBool("DropItem", true);
        mob.tag = "Item";
        OpenDropBox = false;
        if (mob.isQuestMob == true)
            QuestManager.questManager.questUpdate(QuestType.BATTLE, mob.Index);



    }



    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(mob.DiSTANCE<2 &&OpenDropBox == false && Character.Player.ClickObj == true && Character.Player.Target == mob.gameObject)
        {
            OpenDropBox = true;
            UIManager.uimanager.dropBox.gameObject.SetActive(true);
            dropBox = UIManager.uimanager.dropBox;
            //dropBox.AddItem(mob.Item);
            return;

        }


        if(OpenDropBox == true)
        {
            OpenDropBox = false;
            mob.tag = "Item";
            mob.ItemDropState();
            mob.gameObject.SetActive(false);
            animator.SetBool("DropItem", true);


        }


    }

   
  
}
