using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MiniMap : MonoBehaviour
{    
    
    
    public Image minibutton;








    //좌 상단 미니맵
    public GameObject Minimap_n;    
    public ScrollRect MapImage;
    float MapSize_X = 0;
    float MapSize_Y = 0;
    public Image MapSprite_n;
    public Image MapSprite_M;
    public Vector2 Pos = Vector2.zero;
    public Image PlayerDot;
    

    //중앙 미니맵
    public GameObject Minimap_M;
    public CanvasGroup MiniMap_Fade;
    public Image Player_Dot;



    //미니맵들 상태관리
    public bool MiniMap_nActive = true;
    public bool MiniMap_MActive = false;

    public void MapSetting()        //맵 바꿀때
    {
        MapSize_X = GameObject.FindGameObjectWithTag("Floor").transform.localScale.x *10;
        MapSize_Y = GameObject.FindGameObjectWithTag("Floor").transform.localScale.z *10;              


        if (MiniMap_nActive == false)
        {
            miniMap_n_Max_Min();
            MapSprite_n.sprite = GameManager.gameManager.GetSprite(GameManager.gameManager.MapName);
        }
        else
        {
            MapSprite_n.sprite = GameManager.gameManager.GetSprite(GameManager.gameManager.MapName);
        }

        if(MiniMap_MActive == false)
        {
            miniMap_M_Max_Min();
            MapSprite_M.sprite = GameManager.gameManager.GetSprite(GameManager.gameManager.MapName);
            miniMap_M_Max_Min();
        }
        else
        {
            MapSprite_M.sprite = GameManager.gameManager.GetSprite(GameManager.gameManager.MapName);
        }
        

    }
    public void MovePosition()      //좌상단 캐릭터의 위치
    {
        float x = Character.Player.transform.position.x / MapSize_X;
        float y = Character.Player.transform.position.z/ MapSize_Y;

        
        Pos.Set(x, y);
        MapImage.normalizedPosition = Pos;
        


    }
    public void MovePosition_M()      //중앙 미니맵 위치
    {
        Player_Dot.rectTransform.anchoredPosition = MoveDotPosition(Character.Player.transform.position,700f);

    }
    public Vector2 MoveDotPosition(Vector3 Pos, float Measure=900)     //좌상단 미니맵상 위치
    {
        Vector2 End = Vector2.zero;
        float x = (Pos.x / MapSize_X)*Measure;
        float y = (Pos.z / MapSize_Y)* Measure;
        End.Set(x, y);
        return End;

    }
    public void miniMap_n_Max_Min()       //미니맵 최소화 최대화
    {
        MiniMap_nActive = !MiniMap_nActive;


        if (MiniMap_nActive)
        {
            Minimap_n.SetActive(true);
            minibutton.gameObject.SetActive(true);
        }
        else 
        {
            Minimap_n.SetActive(false);
            minibutton.gameObject.SetActive(false);

        }     
    }
    public void miniMap_M_Max_Min()       //미니맵 최소화 최대화
    {
        MiniMap_MActive = !MiniMap_MActive;


        if (MiniMap_MActive)
        {            
            StartCoroutine(MinimFadein());
        }
        else
        {
            StartCoroutine(MinimFadeout());
        }
        
    }
    IEnumerator MinimFadein()
    {
        Minimap_M.SetActive(true);
        while (MiniMap_Fade.alpha < 1)
        {
            if (MiniMap_MActive == false)
                yield break;

            MiniMap_Fade.alpha += Time.deltaTime * 4f;            


            yield return null;

        }
    }
    IEnumerator MinimFadeout()
    {
        while (MiniMap_Fade.alpha > 0)
        {
            if (MiniMap_MActive == true)
                yield break;

            MiniMap_Fade.alpha -= Time.deltaTime * 4f;
            if(MiniMap_Fade.alpha <= 0)
            {
                Minimap_M.SetActive(false);
                yield break;
            }
                

            yield return null;

        }
        
    }

    void Update()
    {
        if(Minimap_n.activeSelf == true)
        {
            MovePosition();
            float dir = Camera.main.transform.rotation.eulerAngles.y;
            PlayerDot.rectTransform.rotation = Quaternion.Euler(0, 0, -dir);
            
        }
            
        if (Minimap_M.activeSelf == true)
            MovePosition_M();
    }
}
