using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Attack : StateMachineBehaviour
{
    Character character;
    GameObject breath;
    GameObject Gunfire; // 입    
    SkinnedMeshRenderer breathrange;
    TutorialBoss boss;
    float atk_num = 0;    
    
    float timer = 0;
    float vr;

    bool Damage = false;
    Vector3 dir = Vector3.zero;



    AudioSource audio;
    GameObject screamsound;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        atk_num = animator.GetFloat("PhaseNumber");
        character = GameManager.gameManager.character;
        if (boss == null)
        {
            boss = animator.GetComponent<TutorialBoss>();            
            breath = boss.gameObject.transform.Find("breath").gameObject;
            breathrange = breath.GetComponent<SkinnedMeshRenderer>();
            Gunfire = GameObject.FindGameObjectWithTag("MobWeapon");
        }
        if (screamsound == null)
        {
            screamsound = new GameObject("breath");
            screamsound.transform.SetParent(boss.transform);
            audio = screamsound.AddComponent<AudioSource>();
            audio.clip = ResourceManager.resource.GetSound("Breath");
            audio.loop = true;
            audio.Play();
            audio.gameObject.SetActive(false);
        }
      
        vr = 1;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        
        if (atk_num == 2)
        {
            timer += Time.deltaTime;

            if (timer >= 0.9f)
            {
                audio.gameObject.SetActive(true);
                breath.gameObject.SetActive(true);
                breath.gameObject.transform.position = Gunfire.transform.position;
                vr -= (float)Time.deltaTime * 0.3f;
                dir = BressDir(boss.gameObject, vr) - breath.transform.localPosition;
                breath.transform.rotation = Quaternion.LookRotation(dir.normalized);

                if (breathrange.bounds.Intersects(character.AttackBox) && Damage == false)
                {
                    Damage = true;
                    boss.StartCoroutine(DamageOn(0.5f));
                    SoundManager.soundmanager.soundsPlay("Icestate", character.gameObject);
                    character.stat.Damaged(false, boss.AttackDmg(false));                    
                }
            }
        }
        



        



    }

    IEnumerator DamageOn(float _time)
    {
        yield return new WaitForSeconds(_time);
        Damage = false;
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if (breath.gameObject.activeSelf == true)
            breath.gameObject.SetActive(false);
        audio.gameObject.SetActive(false);
        timer = 0;
        vr = 1;                
    }

    public Vector3 BressDir(GameObject tmp, float value)
    {
        Vector3 a, b, c, d, e;
        a = tmp.transform.right * -20f + tmp.transform.up *5f;
        b = a + tmp.transform.forward * 20f;
        c = tmp.transform.right * 20f + tmp.transform.up * 5f;
        d = c + tmp.transform.forward * 20f;

        e = BezierTest(a, b, d, c, value);
        return e;

    }

    public Vector3 BezierTest(Vector3 P_1, Vector3 P_2, Vector3 P_3, Vector3 P_4, float value)
    {
        Vector3 A = Vector3.Lerp(P_1, P_2, value);
        Vector3 B = Vector3.Lerp(P_2, P_3, value);
        Vector3 C = Vector3.Lerp(P_3, P_4, value);

        Vector3 D = Vector3.Lerp(A, B, value);
        Vector3 E = Vector3.Lerp(B, C, value);

        Vector3 F = Vector3.Lerp(D, E, value);

        return F;
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
