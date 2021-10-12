using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Slot : MonoBehaviour
{       
    protected Image Icon;        
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
    public bool ActiveIcon() { return Icon.gameObject.activeSelf; }
    public bool isInRect(Vector2 pos)
    {
        if (pos.x >= RC.x &&pos.x <= RC.x + RC.width &&pos.y >= RC.y - RC.height &&pos.y <= RC.y)
        {            
            return true;
        }
        return false;
    }    
    public virtual void Add(){}
    public virtual void Clear(){}

    //public void SetSlotCount()
    //{
    //    //if (item.ItemCount > 1)
    //    //{
    //    //    Count.gameObject.SetActive(true);
    //    //    Count.text = item.ItemCount.ToString();
    //    //}

    //    //if (item.ItemCount == 1)
    //    //{

    //    //    Count.gameObject.SetActive(false);

    //    //}


    //    //if (item.ItemCount <= 0)
    //    //    Clear();
    //}


    // 아이템 쿨타임



    IEnumerator buf_character(string _bufimagename,float buf_num_max, float _buf_num, float Max_Time) //버프이미지 // 총 버프하는 양 // 초당 버프하는 양// 총 버프되는시간
    {
        
        float Maxbuf = buf_num_max; //총 버프양
        float buf_s = _buf_num; //초당 버프되는양
        float buf_timer = 0f; // 타이머
        float MaxTime = Max_Time;
        float cooltime = 0;
        bufimage bufimage = ObjectPoolManager.objManager.PoolingbufControl();  // 버프이미지
        GameObject bufEffect = ObjectPoolManager.objManager.EffectPooling(_bufimagename);   //버프 이펙트     
        bufimage.bufsprite.sprite = GameManager.gameManager.ImageManager[_bufimagename];
        

        if(_bufimagename == "Hp")
        {
            Character.Player.isrecovery_Hp = true;
            while (true)
            {
                if (Maxbuf <= 0)
                {
                    
                    
                    bufimage.gameObject.SetActive(false);
                    bufEffect.SetActive(false);
                    Character.Player.isrecovery_Hp = false;
                    yield break;
                }

                cooltime += Time.deltaTime; // 남은시간 
                bufimage.cooltime_image.fillAmount = (cooltime/MaxTime);

                bufEffect.transform.position = Character.Player.transform.position;


                buf_timer += Time.deltaTime;  // 1초당 한번
                if (buf_timer >= 1f)
                {
                    buf_timer -= 1f;

                    


                    Maxbuf -= _buf_num;
                    Character.Player.Stat.HP += _buf_num;
                    if (Character.Player.Stat.HP >= Character.Player.Stat.MAXHP)
                        Character.Player.Stat.HP = Character.Player.Stat.MAXHP;



                    yield return null;





                }

                yield return null;
            }

        }
        else if(_bufimagename == "Mp")
        {
            Character.Player.isrecovery_Mp = true;
            while (true)
            {
                if (Maxbuf <= 0)
                {
                    
                    bufimage.gameObject.SetActive(false);
                    bufEffect.SetActive(false);
                    Character.Player.isrecovery_Mp = false;
                    yield break;
                }

                cooltime += Time.deltaTime; // 남은시간 
                bufimage.cooltime_image.fillAmount = (cooltime / MaxTime);

                bufEffect.transform.position = Character.Player.transform.position;


                buf_timer += Time.deltaTime;  // 1초당 한번
                if (buf_timer >= 1f)
                {
                    buf_timer -= 1f;




                    Maxbuf -= _buf_num;
                    Character.Player.Stat.MP += _buf_num;
                    if (Character.Player.Stat.MP >= Character.Player.Stat.MAXMP)
                        Character.Player.Stat.MP = Character.Player.Stat.MAXMP;



                    yield return null;





                }

                yield return null;
            }

           

        }








       
    }
   
    // 아이템 개수 조정
    

   
   
}
