using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Potal : MonoBehaviour
{   
    public string mapName { get; set; }
    public Vector3 pos { get; set; }


    public Image MiniMap_Dot_M;
    public float DISTANCE
    {
        get
        {
            if(GameManager.gameManager.character != null)
            {
                
                return Vector3.Distance(transform.position, GameManager.gameManager.character.transform.position);
            }
            else
            {
                return 100;
            }   
        }
    }
}
