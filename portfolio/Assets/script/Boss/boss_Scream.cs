using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_Scream : StateMachineBehaviour
{
    TutorialBoss boss;
    GameObject iceField;
    
    AudioSource audio;
    GameObject screamsound;
    Character character;

    float timer;
    bool isHit;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss == null)
            boss = animator.GetComponent<TutorialBoss>();

        if (iceField == null)
        {
            iceField = ResourceManager.resource.GetEffect("IceField");            
            iceField.transform.position = boss.transform.position;
            iceField.SetActive(false);
        }
        
        //if(screamsound == null)
        //{
        //    screamsound = new GameObject("scream");
        //    screamsound.transform.SetParent(boss.transform);
        //    audio = screamsound.AddComponent<AudioSource>();
        //    audio.clip = Resources.Load<AudioClip>("Sounds/Shout");
        //    audio.loop = true;
        //    audio.Play();
        //    audio.gameObject.SetActive(false);
        //}
        if (character == null)
        {
            character = GameManager.gameManager.character;
        }
        isHit = false;
        timer = 0f;
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        timer += Time.deltaTime;

        //if(timer >= 0.7f)
        //    audio.gameObject.SetActive(true);
        if (timer >= 1f)
        {
            if(iceField.gameObject.activeSelf == false)
            {
                iceField.gameObject.SetActive(true);
                iceField.gameObject.transform.position = boss.transform.position;
            }
            
            

            if (boss.DISTANCE > 8f && boss.DISTANCE < 12f && isHit == false)
            {
                isHit = true;
                boss.StartCoroutine(CoHitControl());
                Debug.Log("hit");
                //SoundManager.soundmanager.soundsPlay("Icestate", Character.Player.gameObject);
                //Character.Player.Stat.HP -= mob.Atk;
                //Character.Player.icestateOn();
                //ObjectPoolManager.objManager.LoadDamage(Character.Player.gameObject, mob.Atk, Color.red, 1);


            }








        }
    }
   
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        //audio.gameObject.SetActive(false);
        iceField.gameObject.SetActive(false);       
    }

    IEnumerator CoHitControl()
    {
        yield return new WaitForSeconds(1f);
        isHit = false;
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
