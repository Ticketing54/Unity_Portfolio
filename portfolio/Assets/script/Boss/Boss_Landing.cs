using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Landing : StateMachineBehaviour
{
    AudioSource audio;
    GameObject screamsound;
    
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (screamsound == null)
        {
            screamsound = new GameObject("DraonFly");
            screamsound.transform.SetParent(animator.gameObject.transform);
            audio = screamsound.AddComponent<AudioSource>();
            audio.clip = Resources.Load<AudioClip>("Sounds/DraonFly");
            audio.loop = true;
            audio.Play();      

        }
        
        audio.gameObject.SetActive(true);        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        audio.gameObject.SetActive(false);
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
