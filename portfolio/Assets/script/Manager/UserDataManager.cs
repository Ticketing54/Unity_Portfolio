using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UserDataManager :MonoBehaviour
{
    public static UserDataManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
    }
    public void SaveData(int _num)
    {        
        PlayerPrefs.SetString(_num.ToString(), GameManager.gameManager.character.GetChracterInfo());
    }
    public string LoadData(int _num)
    {
        
       if(PlayerPrefs.GetString(_num.ToString()) != null && PlayerPrefs.GetString(_num.ToString()) !="")
       {
            return PlayerPrefs.GetString(_num.ToString());
       }
       return null;
    }
 
}
