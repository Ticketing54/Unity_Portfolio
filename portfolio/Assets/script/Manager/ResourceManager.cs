using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using System;


public class ResourceManager: MonoBehaviour
{
    public static ResourceManager resource;

    [SerializeField]
    List<Npc> runningNpc = new List<Npc>();
    [SerializeField]
    List<Monster> runningMonster = new List<Monster>();
    
    public GameObject character;

    private void Awake()
    {
        if (resource == null)
        {
            resource = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void ReleaseAddressable()
    {
        foreach (Npc one in runningNpc)
        {
            Addressables.ReleaseInstance(one.gameObject);
        }
        foreach (Monster one in runningMonster)
        {
            Addressables.ReleaseInstance(one.gameObject);
        }
    }
    Dictionary<string, GameObject> loadObj;
    Dictionary<string, string> mapInfo;    
    
    List<GameObject> ClickEffect;

    #region StartResource[Table/Image]

    IList<IResourceLocation> tableList;                 // 테이블 주소
    IList<IResourceLocation> imageList;                 // 이미지 주소
    IList<IResourceLocation> dialogList;                // 이미지 주소
    IList<IResourceLocation> effectList;                // 이펙트 주소

    bool tableSetting       = false;                          // 세팅 체크
    bool imageSetting       = false;                          // 세팅 체크
    bool dialogueSetting    = false;
    bool characterSetting   = false;
    bool effectSetting      = false;
    Dictionary<string, Sprite> imageRes = new Dictionary<string, Sprite>();
    Dictionary<string, List<string>> tableRes = new Dictionary<string, List<string>>();          // 배열 인덱스가 각각의 인덱스
    Dictionary<string, List<string>> dialogueRes = new Dictionary<string, List<string>>();
    Dictionary<string, GameObject> effectRes = new Dictionary<string, GameObject>();
    public void StartResourceSetting()
    {
        StartCoroutine(IsSettingBasicRes());
        Addressables.LoadResourceLocationsAsync("Table").Completed +=
            (talbeLocation) =>
            {
                tableList = talbeLocation.Result;
                Addressables.LoadAssetsAsync<TextAsset>(tableList, SaveTextAsset).Completed +=
                (ReadyToStart) =>
                {
                    Addressables.Release<IList<TextAsset>>(ReadyToStart.Result);
                    tableSetting = true;
                };
            };
        Addressables.LoadResourceLocationsAsync("Image").Completed +=
            (imageLocation) =>
            {
                imageList = imageLocation.Result;
                Addressables.LoadAssetsAsync<Sprite>(imageList, SaveImageAsset).Completed +=
                (ReadyToStart) =>
                {
                    imageSetting = true;
                };
            };

        Addressables.LoadAssetAsync<GameObject>("PlayerCharacter").Completed +=
            (player) =>
            {               
                
                character = player.Result;
                characterSetting = true;

            };
        Addressables.LoadResourceLocationsAsync("Dialogue").Completed +=
           (dialogLocation) =>
           {
               dialogList = dialogLocation.Result;
               Addressables.LoadAssetsAsync<TextAsset>(dialogList, SaveDialogAsset).Completed +=
               (ReadyToStart) =>
               {
                   
                   Addressables.Release<IList<TextAsset>>(ReadyToStart.Result);
                   dialogueSetting = true;
               };
           };
        Addressables.LoadResourceLocationsAsync("Effect").Completed +=
          (dialogLocation) =>
          {
              effectList = dialogLocation.Result;
              Addressables.LoadAssetsAsync<GameObject>(effectList, SaveEffectAsset).Completed +=
              (ReadyToStart) =>
              {
                  EffectManager.effectManager.BasicEffectAdd();
                  effectSetting = true;
              };
          };
    }
    
    IEnumerator IsSettingBasicRes()
    {
        while (!(tableSetting&& imageSetting && dialogueSetting && characterSetting  && effectSetting))
        {
            yield return null;
        }

        PatchManager.patchManager.readyToPlay = true;
    }
    void SaveTextAsset(TextAsset _result)
    {
        if (!tableRes.ContainsKey(_result.name))
        {
            string[] line = _result.text.Split(Environment.NewLine.ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
            List<String> info = new List<string>();
            for (int i = 1; i < line.Length; i++)
            {
                info.Add(line[i]);
            }
            tableRes.Add(_result.name, info);
        }
    }
    void SaveDialogAsset(TextAsset _result)
    {
        if (!dialogueRes.ContainsKey(_result.name))
        {
            string[] line = _result.text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            List<String> info = new List<string>();
            for (int i = 1; i < line.Length; i++)
            {
                info.Add(line[i]);
            }
            dialogueRes.Add(_result.name, info);
        }
    }
    void SaveImageAsset(Sprite _result)
    {
        if (!imageRes.ContainsKey(_result.name))
        {
            imageRes.Add(_result.name, _result);
        }
    }
    void SaveEffectAsset(GameObject _result)
    {
        if (!effectRes.ContainsKey(_result.name))
        {
            effectRes.Add(_result.name, _result);
        }
    }



    #endregion



    #region Loading[Obg/Map]
    
    public void LoadSceneResource(string _mapTableName)
    {
        List<string> mapInfo;
        

        if (GameManager.gameManager.character == null)
        {
            Character player = Instantiate(character).AddComponent<Character>();
            GameManager.gameManager.character = player;            
        }
       
        if (tableRes.TryGetValue(_mapTableName, out mapInfo))
        {
            ReleaseAddressable();

            // 시작 시 로드할 오브젝트의 수 세팅            

            string[] sInfo;
            for (int i = 0; i < mapInfo.Count-1; i++)
            {
                sInfo = mapInfo[i].Split(',');
               
                    
                Vector3 pos = new Vector3(float.Parse(sInfo[2]), float.Parse(sInfo[3]), float.Parse(sInfo[4]));
                Quaternion rotate = Quaternion.Euler(float.Parse(sInfo[5]), float.Parse(sInfo[6]), float.Parse(sInfo[7]));
                Vector3 scale = new Vector3(float.Parse(sInfo[8]), float.Parse(sInfo[9]), float.Parse(sInfo[10]));


                List<string> npcTable = GetTable("NpcTable");
                List<string> monsterTable = GetTable("MonsterTable");

                switch (sInfo[0])
                {
                    case "Npc":
                        {
                            List<string> npcTable_index = GetTable(npcTable,int.Parse(sInfo[1]));
                            Addressables.InstantiateAsync(npcTable_index[1]).Completed +=
                                (handle) =>
                                {
                                    GameObject npcObj = handle.Result;             
                                    if(npcObj == null)
                                    {
                                        Debug.Log("npc 가 없습니다.");
                                    }
                                    npcObj.gameObject.tag = "Npc";
                                    npcObj.gameObject.transform.position = pos;
                                    npcObj.gameObject.transform.rotation = rotate;
                                    npcObj.gameObject.transform.localScale = scale;
                                    SettingNpc(npcObj, npcTable_index);     
                                };
                            break;
                        }                        
                    case "Monster":
                        {
                            List<string> monsterTable_index = GetTable(monsterTable, int.Parse(sInfo[1]));                            
                            Addressables.InstantiateAsync(monsterTable_index[1]).Completed +=
                                (handle) =>
                                {
                                    GameObject monsterObj = handle.Result;
                                    monsterObj.gameObject.tag = "Monster";
                                    monsterObj.gameObject.transform.position = pos;
                                    monsterObj.gameObject.transform.rotation = rotate;
                                    monsterObj.gameObject.transform.localScale = scale;
                                    if(monsterObj == null){
                                        Debug.Log("Monster 가 없습니다.");
                                    }
                                    SettingMonster(monsterObj, monsterTable_index);
                                };
                            break;
                        }
                    case "Boss":
                        {
                            break;
                        }
                    default:
                        {
                            Debug.Log("존재하지않는 형식의 오브젝트입니다.");
                            break;
                        }                        
                }
            }
            LoadingSceneController.instance.resourceSetting = true;            
        }
        else
        {
            Debug.Log("맵정보가 없습니다.");
        }
    }
    
    void SettingMonster(GameObject _obj, List<string> _table)
    {
        Monster newMonster;
        switch (_table[2])                                                  // 몬스터 타입별 스크립트 
        {
            case "Nomal":
                {
                    newMonster= (Monster)_obj.AddComponent<Nomal_Monster>();
                    break;
                }
            case "Tutorial":
                {
                    newMonster = (Monster)_obj.AddComponent<Tutorial_Monster>();
                    break;
                }
            case "Offensive":
                {
                    newMonster = (Monster)_obj.AddComponent<Tutorial_Monster>();        //
                    break;
                }
            case "Mimic":
                {
                    newMonster = (Monster)_obj.AddComponent<Tutorial_Monster>();        //
                    break;
                }
            default:
                {
                    return;
                }
        }
        if(newMonster == null)
        {
            Debug.Log("Monster 가 생성되지 않았습니다.");
            return;
        }
        newMonster.name = _table[3];
        if (!string.IsNullOrEmpty(_table[7]))
        {
            string[] dropItemsInfo = _table[7].Split('/');
            List<int[]> items = new List<int[]>();
            for (int i = 0; i < dropItemsInfo.Length; i++)
            {
                int[] iteminfo = Array.ConvertAll(dropItemsInfo[i].Split('#'), index => int.Parse(index));
                items.Add(iteminfo);
            }
            newMonster.SetMonster(int.Parse(_table[0]),_table[3], int.Parse(_table[4]), float.Parse(_table[5]), float.Parse(_table[6]), int.Parse(_table[7]), int.Parse(_table[8]), float.Parse(_table[10]), _table[11], items);
        }
        else
        {
            newMonster.SetMonster(int.Parse(_table[0]),_table[3], int.Parse(_table[4]), float.Parse(_table[5]), float.Parse(_table[6]), int.Parse(_table[7]), int.Parse(_table[8]), float.Parse(_table[10]), _table[11]);
        }

        ObjectManager.objManager.AddMobList(newMonster);
    }
    void SettingNpc(GameObject _obj,List<string>_table)                                                     // 차후 추가하게될경우 추가
    {
        Npc newNpc;
        switch (_table[2])
        {
            case "Nomal":
                {
                    newNpc = (Npc)_obj.AddComponent<Nomal_Npc>();                   
                    break;
                }
            case "Tutorial":
                {
                    newNpc = (Npc)_obj.AddComponent<Tutorial_Npc>();
                    break;
                }
            case "Passerby":
                {
                    newNpc = (Npc)_obj.AddComponent<Tutorial_Npc>();            //
                    break;
                }
            case "NoneDialog":
                {
                    newNpc = (Npc)_obj.AddComponent<Tutorial_Npc>();            //
                    break;
                }
            default:
                {
                    Debug.Log("데이터가 없습니다.");
                    return;
                } 
        }

        if (newNpc == null)
        {
            Debug.Log("Npc 가 생성되지 않았습니다.");
            return;
        }

        List<int> npcitems = null;
        List<int> npcQuest = null;
        if (!string.IsNullOrEmpty(_table[4]))
        {
            string[] items = _table[4].Split('/');
            npcitems = new List<int>();
            for (int i = 0; i < items.Length; i++)
            {
                npcitems.Add(int.Parse(items[i]));
            }            
        }
        if (!string.IsNullOrEmpty(_table[5]))
        {
            string[] quests = _table[5].Split('/');
            npcQuest = new List<int>();
            for (int i = 0; i < quests.Length; i++)
            {
                npcQuest.Add(int.Parse(quests[i]));
            }            
        }
        
        newNpc.SetNpc(int.Parse(_table[0]),_table[3], float.Parse(_table[6]), npcQuest, npcitems,_table[7]);

        ObjectManager.objManager.AddnpcDic(int.Parse(_table[0]), newNpc);        
    }
    #endregion


    public GameObject Instantiate(string _name) { return null; } /// <summary>
    public Sprite GetImage(string _name)
    {
        Sprite getImage;
        if(imageRes.TryGetValue(_name,out getImage))
        {
            return getImage;
        }
        else
        {
            Debug.Log("이미지가 존재하지 않습니다.");
            return null;
        }
    }


    public List<string> GetTable(string _type)
    {
        List<string> table;
        if (tableRes.TryGetValue(_type, out table))
        {
            return table;
        }
        return null;
    }
    public List<string> GetTable(List<string> _table,int _index)
    {
        string[] getTable = _table[_index-1].Split(',');
        List<string> getList = new List<string>();

        for (int i = 0; i < getTable.Length; i++)
        {
            getList.Add(getTable[i]);
        }

        return getList;
    }
    public List<string>  GetTable_Index(string _type, int _index)
    {
        List<string> table = GetTable(_type);
        
        if(table == null)
        {
            return null;
        }

        string[] getTable = table[_index-1].Split(',');
        List<string> getList = new List<string>();

        for (int i = 0; i < getTable.Length; i++)
        {
            getList.Add(getTable[i]);
        }

        return getList;       
    }
    #region Dialogue
    public List<string> GetDialogue(string _npcDialog)
    {
        List<string> dialog;
        if (dialogueRes.TryGetValue(_npcDialog, out dialog))
        {
            return dialog;
        }
        return null;
    }   
    public GameObject GetEffect(string _effectName)
    {
        GameObject effect;
        if (effectRes.TryGetValue(_effectName, out effect))
        {
            
            return Instantiate(effect);
        }
        return null;
    }
    #endregion
}