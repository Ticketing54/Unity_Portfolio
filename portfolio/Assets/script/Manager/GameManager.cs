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
    public ResourceManager resource = new ResourceManager();    


    public bool isCharacter = false;    //캐릭터 생성 여부
    
   

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

        StartResourcesd();
        
  


        
    }

    

    public void StartResourcesd()
    {
        resource.LoadGameObjectRes();                                
        resource.LoadImageResources();
        resource.LoadTable(TABLETYPE.ITEM, "csv/Table/ItemTable");
        resource.LoadTable(TABLETYPE.LEVEL, "csv/Table/LevelTable");
        resource.LoadTable(TABLETYPE.MONSTER, "csv/Table/MonsterTable");
        resource.LoadTable(TABLETYPE.SKILL, "csv/Table/SkillTable");
        resource.LoadTable(TABLETYPE.QUEST, "csv/Table/QuestTable");    

    }
    public void LoadGame(int _index)    // 저장된 데이터 로드
    {
        
        isCharacter = true;
        UIManager.uimanager.UiObj.SetActive(true);
        //UIManager.uimanager.InfoReset();
        Load_C_Data(_index);        
        Load_Map_Data();
        Load_Mob_Data();
        //Load_Npc_Data();        
        //UIManager.uimanager.InfoUpdate();
        ObjectPoolManager.objManager.PoolingReset_Load();
        //SkillManager.skillmanager.ApplySkill();        
        UIManager.uimanager.minimap.MapSetting();   //미니맵 변경
        //QuestManager.questManager.applyQuest();
        CameraManager.cameraManager.IsCharacter = true;

    }
    public void NewGame()       // 새로운 게임시작 
    {
        
        isCharacter = true;                     
        New_C_Data();
        UIManager.uimanager.UiObj.SetActive(true);
        //UIManager.uimanager.InfoReset();
        Load_Map_Data();
        Load_Mob_Data();
        Load_Npc_Data();        
        //UIManager.uimanager.InfoUpdate();
        ObjectPoolManager.objManager.PoolingReset_Load();
        UIManager.uimanager.minimap.MapSetting();
        CameraManager.cameraManager.IsCharacter = true;
    }
    public void NewArea(string _name)       // 맵이동
    {
        ObjectPoolManager.objManager.PoolingReset_newArea();
        isCharacter = true;
        MapName = _name;
        Character.Player.ListReset();     //몹orNPC리스트 초기화   
        Load_Map_Data();
        Load_Mob_Data();
        Load_Npc_Data();
        //QuestManager.questManager.applyQuest();
        UIManager.uimanager.minimap.MapSetting();
        CameraManager.cameraManager.IsCharacter = true;
    }

    public void MainMenu()
    {
        UIManager.uimanager.UiObj.SetActive(false);
        CameraManager.cameraManager.IsCharacter = false;        
        isCharacter = false;        
        ObjectPoolManager.objManager.PoolingReset_Load();
        //UIManager.uimanager.InfoReset();
        Destroy(Character.Player.gameObject);        
        
        


    }
    public void New_C_Data()
    {
        character = null;
        
        GameObject obj = Instantiate(resource.GetGameObject("Character"));        
        character = obj.AddComponent<Character>();
        Status NewStat = new Status("New", 0, 1, 0, 0, 0, 0);       

        NewStat.LevelSetting(1);
        character.Stat = NewStat;
        character.tag = "Player";
        character.gameObject.layer = 8;        
        character.name = "Player";        
        Character_Name = string.Empty;
        MapName = "Village";        
        character.Inven = new Inventory();

        // Test


        Item test1 = new Item(0, 1, "test", "apple", "테스트입니다", "테스트입니다", 0);
        
        Item test2 = new Item(0, 1, "test2", "armor", "테스트입니다", "테스트입니다", 0);
        Item test3 = new Item(0, 1, "test3", "axe", "테스트입니다", "테스트입니다", 0);
        Item test4 = new Item(0, 1, "test4", "bag", "테스트입니다", "테스트입니다", 0);
        

        character.Inven.PushItem(test1);
        character.Inven.PushItem(test2);
        character.Inven.PushItem(test3);
        character.Inven.PushItem(test4);
        



        //






        character.QuickSlot = new QuickSlot();
        character.quest = new CharacterQuest();
        Equipment newEquip = new Equipment(NewStat);
        character.Equip = newEquip;
        character.StartPos = new Vector3(31f,0f,17f);        
    }
    public void Load_C_Data(int _num)
    {
        if (Player_num == -1)
        {            
            New_C_Data();
            return;
        }
            
        string temp = UserDataManager.instance.LoadData(_num);
        string[] DATA = temp.Split('\n');
        GameObject obj = Instantiate(resource.GetGameObject("Character"));        
        character = obj.AddComponent<Character>();        
        character.tag = "Player";
        character.gameObject.layer = 8;
        string []info = DATA[0].Split(',');
        character.name = info[0];
        Status NewStat = new Status(info[0], int.Parse(info[10]), int.Parse(info[5]), float.Parse(info[6]), float.Parse(info[7]), int.Parse(info[8]), int.Parse(info[9]));
        character.Stat = NewStat;
        ChangeLayerObj(character.gameObject.transform, 8);
        MapName = info[1];
        character.StartPos = new Vector3(float.Parse(info[2]), float.Parse(info[3]), float.Parse(info[4]));
        Inventory NewInven = new Inventory();
        character.Inven = NewInven;
        QuickSlot NewQuick = new QuickSlot();
        character.QuickSlot = NewQuick;
        Equipment newEquip = new Equipment(NewStat);
        character.Equip = newEquip;

        if(DATA[1] != "")
        {
            info = DATA[1].Split('/');            
            foreach (string one in info)    // 인벤
            {
                string[] sInven = one.Split(',');
                
                List<string> iteminfo = resource.GetTable(TABLETYPE.ITEM, int.Parse(sInven[0]));
                Item tmp = new Item(int.Parse(iteminfo[0]), int.Parse(iteminfo[1]), iteminfo[2], iteminfo[3], iteminfo[4], iteminfo[5], int.Parse(iteminfo[6]), int.Parse(iteminfo[7]));
                tmp.SlotNum = int.Parse(sInven[1]);
                tmp.ItemCount = int.Parse(sInven[2]);
                
                character.Inven.AddItem(int.Parse(sInven[1]), tmp);
            }
        }
        if (DATA[2] != "")
        {
            info = DATA[2].Split('/');      //장비
            foreach (string one in info)
            {
                string[] sInven = one.Split(',');
                List<string> iteminfo = resource.GetTable(TABLETYPE.ITEM, int.Parse(sInven[0]));
                Item tmp = new Item(int.Parse(iteminfo[0]), int.Parse(iteminfo[1]), iteminfo[2], iteminfo[3], iteminfo[4], iteminfo[5], int.Parse(iteminfo[6]), int.Parse(iteminfo[7]));
                tmp.SlotNum = int.Parse(sInven[1]);
                tmp.ItemCount = 1;
                character.Equip.AddItem(tmp);        
                    
            }
        }
        if (DATA[3] != "")
        {
            info = DATA[3].Split('/');      //퀵아이템
            foreach (string one in info)
            {
                string[] sInven = one.Split(',');
                List<string> iteminfo = resource.GetTable(TABLETYPE.ITEM, int.Parse(sInven[0]));
                Item tmp = new Item(int.Parse(iteminfo[0]), int.Parse(iteminfo[1]), iteminfo[2], iteminfo[3], iteminfo[4], iteminfo[5], int.Parse(iteminfo[6]), int.Parse(iteminfo[7]));
                tmp.SlotNum = int.Parse(sInven[1]);
                tmp.ItemCount = int.Parse(sInven[2]);                
                //character.QuickSlot.ExchangeItem(tmp.SlotNum, tmp);
            }
        }
        //if (DATA[4] != "")
        //{
        //    info = DATA[4].Split('/');      //퀘스트
        //    foreach (string one in info)
        //    {
        //        string[] squest = one.Split(',');
        //        QuestManager.questManager.AddQuest(int.Parse(squest[0]), int.Parse(squest[1]), int.Parse(squest[2]));
        //    }

        //}





    }
 
    public void Load_Map_Data()
    {
        TextAsset temp = Resources.Load<TextAsset>("csv/" + MapName + "/Map");
        string[] ldata = temp.text.Split('\r');
        for (int i = 1; i < ldata.Length - 1; i++)
        {
            string[] data = ldata[i].Split(',');
            GameObject tmpobj = Instantiate(resource.GetGameObject("Potal"));
            potal =tmpobj.AddComponent<Potal>();
            potal.MapName = data[0].Replace("\n","");
            potal.transform.position = new Vector3(float.Parse(data[1]), float.Parse(data[2]), float.Parse(data[3]));
            potal.Pos = new Vector3(float.Parse(data[4]), float.Parse(data[5]), float.Parse(data[6]));

        }

    }
    public void Load_Mob_Data()
    {

        GameObject parentObj = new GameObject("name");
        parentObj.name = "Monster";
        parentObj.transform.position = Vector3.zero;
        TextAsset temp = Resources.Load<TextAsset>("csv/" + MapName + "/Mob");
        string[] ldata = temp.text.Split('\r');
        for (int i = 1; i < ldata.Length - 1; i++)
        {
            string[] data = ldata[i].Split(',');
            
            List<string> mobinfo = resource.GetTable(TABLETYPE.MONSTER, int.Parse(data[0]));
            GameObject tmpobj = Instantiate(resource.GetGameObject(mobinfo[1]));
            Vector3 pos = new Vector3(float.Parse(data[1]), float.Parse(data[2]), float.Parse(data[3]));
            tmpobj.transform.position = pos;
            mob = tmpobj.AddComponent<Monster>();           
            mob.startPos = pos;
            Vector3 rot = new Vector3(float.Parse(data[4]), float.Parse(data[5]), float.Parse(data[6]));
            Vector3 scale = new Vector3(float.Parse(data[7]), float.Parse(data[8]), float.Parse(data[9]));            
            mob.Index = int.Parse(mobinfo[0]);
            mob.name = mobinfo[2]+i;            
            mob.MobName = mobinfo[2];
            mob.Lev = int.Parse(mobinfo[3]);
            mob.Hp = float.Parse(mobinfo[4]);
            mob.Hp_max = mob.Hp;
            mob.Atk = float.Parse(mobinfo[5]);
            mob.Item = mobinfo[6];
            mob.Gold = int.Parse(mobinfo[7]);
            mob.Exp = int.Parse(mobinfo[8]);
            mob.Nick_y = float.Parse(mobinfo[9]);
            mob.soundsName = mobinfo[10];
            mob.transform.parent = parentObj.transform;
            mob.tag = "Monster";
            ChangeLayerObj(mob.gameObject.transform, 10);

            mob.transform.localEulerAngles = rot;
            mob.transform.localScale = scale;
            
            Character.Player.MobList.Add(mob);
        }

    }
    public void Load_Npc_Data()
    {
        GameObject parentObj = new GameObject("name");
        parentObj.name = "Npc";
        parentObj.transform.position = Vector3.zero;
        TextAsset temp = Resources.Load<TextAsset>("csv/"+ MapName+"/Npc");
        string[] ldata = temp.text.Split('\r');
        for (int i = 0; i < ldata.Length - 1; i++)
        {
            string[] data = ldata[i].Split(',');
            if (resource.GetGameObject(data[2]) != null)
            {
                GameObject tmpobj = Instantiate(resource.GetGameObject(data[2]));
                Vector3 pos = new Vector3(float.Parse(data[3]), float.Parse(data[4]), float.Parse(data[5]));
                Vector3 rot = new Vector3(float.Parse(data[6]), float.Parse(data[7]), float.Parse(data[8]));
                Vector3 scale = new Vector3(float.Parse(data[9]), float.Parse(data[10]), float.Parse(data[11]));
                tmpobj.transform.position = pos;
                tmpobj.transform.localEulerAngles = rot;
                tmpobj.transform.localScale = scale;
                npc = tmpobj.AddComponent<Npc>();
                npc.transform.parent = parentObj.transform;
                npc.startPos = pos;
                npc.name = data[2];
                npc.NpcName = data[1];
                npc.index = int.Parse(data[0]);
                npc.Dialog_num = 1;
                npc.NextDialog = -1;
                npc.tag = "Npc";
                npc.Nick_y = float.Parse(data[14]);                
                ChangeLayerObj(npc.gameObject.transform, 11);
                //대화데이터
                string dialog = NpcDialog(data[2]);
                string[] dialog_data = dialog.Split('\r');
                for (int j = 1; j < dialog_data.Length - 1; j++)
                {
                    string[] tmp = dialog_data[j].Split(',');
                    for (int a = 0; a < tmp.Length; a++)
                    {
                        //npc.Dialog.AddData(int.Parse(tmp[0]), tmp[a]);
                    }
                }
                //상점 물건 데이터

                if (data[12] != "")
                {
                    string[] shop = data[12].Split('/');
                    for (int k = 0; k < shop.Length - 1; k++)
                    {

                        List<string> iteminfo = resource.GetTable(TABLETYPE.ITEM, int.Parse(shop[k]));
                        Item tmp = new Item(int.Parse(iteminfo[0]), int.Parse(iteminfo[1]), iteminfo[2], iteminfo[3], iteminfo[4], iteminfo[5], int.Parse(iteminfo[6]), int.Parse(iteminfo[7]));
                        tmp.ItemCount = 1;
                        npc.item_list.Add(tmp);

                    }
                }
                if(data[13] != "")
                {
                    string[] Questlist = data[13].Split('/');
                    for (int n = 0; n < Questlist.Length; n++)
                    {
                        List<string> questinfo = resource.GetTable(TABLETYPE.QUEST, int.Parse(Questlist[n]));                        
                        Quest tmp  = new Quest(int.Parse(questinfo[0]), questinfo[1], questinfo[2], questinfo[3], questinfo[4], int.Parse(questinfo[5]), int.Parse(questinfo[6]));
                        npc.quest_list.Add(tmp);

                    }   
                }
                Character.Player.npcList.Add(npc);

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
