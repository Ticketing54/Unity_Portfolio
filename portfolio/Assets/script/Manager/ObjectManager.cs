using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{

    public static ObjectManager objManager;

    List<Monster> mobList = new List<Monster>();
    Dictionary<int, Npc> npcDic = new Dictionary<int, Npc>();        
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
    public void AddMobList(Monster _mob)
    {
        mobList.Add(_mob);
    }
    public void AddnpcDic(int _index,Npc _npc)
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

    public void UpdateQuestMark(int _npcIndex)
    {
        Npc npc;
        if (npcDic.TryGetValue(_npcIndex, out npc))
        {
            npc.SetQuestMark();
        }
        else
        {
            Debug.Log("��ǥ�� ã�� ���߽��ϴ�.");

        }
    }

    public void MoveOtherMap()
    {        
        mobList.Clear();
        npcDic.Clear();        
    }

}
