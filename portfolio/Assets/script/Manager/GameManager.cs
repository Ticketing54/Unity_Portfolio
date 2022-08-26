using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;


public class GameManager : MonoBehaviour 
{
    public static GameManager gameManager;
    public int Player_num { get; set; }
   
    
    public Character character = null;

    //맵 정보    
    #region CurrentMapInfo
    public string mapName { get; set; }
    public float mapSizeX { get; set; }
    public float mapSizeY { get; set; }

    Dictionary<string, Vector3> wayPoint = new();
    void resetWayPoint()
    {
        wayPoint.Clear();
    }

    public void ClearWayPoint()
    {
        wayPoint.Clear();
    }
    public void AddWayPoint(string _key, Vector3 _pos)
    {
        if (wayPoint.ContainsKey(_key))
        {
            Debug.LogError("이미 있는 키를 더할려고 합니다.");
        }
        else
        {
            wayPoint.Add(_key, _pos);
        }
    }

    #endregion
    public void SetMapInfo(string _mapName)
    {
        mapName = _mapName;
        mapSizeX = GameObject.FindGameObjectWithTag("Floor").transform.position.x * 2;
        mapSizeY= GameObject.FindGameObjectWithTag("Floor").transform.position.z * 2;
    }

    
    private void Awake()
    {        
        if (gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        moveSceneReset += resetWayPoint;
    }

    

    public void LoadGame(int _index)    // 저장된 데이터 로드
    {
        if(character != null)
        {
            Destroy(character);
            character = null;
        }
        character = Instantiate(ResourceManager.resource.character).AddComponent<Character>();
        character.transform.SetParent(this.transform);
        Load_C_Data(_index);
        UIManager.uimanager.OnBaseUI();        


    }
    public void NewGame(string _nickName)       // 새로운 게임시작 
    {
        if (character != null)
        {
            Destroy(character);
            character = null;
        }
        character = Instantiate(ResourceManager.resource.character).AddComponent<Character>();
        character.transform.SetParent(this.transform);
        character.gameObject.layer = 8;
        New_C_Data(_nickName);        
        LoadingSceneController.Instance.LoadScene("Village",MAPTYPE.NOMAL);        
    }


    public delegate void MoveSceneReset();
    public MoveSceneReset moveSceneReset;
    public void MoveToScene(string _sceneName,Vector3 _pos,MAPTYPE _type,string _cutScene)
    {
        UIManager.uimanager.OffBaseUI();
        character.StartPos = _pos;
        moveSceneReset();
        LoadingSceneController.instance.LoadScene(_sceneName,_type,_cutScene);
    }
   
    public void New_C_Data(string _nickName)
    {

        character.stat.LevelSetting(5);
        character.tag = "Player";         
        character.name = _nickName;                
        mapName = "Village";                
        character.inven.Gold = 5000;
        character.StartPos = new Vector3(32f,0f,20f);

        // Test
       
        Item test1 = new (1,1);
        Item test2 = new (1,3);
        Item test3 = new (1,2);
        Item test4 = new (2,1);
        Item test5 = new (2,2);
        Item test6 = new (3,1);
        character.inven.PushItem(test1);
        character.inven.PushItem(test2);
        character.inven.PushItem(test3);
        character.inven.PushItem(test4);
        character.inven.PushItem(test5);
        character.inven.PushItem(test6);
        
        
        
    }
    

    public void Load_C_Data(int _num)
    {           
        string temp = UserDataManager.instance.LoadData(_num);
        if (temp == string.Empty)
        {
            MainManager.mainManager.NewGame();
        }
            
        
        string[] DATA = temp.Split('\n');
        GameObject obj = new();
        character = obj.AddComponent<Character>();        
        character.tag = "Player";
        character.gameObject.layer = 8;
        string []info = DATA[0].Split(',');
        character.name = info[0];  
        
        ChangeLayerObj(character.gameObject.transform, 8);
        mapName = info[1];        
        
        
              

        if(DATA[1] != "")
        {
            info = DATA[1].Split('/');            
            foreach (string one in info)    // 인벤
            {
                string[] sInven = one.Split(',');
                
                List<string> iteminfo = ResourceManager.resource.GetTable_Index("ItemTable", int.Parse(sInven[0]));
                //Item tmp = new Item(int.Parse(iteminfo[0]), int.Parse(iteminfo[1]), iteminfo[2], iteminfo[3], iteminfo[4], iteminfo[5], int.Parse(iteminfo[6]), int.Parse(iteminfo[7]));
                //tmp.SlotNum = int.Parse(sInven[1]);
                //tmp.ItemCount = int.Parse(sInven[2]);
                
                //character.inven.AddItem(int.Parse(sInven[1]), tmp);
            }
        }
        if (DATA[2] != "")
        {
            info = DATA[2].Split('/');      //장비
            foreach (string one in info)
            {
                string[] sInven = one.Split(',');
                List<string> iteminfo = ResourceManager.resource.GetTable_Index("ItemTable", int.Parse(sInven[0]));
                //Item tmp = new Item(int.Parse(iteminfo[0]), int.Parse(iteminfo[1]), iteminfo[2], iteminfo[3], iteminfo[4], iteminfo[5], int.Parse(iteminfo[6]), int.Parse(iteminfo[7]));
                //tmp.SlotNum = int.Parse(sInven[1]);
                //tmp.ItemCount = 1;
                //character.equip.AddItem((int)tmp.equipType,tmp);        
                    
            }
        }
        if (DATA[3] != "")
        {
            info = DATA[3].Split('/');      //퀵아이템
            foreach (string one in info)
            {
                string[] sInven = one.Split(',');
                List<string> iteminfo = ResourceManager.resource.GetTable_Index("ItemTable", int.Parse(sInven[0]));
                //Item tmp = new Item(int.Parse(iteminfo[0]), int.Parse(iteminfo[1]), iteminfo[2], iteminfo[3], iteminfo[4], iteminfo[5], int.Parse(iteminfo[6]), int.Parse(iteminfo[7]));
                //tmp.SlotNum = int.Parse(sInven[1]);
                //tmp.ItemCount = int.Parse(sInven[2]);                
                //character.QuickSlot.ExchangeItem(tmp.SlotNum, tmp);
            }
        }
       



    }
 
    public string NpcDialog(string _Name)
    {
        TextAsset temp = Resources.Load<TextAsset>("csv/Npc/" + _Name);
        if (temp == null)
            return null;

        return temp.text;
        
        
    } 
  
    public void ChangeLayerObj(Transform _Obj, int _Layer)
    {
        _Obj.gameObject.layer = _Layer;
        for(int i = 0; i < _Obj.childCount; i++)
        {
            Transform tmp = _Obj.GetChild(i);
            tmp.gameObject.layer = _Layer;
            ChangeLayerObj(tmp,_Layer);
        }

    }
   
              
}
