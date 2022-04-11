using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{

    public static ObjectManager objManager;

    public List<Monster> mobList = new List<Monster>();
    public Dictionary<int, Npc> npcDic = new Dictionary<int, Npc>();
    private void Awake()
    {
        if(objManager == null)
        {
            objManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }        
    }


    public Npc FindQuestNpc(int _npcIndex)
    {
        Npc npc;
        if (npcDic.TryGetValue(_npcIndex, out npc))
        {
            return npc;
        }


        return null;
    }

    public void MoveOtherMap()
    {
        StopAllCoroutines();
        mobList.Clear();
        npcDic.Clear();        
    }

}
