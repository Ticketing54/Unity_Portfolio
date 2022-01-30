using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Potal : MonoBehaviour
{
    float dis;
    bool isWarp = false;
    public string MapName;
    public Vector3 Pos;


    public Image MiniMap_Dot_M;
    float DISTANCE
    {
        get
        {
            if(Character.Player != null)
                dis = Vector3.Distance(transform.position, Character.Player.transform.position);

            return dis;
                
        }
    }
    private void Start()
    {
        StartCoroutine(Mini_Dot_MMove());
    }


    IEnumerator Mini_Dot_MMove()
    {
        while (true)
        {
            if (UIManager.uimanager.minimap.MiniMap_MActive == true && Character.Player != null)
            {
                if (MiniMap_Dot_M == null)
                {
                    MiniMap_Dot_M = ObjectPoolManager.objManager.PoolingMiniDot_M();
                    MiniMap_Dot_M.sprite = GameManager.gameManager.resource.GetImage("Dot_P");
                }
                else
                {
                    MiniMap_Dot_M.rectTransform.anchoredPosition = UIManager.uimanager.minimap.MoveDotPosition(transform.position, 700);
                }

            }
            else
            {
                if (MiniMap_Dot_M != null)
                {
                    MiniMap_Dot_M.gameObject.SetActive(false);
                    MiniMap_Dot_M = null;
                }

            }

            yield return null;
        }
    }
















    void Update()
    {
        if (DISTANCE < 1 && isWarp == false && Character.Player !=null)
        {
            isWarp = true;            
            GameManager.gameManager.MapName = MapName;
            Character.Player.nav.SetDestination(Character.Player.transform.position);
            Character.Player.nav.Warp(Pos);
            LoadingSceneController.instance.LoadScene(MapName);
        }
            

        
    }

}
