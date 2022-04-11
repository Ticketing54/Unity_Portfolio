using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_flybreath : StateMachineBehaviour
{
    Monster mob;
    GameObject breath;
    GameObject Gunfire;
    SkinnedMeshRenderer breathrange;
    Vector3 dir = Vector3.zero;
    float vr = 1;
    float timer = 0;
    float Damage_count = 0;
    bool Damage = false;





    AudioSource audio;
    GameObject screamsound;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (mob == null)
        {
            mob = animator.GetComponent<Monster>();
            breath = mob.gameObject.transform.Find("breath").gameObject;
            breathrange = breath.GetComponent<SkinnedMeshRenderer>();
            Gunfire = GameObject.FindGameObjectWithTag("MobWeapon");

        }
        if (screamsound == null)
        {
            screamsound = new GameObject("breath");
            screamsound.transform.SetParent(mob.transform);
            audio = screamsound.AddComponent<AudioSource>();
            audio.clip = Resources.Load<AudioClip>("Sounds/Breath");
            audio.loop = true;
            audio.Play();
            audio.gameObject.SetActive(false);
        }

    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if(timer >= 1f)
        {
            audio.gameObject.SetActive(true);
            breath.gameObject.SetActive(true);
            breath.gameObject.transform.position = Gunfire.transform.position;
            vr -= (float)Time.deltaTime * 0.4f;
            dir = BressDir(mob.gameObject, vr) - breath.transform.localPosition;
            breath.transform.rotation = Quaternion.LookRotation(dir.normalized);


            //if (breathrange.bounds.Intersects(Character.Player.Character_bounds.bounds) && Damage == false)
            //{
            //    Damage = true;
            //    mob.StartCoroutine(DamageOn());
            //    SoundManager.soundmanager.soundsPlay("Icestate", Character.Player.gameObject);
            //    Damage_count++;
            //    //Character.Player.Stat.HP-= mob.Atk;
            //    Character.Player.icestateOn();
            //    //ObjectPoolManager.objManager.LoadDamage(Character.Player.gameObject, mob.Atk, Color.red, 1);
            //}

        }





    }
    IEnumerator DamageOn()
    {
        yield return new WaitForSeconds(0.5f);
        Damage = false;
    }
    public Vector3 BressDir(GameObject tmp, float value)
    {
        Vector3 a, b, c, d, e;
        a = tmp.transform.right * -10f;
        b = a + tmp.transform.forward * 10f;
        c = tmp.transform.right * 10f;
        d = c + tmp.transform.forward * 10f;

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
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        audio.gameObject.SetActive(false);
        breath.gameObject.SetActive(false);

        vr = 1;
        timer = 0;
        Damage_count = 0;

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
