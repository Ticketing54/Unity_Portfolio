using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arthur_dialog : StateMachineBehaviour
{
    Npc npc;
    Dialogue dialog;
    public bool dialog_Texting = false;
    float timer = 0;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (npc == null)
            npc = animator.GetComponent<Npc>();
        if (dialog == null)
            dialog = UIManager.uimanager.dialog;

        dialog_Texting = false;
        timer = 0;
        animator.SetFloat("Blend", 0);
        


    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if(timer >= 1.8)
        {
            animator.SetFloat("Blend", 1);
        }
            

        if (dialog_Texting == false && dialog.gameObject.activeSelf == true)
        {
            dialog_Texting = true;




            //for (int I = 0; I < npc.quest_list.Count; I++)
            //{
            //    if (npc.quest_list[I].Index == 0 && npc.quest_list[I].State == 2)
            //    {
            //        npc.QuestMarkerNum = -1;
            //        npc.quest_list[I].State = 3;
            //        QuestManager.questManager.FindNpc(2).QuestMarkerNum = -1;
            //        QuestManager.questManager.QuestCompletenum(QuestManager.questManager.FindNpc(2), 0, 3);
            //        dialog.Npc_Texting(2);
            //        QuestManager.questManager.DestroyQuestSLot(0);
            //        QuestManager.questManager.QuestComplete_QuestList(0);
            //        return;
            //    }
            //    if (npc.quest_list[I].Index == 1 && npc.quest_list[I].State == 1)
            //    {                    
            //        dialog.Npc_Texting(6);                    
            //        return;
            //    }
            //    if (npc.quest_list[I].Index == 1 && npc.quest_list[I].State == 2)
            //    {
            //        npc.quest_list[I].State = 3;
            //        npc.QuestMarkerNum = -1;
            //        Character.Player.Stat.HP -= 50;
            //        dialog.Npc_Texting(7);                    
            //        QuestManager.questManager.DestroyQuestSLot(1);
            //        QuestManager.questManager.QuestComplete_QuestList(1);
            //        return;
            //    }
            //    if (npc.quest_list[I].Index == 2 && npc.quest_list[I].State == 1)
            //    {
            //        dialog.Npc_Texting(10);
            //        return;
            //    }
            //    if (npc.quest_list[I].Index == 2 && npc.quest_list[I].State == 2)
            //    {
            //        npc.quest_list[I].State = 3;
            //        npc.QuestMarkerNum = -1;
            //        dialog.Npc_Texting(12);
            //        QuestManager.questManager.DestroyQuestSLot(2);
            //        QuestManager.questManager.QuestComplete_QuestList(2);
            //        return;
                  
            //    }                
            //    if (npc.quest_list[I].Index == 2 && npc.quest_list[I].State == 3)
            //    {
            //        dialog.Npc_Texting(3);
            //        return;

            //    }
                
            //}








            dialog.Npc_Texting(npc.Dialog_num);


        }
        if (Input.GetMouseButtonDown(0) && npc.dialog_Done == true)
        {
            npc.dialog_Done = false;
            dialog.Npc_Texting(npc.NextDialog);

        }

        if (npc.dialog_Done == true && Input.GetKeyDown(KeyCode.Escape) || npc.ExitDialog == true)  // 나가기 버튼 // 대화가 끝났을때 
        {
            npc.ExitDialog = false;
            UIManager.uimanager.CloseDialog(npc);
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
