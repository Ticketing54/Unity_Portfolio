using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class boss_Sleep : StateMachineBehaviour
{
    PlayableDirector director;
    GameObject cameraline;
    bool Camerain = false;  
    
    

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(director == null)
        {
            director = GameObject.FindGameObjectWithTag("Timeline").GetComponent<PlayableDirector>();
            cameraline = director.transform.Find("cameraline").gameObject;
            CameraManager.cameraManager.IsCharacter = false;
            CameraManager.cameraManager.gameObject.transform.forward = cameraline.transform.forward;
            CameraManager.cameraManager.gameObject.transform.SetParent(cameraline.transform);
            CameraManager.cameraManager.gameObject.transform.localPosition = Vector3.zero;
            

        }
            


        UIManager.uimanager.DialogControl();
        director.Play();
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {


        


        
            
        if (Camerain == false && director.time >=8)
        {
            Camerain = true;
            animator.SetBool("WakeUp", true);
            
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
