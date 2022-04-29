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
            if(GameManager.gameManager.character != null)
                dis = Vector3.Distance(transform.position, GameManager.gameManager.character.transform.position);

            return dis;
                
        }
    }
   
















    void Update()
    {
        if (DISTANCE < 1 && isWarp == false && GameManager.gameManager.character !=null)
        {
            isWarp = true;            
            GameManager.gameManager.MapName = MapName;            
            LoadingSceneController.instance.LoadScene(MapName);
        }
            

        
    }

}
