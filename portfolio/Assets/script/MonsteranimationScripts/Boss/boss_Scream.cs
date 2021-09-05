using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_Scream : StateMachineBehaviour
{
    Monster mob;
    float Timer = 0;
    GameObject IceField;
    bool Damage = false;

    AudioSource audio;
    GameObject screamsound;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (mob == null)
            mob = animator.GetComponent<Monster>();
        if(IceField == null)
        {
            IceField = ObjectPoolManager.objManager.EffectPooling("IceField");
            IceField.transform.position = mob.transform.position;
            IceField.gameObject.SetActive(false);
        }

        if(screamsound == null)
        {
            screamsound = new GameObject("scream");
            screamsound.transform.SetParent(mob.transform);
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

        if(Timer >= 0.7f)
            audio.gameObject.SetActive(true);
        if (Timer >= 1f)
        {
            IceField.gameObject.SetActive(true);
            IceField.gameObject.transform.position = mob.transform.position;
            

            if (mob.DiSTANCE >7f && mob.DiSTANCE < 10f &&Damage == false)
            {
                Damage = true;
                mob.StartCoroutine(DamageOn());
                SoundManager.soundmanager.soundsPlay("Icestate", Character.Player.gameObject);                
                Character.Player.Hp_C -= mob.Atk;
                Character.Player.icestateOn();
                ObjectPoolManager.objManager.LoadDamage(Character.Player.gameObject, mob.Atk, Color.red, 1);


            }








        }
    }
   
    IEnumerator DamageOn()
    {
        yield return new WaitForSeconds(0.5f);
        Damage = false;
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Timer = 0;        
        audio.gameObject.SetActive(false);
        IceField.gameObject.SetActive(false);
        
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
