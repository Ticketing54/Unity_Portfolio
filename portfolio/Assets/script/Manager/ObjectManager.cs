using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager objManager;    
    Dictionary<int, Npc> npcDic;
    Dictionary<Monster, Coroutine> respawnMob;


    private void Awake()
    {
        if (objManager == null)
        {
            objManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        npcDic = new Dictionary<int, Npc>();
        respawnMob = new Dictionary<Monster, Coroutine>();

    }

    private void Start()
    {
        GameManager.gameManager.moveSceneReset += MoveOtherMap;
    }


    public void StartRespawnMob(Monster _mob)
    {
        Coroutine coRespawn = StartCoroutine(CoRespawn(_mob));
        respawnMob.Add(_mob, coRespawn);    
    }
    

    IEnumerator CoRespawn(Monster _mob)
    {
        _mob.gameObject.SetActive(false);
        yield return new WaitForSeconds(5f);
        respawnMob.Remove(_mob);
        _mob.gameObject.SetActive(true);
        
    }
    
    public void AddnpcDic(int _index, Npc _npc)
    {
        if (npcDic.ContainsKey(_index))
        {
            Debug.LogError("이미 있는 Npc 를 추가하려 합니다.");
        }
        else
        {
            npcDic.Add(_index, _npc);
        }

    }
    public Npc GetNpc(int _index)
    {
        if (npcDic.ContainsKey(_index))
        {
            return npcDic[_index];
        }
        else
        {
            return null;
        }
    }

    public void UpdateQuestMark(int _npcIndex)
    {
        Npc npc;
        if (npcDic.TryGetValue(_npcIndex, out npc))
        {
            npc.SetQuestMark();
        }
        else
        {
            Debug.Log("목표를 찾지 못했습니다.");

        }
    }

    public void MoveOtherMap()
    {
        respawnMob.Clear();
        npcDic.Clear();
    }
}
