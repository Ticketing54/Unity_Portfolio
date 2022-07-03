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
            Debug.LogError("�̹� �ִ� Npc �� �߰��Ϸ� �մϴ�.");
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

    public void UpdateQuestMark(Quest _quest)
    {
        Quest quest = _quest;
        int startNpc = quest.Start_Npc;
        int endNpc   = quest.Goal_Npc;


        if (npcDic.ContainsKey(startNpc))
        {
            npcDic[startNpc].SetQuestMark();
        }


        if (npcDic.ContainsKey(endNpc))
        {
            npcDic[endNpc].SetQuestMark();
        }
    }

    public void MoveOtherMap()
    {
        respawnMob.Clear();
        npcDic.Clear();
    }
}
