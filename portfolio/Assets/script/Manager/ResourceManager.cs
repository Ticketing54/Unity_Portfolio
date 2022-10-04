using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;
using System;


public class ResourceManager: MonoBehaviour
{

    public static ResourceManager resource;
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

        imageRes    = new();
        tableRes    = new();
        dialogueRes = new();
        effectRes   = new();
        soundRes    = new();
    }



    #region StartResource[Table/Image]
    public Action AClosePatchUi;
    Dictionary<string, Sprite> imageRes;
    Dictionary<string, List<string>> tableRes;          // 배열 인덱스가 각각의 인덱스
    Dictionary<string, List<string>> dialogueRes;
    Dictionary<string, GameObject> effectRes;
    Dictionary<string, AudioClip> soundRes ;
    public void StartResourceSetting()
    {
        StartCoroutine(CoStartResourceSetting());
    }
    
    
    IEnumerator CoStartResourceSetting()
    {
        IList<IResourceLocation> tableList;                 // 테이블 주소
        IList<IResourceLocation> imageList;                                                    // 
        IList<IResourceLocation> dialogList;                // 이미지 주소
        IList<IResourceLocation> effectList;                // 이펙트 주소
        IList<IResourceLocation> soundsList;

        AsyncOperationHandle<IList<IResourceLocation>> tableHandle = Addressables.LoadResourceLocationsAsync("Table");
        yield return tableHandle;
        tableList = tableHandle.Result;

        AsyncOperationHandle<IList<IResourceLocation>> imageHandle = Addressables.LoadResourceLocationsAsync("Image");
        yield return imageHandle;
        imageList= imageHandle.Result;

        AsyncOperationHandle<IList<IResourceLocation>> dialogHandle = Addressables.LoadResourceLocationsAsync("Dialogue");
        yield return dialogHandle;
        dialogList = dialogHandle.Result;

        AsyncOperationHandle<IList<IResourceLocation>> effectHandle = Addressables.LoadResourceLocationsAsync("Effect");
        yield return effectHandle;
        effectList = effectHandle.Result;

        AsyncOperationHandle<IList<IResourceLocation>> soundsHandle = Addressables.LoadResourceLocationsAsync("Sounds");
        yield return soundsHandle;
        soundsList = soundsHandle.Result;

        AsyncOperationHandle<IList<TextAsset>> tableAssetHandle = Addressables.LoadAssetsAsync<TextAsset>(tableList, SaveTextAsset);
        yield return tableAssetHandle;
        Addressables.Release<IList<IResourceLocation>>(tableList);
        Addressables.Release<IList<TextAsset>>(tableAssetHandle.Result);

        AsyncOperationHandle<IList<Sprite>> imageAssetHandle = Addressables.LoadAssetsAsync<Sprite>(imageList, SaveImageAsset);
        yield return imageAssetHandle;
        Addressables.Release<IList<IResourceLocation>>(imageList);

        AsyncOperationHandle<IList<TextAsset>> dialogAssetHandle = Addressables.LoadAssetsAsync<TextAsset>(dialogList, SaveDialogAsset);
        yield return dialogAssetHandle;
        Addressables.Release<IList<IResourceLocation>>(dialogList);
        Addressables.Release<IList<TextAsset>>(dialogAssetHandle.Result);

        AsyncOperationHandle<IList<GameObject>> effectAssetHandle = Addressables.LoadAssetsAsync<GameObject>(effectList, SaveEffectAsset);
        yield return effectAssetHandle;
        Addressables.Release<IList<IResourceLocation>>(effectList);
        EffectManager.effectManager.BasicEffectAdd();

        AsyncOperationHandle<IList<AudioClip>> soundAssetHandle = Addressables.LoadAssetsAsync<AudioClip>(soundsList, SaveSoundsAsset);
        yield return soundAssetHandle;
        Addressables.Release<IList<IResourceLocation>>(soundsList);

        AsyncOperationHandle<GameObject> playercharacter = Addressables.LoadAssetAsync<GameObject>("PlayerCharacter");        
        yield return playercharacter;
        character = playercharacter.Result;

        AsyncOperationHandle<GameObject> dialogText = Addressables.LoadAssetAsync<GameObject>("DialogText");
        yield return dialogText;        

        AsyncOperationHandle<GameObject> dialogBubble = Addressables.LoadAssetAsync<GameObject>("DialogBubble");
        yield return dialogBubble;

        AsyncOperation mainScene = SceneManager.LoadSceneAsync("Main");
        yield return mainScene;
        TextMeshPro pro = dialogText.Result.GetComponent<TextMeshPro>();
        EffectManager.effectManager.AddSpeechBubbleResource(dialogBubble.Result, pro);

        AClosePatchUi();
    }









    void SaveTextAsset(TextAsset _result)
    {
        if (!tableRes.ContainsKey(_result.name))
        {
            string[] line = _result.text.Split(Environment.NewLine.ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
            List<String> info = new();
            for (int i = 0; i < line.Length; i++)
            {
                info.Add(line[i]);
            }
            tableRes.Add(_result.name, info);
        }
    }

    void SaveImageAsset(Sprite _result)
    {
        if (!imageRes.ContainsKey(_result.name))
        {
            imageRes.Add(_result.name, _result);
        }
    }

    void SaveDialogAsset(TextAsset _result)
    {
        if (!dialogueRes.ContainsKey(_result.name))
        {
            string[] line = _result.text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            List<String> info = new ();
            for (int i = 0; i < line.Length; i++)
            {
                info.Add(line[i]);
            }
            dialogueRes.Add(_result.name, info);
        }
    }
  
    void SaveEffectAsset(GameObject _result)
    {
        if (!effectRes.ContainsKey(_result.name))
        {
            effectRes.Add(_result.name, _result);
        }
    }

    void SaveSoundsAsset(AudioClip _sound)
    {
        if (!soundRes.ContainsKey(_sound.name))
        {
            soundRes.Add(_sound.name, _sound);
        }
    }

    #endregion



    #region Loading[Obg/Map]
    
    

    List<string> LoadText(TextAsset _textAsset)
    {
        if(_textAsset == null)
        {
            return null;
        }
        else
        {
            string [] line = _textAsset.text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            List<string> loadNpcList = new();
            loadNpcList.AddRange(line);
            return loadNpcList;
        }        
    }    
    IEnumerator CoLoadNpc(string _mapName)
    {
        AsyncOperationHandle<TextAsset> mapRes = Addressables.LoadAssetAsync<TextAsset>(_mapName + "Npc");
        yield return mapRes;
            
            
        List<string> npcInfo = LoadText(mapRes.Result);
        Addressables.Release<TextAsset>(mapRes.Result);


        List<string> npcTable = GetTable("NpcTable");
        for (int i = 1; i < npcInfo.Count; i++)
        {
            string[] sInfo = npcInfo[i].Split(',');

            List<string> npcTable_index = GetTable(npcTable, int.Parse(sInfo[0]));

            AsyncOperationHandle<GameObject> npc= Addressables.InstantiateAsync(npcTable_index[1]);
            yield return npc;


            
            SettingNpc(npc.Result, npcTable_index,sInfo);
        }
    }
    IEnumerator CoLoadMonster(string _mapName)
    {   
        AsyncOperationHandle<TextAsset> mapRes = Addressables.LoadAssetAsync<TextAsset>(_mapName + "Monster");

        yield return mapRes;
        List<string> mobInfo = LoadText(mapRes.Result);
        Addressables.Release<TextAsset>(mapRes);


        List<string> mobTable = GetTable("MonsterTable");
        for (int i = 1; i < mobInfo.Count; i++)
        {
            string[] sInfo = mobInfo[i].Split(',');            
            
            AsyncOperationHandle<GameObject> mobHandle = Addressables.InstantiateAsync(sInfo[2]);
            yield return mobHandle;


            GameObject mob = mobHandle.Result;
            Vector3 pos = new(float.Parse(sInfo[3]), float.Parse(sInfo[4]), float.Parse(sInfo[5]));
            Quaternion rotate = Quaternion.Euler(float.Parse(sInfo[6]), float.Parse(sInfo[7]), float.Parse(sInfo[8]));
            Vector3 scale = new(float.Parse(sInfo[9]), float.Parse(sInfo[10]), float.Parse(sInfo[11]));


            mob.tag = "Monster";
            mob.layer = 10;
            mob.transform.position = pos;
            mob.transform.rotation = rotate;
            mob.transform.localScale = scale;
            mob.name = sInfo[2];

            SettingMonster(mob, int.Parse(sInfo[1]), sInfo[0],pos);            
        }
    }

    IEnumerator CoLoadPotal(string _mapName)
    {

        AsyncOperationHandle<TextAsset> mapRes = Addressables.LoadAssetAsync<TextAsset>(_mapName + "Potal");
        
        yield return mapRes ;
        List<string> npcInfo = LoadText(mapRes.Result);
        Addressables.Release<TextAsset>(mapRes);
        for (int i = 1; i < npcInfo.Count; i++)
        {
            string[] sInfo = npcInfo[i].Split(',');

            GameObject potalObj;
            yield return potalObj = Addressables.InstantiateAsync("Potal").Result;
            Potal potal = potalObj.AddComponent<Potal>();
            potal.SettingPotal(sInfo);
            ObjectManager.objManager.AddRunningPotal(potal);
        }
    }
    IEnumerator CoLoadWayPoint(string _mapName)
    {
        GameManager.gameManager.ClearWayPoint();       

        AsyncOperationHandle<TextAsset> mapRes = Addressables.LoadAssetAsync<TextAsset>(_mapName + "WayPoint");
        yield return mapRes;

        List<string> wayPointList = LoadText(mapRes.Result);
        Addressables.Release<TextAsset>(mapRes);

        for (int i = 1; i < wayPointList.Count; i++)
        {
            string[] sInfo = wayPointList[i].Split(',');
            
            Vector3 wayPoint = new Vector3(float.Parse(sInfo[1]), float.Parse(sInfo[2]), float.Parse(sInfo[3]));
            GameManager.gameManager.AddWayPoint(sInfo[0], wayPoint);
        }
    }
    public IEnumerator CoLoadSceneResource(string _mapName)
    {
        yield return StartCoroutine(CoLoadNpc(_mapName));
        LoadingSceneController.instance.LoadingPercent(0.25f);
        yield return StartCoroutine(CoLoadMonster(_mapName));
        LoadingSceneController.instance.LoadingPercent(0.5f);
        yield return StartCoroutine(CoLoadPotal(_mapName));
        LoadingSceneController.instance.LoadingPercent(0.75f);
        yield return StartCoroutine(CoLoadWayPoint(_mapName));
        LoadingSceneController.instance.LoadingPercent(1);       
    }
    public void LoadSceneResource(string _mapName)
    {
        StartCoroutine(CoLoadSceneResource(_mapName));
    }
    
    void SettingMonster(GameObject _mobObj,int _mobTableIndex,string _mobType,Vector3 _startPos)
    {
        Monster newMonster;
        switch (_mobType)                                                  // 몬스터 타입별 스크립트 
        {
            case "Nomal":
                {
                    newMonster= (Monster)_mobObj.AddComponent<Nomal_Monster>();
                    break;
                }          
            case "Hide":
                {
                    newMonster = (Monster)_mobObj.AddComponent<Hide_Monster>();        //                    
                    break;
                }
            case "NotMonster":
                {
                    newMonster = (Monster)_mobObj.AddComponent<NotMonster>();        //                    
                    break;
                }
            case "Boss":
                {
                    newMonster = (Monster)_mobObj.AddComponent<TutorialBoss>();
                    newMonster.SetMonster(_mobTableIndex, _startPos);                    
                    UIManager.uimanager.aOpenTopInfoUi();
                    UIManager.uimanager.aUpdateTopinfo(newMonster);                    
                    return;
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
        else 
        {
            newMonster.SetMonster(_mobTableIndex,_startPos);            
        }
      
        
    }
    void SettingNpc(GameObject _npc,List<string>_table,string [] _objInfo)                                                     
    {
        Vector3 pos = new(float.Parse(_objInfo[1]), float.Parse(_objInfo[2]), float.Parse(_objInfo[3]));
        Quaternion rotate = Quaternion.Euler(float.Parse(_objInfo[4]), float.Parse(_objInfo[5]), float.Parse(_objInfo[6]));
        Vector3 scale = new(float.Parse(_objInfo[7]), float.Parse(_objInfo[8]), float.Parse(_objInfo[9]));



        _npc.tag = "Npc";
        _npc.transform.position = pos;
        _npc.transform.rotation = rotate;
        _npc.transform.localScale = scale;






        Npc newNpc;
        switch (_table[2])
        {
            case "Nomal":
                {
                    newNpc = (Npc)_npc.AddComponent<Nomal_Npc>();                   
                    break;
                }
            case "Tutorial":
                {
                    newNpc = (Npc)_npc.AddComponent<Tutorial_Npc>();
                    break;
                }
            case "Quest":
                {
                    newNpc = (Npc)_npc.AddComponent<MoveNpc>();            //
                    break;
                }
            case "NoneDialog":
                {
                    newNpc = (Npc)_npc.AddComponent<NonDialog_Npc>();
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
        if (!string.IsNullOrEmpty(_objInfo[10]))
        {
            string[] wayarr = _objInfo[10].Split('/');
            List<string> wayList = new List<string>();
            wayList.AddRange(wayarr);
            newNpc.wayPoint = wayList;
        }
        ObjectManager.objManager.AddnpcDic(int.Parse(_table[0]), newNpc);        
    }
    #endregion


    public AudioClip GetSound(string _name)
    {
        AudioClip sound = null;

        if (!soundRes.TryGetValue(_name, out sound))
        {
            Debug.Log(_name +"Sound is null");
        }

        return sound;
        
    }
    public Sprite GetImage(string _name)
    {
        Sprite getImage;
        if(imageRes.TryGetValue(_name,out getImage))
        {
            return getImage;
        }
        else
        {
            Debug.Log(_name+"Image is null");
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
        string[] getTable = _table[_index].Split(',');
        List<string> getList = new ();

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

        string[] getTable = table[_index].Split(',');
        List<string> getList = new ();

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