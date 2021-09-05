using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Slot : MonoBehaviour
{
    public Item item = null;   
    public Image itemImage;    
    public Text itemCount;


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
    

  


    
    public void AddItem(Item _item)
    {
        item = _item;
        itemImage.gameObject.SetActive(true);        
        itemImage.sprite = GameManager.gameManager.GetSprite(_item.itemSpriteName);            
        SetSlotCount();
        
    }
    public void SlotClear()
    {
        this.item= null;
        itemImage.sprite = null;
        itemCount.text = string.Empty;
        itemImage.gameObject.SetActive(false);
        itemCount.gameObject.SetActive(false);
        
    }

    
 
    // 아이템 쿨타임
  

    public void UseItem()
    {
        if(item.itemType == Item.ItemType.Used)
        {
            
            
            
            string[] Data = item.ItemProperty.Split('/');
            if (Data[0] == "Hp")
            {
                if (Character.Player.isrecovery_Hp == true || Character.Player.returnHp()<=Character.Player.Hp_C)
                    return;
                item.ItemCount -= 1;
                
                SetSlotCount();
                StartCoroutine(buf_character(Data[0],float.Parse(Data[1]),float.Parse(Data[2]), float.Parse(Data[3])));
            }
            else if (Data[0] == "Mp")
            {
                if (Character.Player.isrecovery_Mp == true || Character.Player.returnMp() <= Character.Player.Mp_C)
                    return;
                item.ItemCount -= 1;
                SetSlotCount();
                StartCoroutine(buf_character(Data[0],float.Parse(Data[1]), float.Parse(Data[2]), float.Parse(Data[3])));
            }
            



        }
    }
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
                    Character.Player.Hp_C += _buf_num;
                    if (Character.Player.Hp_C >= Character.Player.returnHp())
                        Character.Player.Hp_C = Character.Player.returnHp();



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
                    Character.Player.Mp_C += _buf_num;
                    if (Character.Player.Mp_C >= Character.Player.returnMp())
                        Character.Player.Mp_C = Character.Player.returnMp();



                    yield return null;





                }

                yield return null;
            }

           

        }








       
    }
   
    // 아이템 개수 조정
    public void SetSlotCount()
    {


        if (item.ItemCount > 1)
        {
            itemCount.gameObject.SetActive(true);
            itemCount.text = item.ItemCount.ToString();
        }

        if(item.ItemCount == 1)
        {
            
            itemCount.gameObject.SetActive(false);
            
        }
            

        if (item.ItemCount <= 0)
            SlotClear();
    }

   
   
}
