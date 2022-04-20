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
    public Monster mob = null;
    public Npc npc = null;
    public Potal potal = null;
    public string Character_Name = string.Empty;

    List<Monster> mob_list = new List<Monster>();        


    

    public delegate void ReadyToStart();
    public ReadyToStart readyToStart;   

    
    //정보    
    public string MapName = string.Empty;    
  
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
    }

    

    public void LoadGame(int _index)    // 저장된 데이터 로드
    {       
        
        
        Load_C_Data(_index);                                                   
        //UIManager.uimanager.minimap.MapSetting();   //미니맵 변경        
        

    }
    public void NewGame(string _nickName)       // 새로운 게임시작 
    {   
        LoadingSceneController.Instance.LoadScene("Village");
        New_C_Data(_nickName);
    }
   
    public void MainMenu()
    {   
//        ObjectPoolManager.objManager.PoolingReset_Load();        
    }
    public void New_C_Data(string _nickName)
    {
        if(character == null)
        {
            GameObject obj = Instantiate(ResourceManager.resource.character);
            obj.transform.SetParent(this.transform);
            character = obj.AddComponent<Character>();

        }

        
        Status NewStat = new Status("New",1, 0, 0, 0, 0);       

        NewStat.LevelSetting(1);
        character.stat = NewStat;
        character.tag = "Player";
        character.gameObject.layer = 8;        
        character.name = "Player";        
        Character_Name = string.Empty;
        MapName = "Village";        
        character.inven = new Inventory();
        character.inven.gold = 5000;


        // Test


        Item test1 = new Item(1,1);
        Item test2 = new Item(1,3);
        Item test3 = new Item(1,2);
        Item test4 = new Item(2,1);
        Item test5 = new Item(2,2);
        Item test6 = new Item(3,1);
        character.inven.PushItem(test1);
        character.inven.PushItem(test2);
        character.inven.PushItem(test3);
        character.inven.PushItem(test4);
        character.inven.PushItem(test5);
        character.inven.PushItem(test6);







        
        character.quest = new CharacterQuest();
        Equipment newEquip = new Equipment(NewStat);
        character.equip = newEquip;
        character.transform.position = new Vector3(31f,0f,17f);        
    }
    public void Load_C_Data(int _num)
    {           
        string temp = UserDataManager.instance.LoadData(_num);
        if (temp == string.Empty)
        {
            MainManager.mainManager.NewGame();
        }
            
        
        string[] DATA = temp.Split('\n');
        GameObject obj = Instantiate(ResourceManager.resource.Instantiate("Character"));        
        character = obj.AddComponent<Character>();        
        character.tag = "Player";
        character.gameObject.layer = 8;
        string []info = DATA[0].Split(',');
        character.name = info[0];
        Status NewStat = new Status(info[0], int.Parse(info[5]), float.Parse(info[6]), float.Parse(info[7]), int.Parse(info[8]), int.Parse(info[9]));
        character.stat = NewStat;
        ChangeLayerObj(character.gameObject.transform, 8);
        MapName = info[1];        
        Inventory NewInven = new Inventory();
        character.inven = NewInven;        
        Equipment newEquip = new Equipment(NewStat);
        character.equip = newEquip;

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
