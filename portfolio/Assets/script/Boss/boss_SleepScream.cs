using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class boss_SleepScream : StateMachineBehaviour
{
    PlayableDirector director;

    
    float Timer = 0;
    AudioSource audio;
    GameObject screamsound;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (director == null)
            director = GameObject.FindGameObjectWithTag("Timeline").GetComponent<PlayableDirector>();


        if (screamsound == null)
        {
            screamsound = new GameObject("scream");
            screamsound.transform.SetParent(animator.gameObject.transform);
            audio = screamsound.AddComponent<AudioSource>();            
            audio.clip = Resources.Load<AudioClip>("Sounds/Shout");
            audio.loop = true;
            audio.Play();
            audio.gameObject.SetActive(false);
            
        }
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        Timer += Time.deltaTime;
        if (Timer >= 0.7f)
        {
            audio.gameObject.SetActive(true);


        }
    }



    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        audio.gameObject.SetActive(false);
        //UIManager.uimanager.DialogControl();
        
        CameraManager.cameraManager.gameObject.transform.parent = null;
        director.Stop();

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
