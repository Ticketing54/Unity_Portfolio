using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Slot : MonoBehaviour
{   
    [SerializeField]
    protected Image icon;   
    public Sprite ICON { get { return icon.sprite; } set { icon.sprite = value; } }
    public RectTransform tr;
    Rect rc;
    public Rect RC
    {
        get
        {
            rc.x = tr.position.x - tr.rect.width * 0.5f;
            rc.y = tr.position.y + tr.rect.height * 0.5f;
            return rc;
        }
    }
    void Start()
    {
        rc.x = tr.transform.position.x - tr.rect.width / 2;
        rc.y = tr.transform.position.y + tr.rect.height / 2;
        rc.xMax = tr.rect.width;
        rc.yMax = tr.rect.height;
        rc.width = tr.rect.width;
        rc.height = tr.rect.height;       
    }    
    public bool isInRect(Vector2 pos)
    {
        if (pos.x >= RC.x &&pos.x <= RC.x + RC.width &&pos.y >= RC.y - RC.height &&pos.y <= RC.y)
        {            
            return true;
        }
        return false;
    }        
    public void ClickedSlot_Start()
    {
        Color alpacontrol = icon.color;
        alpacontrol.a = 0.25f;
        icon.color = alpacontrol;
    }
    public void ClickedSlot_End()
    {
        Color alpacontrol = icon.color;
        alpacontrol.a = 1;
        icon.color = alpacontrol;
    }
    public bool isEmpty()
    {
        return ICON == null;
    }
    public virtual void Clear()
    {
        icon.sprite = null;
        icon.gameObject.SetActive(false);
    }
    public virtual void Add(string _SpriteName)
    {
        if (_SpriteName == string.Empty)
        {
            Clear();
        }
        icon.gameObject.SetActive(true);
        icon.sprite = ResourceManager.resource.GetImage(_SpriteName);
    }
    //IEnumerator buf_character(string _bufimagename,float buf_num_max, float _buf_num, float Max_Time) //버프이미지 // 총 버프하는 양 // 초당 버프하는 양// 총 버프되는시간
    //{
        
    //    //float Maxbuf = buf_num_max; //총 버프양
    //    //float buf_s = _buf_num; //초당 버프되는양
    //    //float buf_timer = 0f; // 타이머
    //    //float MaxTime = Max_Time;
    //    //float cooltime = 0;
    //    ////bufimage bufimage = ObjectPoolManager.objManager.PoolingbufControl();  // 버프이미지
    //    ////GameObject bufEffect = ObjectPoolManager.objManager.EffectPooling(_bufimagename);   //버프 이펙트     
    //    ////bufimage.bufsprite.sprite = ResourceManager.resource.GetImage(_bufimagename);
        

    //    //if(_bufimagename == "Hp")
    //    //{
    //    //    Character.Player.isrecovery_Hp = true;
    //    //    while (true)
    //    //    {
    //    //        if (Maxbuf <= 0)
    //    //        {
                    
                    
    //    //            bufimage.gameObject.SetActive(false);
    //    //            bufEffect.SetActive(false);
    //    //            Character.Player.isrecovery_Hp = false;
    //    //            yield break;
    //    //        }

    //    //        cooltime += Time.deltaTime; // 남은시간 
    //    //        bufimage.cooltime_image.fillAmount = (cooltime/MaxTime);

    //    //        bufEffect.transform.position = Character.Player.transform.position;


    //    //        buf_timer += Time.deltaTime;  // 1초당 한번
    //    //        if (buf_timer >= 1f)
    //    //        {
    //    //            buf_timer -= 1f;

                    


    //    //            Maxbuf -= _buf_num;
    //    //            Character.Player.STAT.HP += _buf_num;
    //    //            if (Character.Player.STAT.HP >= Character.Player.STAT.MAXHP)
    //    //                Character.Player.STAT.HP = Character.Player.STAT.MAXHP;



    //    //            yield return null;





    //    //        }

    //    //        yield return null;
    //    //    }

    //    //}
    //    //else if(_bufimagename == "Mp")
    //    //{
    //    //    Character.Player.isrecovery_Mp = true;
    //    //    while (true)
    //    //    {
    //    //        if (Maxbuf <= 0)
    //    //        {
                    
    //    //            bufimage.gameObject.SetActive(false);
    //    //            bufEffect.SetActive(false);
    //    //            Character.Player.isrecovery_Mp = false;
    //    //            yield break;
    //    //        }

    //    //        cooltime += Time.deltaTime; // 남은시간 
    //    //        bufimage.cooltime_image.fillAmount = (cooltime / MaxTime);

    //    //        bufEffect.transform.position = Character.Player.transform.position;


    //    //        buf_timer += Time.deltaTime;  // 1초당 한번
    //    //        if (buf_timer >= 1f)
    //    //        {
    //    //            buf_timer -= 1f;




    //    //            Maxbuf -= _buf_num;
    //    //            Character.Player.STAT.MP += _buf_num;
    //    //            if (Character.Player.STAT.MP >= Character.Player.STAT.MAXMP)
    //    //                Character.Player.STAT.MP = Character.Player.STAT.MAXMP;



    //    //            yield return null;





    //    //        }

    //    //        yield return null;
    //    //    }

           

    //    //}








       
    //}
   
    //// 아이템 개수 조정
    

   
   
}
