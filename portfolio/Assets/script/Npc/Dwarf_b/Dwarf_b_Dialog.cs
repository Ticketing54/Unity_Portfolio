using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dwarf_b_Dialog : StateMachineBehaviour
{
    Npc npc;
    Dialogue dialog;
    public bool dialog_Texting = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (npc == null)
            npc = animator.GetComponent<Npc>();
        

        dialog_Texting = false;



    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (dialog_Texting == false && dialog.gameObject.activeSelf == true)
        {
            dialog_Texting = true;
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
