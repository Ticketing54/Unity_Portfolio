using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Potal : MonoBehaviour
{
    MAPTYPE mapType;
    string mapName;
    Vector3 pos;
    string cutScene;
    public string MapName { get => mapName; }
    
    public  Vector3 Pos { get => pos; }
    
    public string CutScene { get => cutScene; }
    public MAPTYPE MapType { get => mapType; }  
    public void SettingPotal(string[] _info)
    {
        mapName = _info[0];
        
        MAPTYPE _type;
        if(Enum.TryParse(_info[1],out _type))
        {
            mapType = _type;
        }
        else
        {
            Debug.LogError("mapType Error");
        }
        transform.position = new Vector3(float.Parse(_info[2]), float.Parse(_info[3]), float.Parse(_info[4]));
        pos = new Vector3(float.Parse(_info[5]), float.Parse(_info[6]), float.Parse(_info[7]));

        if (!string.IsNullOrEmpty(_info[8]))
        {
            cutScene = _info[8];
        }
    }
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
