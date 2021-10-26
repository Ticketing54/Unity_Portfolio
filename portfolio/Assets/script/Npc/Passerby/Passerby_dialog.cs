using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passerby_dialog : StateMachineBehaviour
{
    Npc npc;
    Dialogue dialog;
    public bool dialog_Texting = false;
    float timer = 0;
    bool meet = false;
    Vector3 dir = Vector3.zero;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (npc == null)
            npc = animator.GetComponent<Npc>();
        if (dialog == null)
            dialog = UIManager.uimanager.dialog;

        dialog_Texting = false;
        timer = 0;
        animator.SetFloat("Blend", 0);


        dir = Character.Player.transform.position - npc.transform.position;
        
        

    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if(animator.GetFloat("Blend") >= 1)
        {
            Vector3 tmp = Vector3.RotateTowards(npc.transform.forward, dir.normalized, Time.deltaTime*5f, Time.deltaTime*5f);
            npc.transform.rotation = Quaternion.LookRotation(tmp);
        }

        
        if (timer >= 1.8)
        {
            animator.SetFloat("Blend", 1);

            
        }


        if (dialog_Texting == false && dialog.gameObject.activeSelf == true)
        {
            dialog_Texting = true;
            




            for (int I = 0; I < npc.quest_list.Count; I++)
            {
                //if (npc.quest_list[I].Index == 4 && npc.quest_list[I].State == 3)
                //{
                //    dialog.Npc_Texting(19);
                //    return;

                //}
                //if (npc.quest_list[I].Index == 4 && npc.quest_list[I].State == 1)
                //{                    
                //    dialog.Npc_Texting(15);                    
                //    return;
                //}                
                //if (npc.quest_list[I].Index == 4 && npc.quest_list[I].State == 2)
                //{
                //    npc.quest_list[I].State = 3;
                //    npc.QuestMarkerNum = -1;
                //    dialog.Npc_Texting(16);
                //    QuestManager.questManager.DestroyQuestSLot(4);
                //    QuestManager.questManager.QuestComplete_QuestList(4);
                //    return;

                //}
                

            }



            if(meet == true)
            {
                dialog.Npc_Texting(2);
                return;
            }




            dialog.Npc_Texting(npc.Dialog_num);
            meet = true;

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
