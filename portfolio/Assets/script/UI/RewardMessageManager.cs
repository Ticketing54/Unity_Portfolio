using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardMessageManager : MonoBehaviour
{
    [SerializeField]
    GameObject msgParent;
    [SerializeField]
    GameObject contents;
    [SerializeField]
    RewardMessage msg;


    HashSet<RewardMessage> runningMsg;
    PoolData<RewardMessage> poolMsg;

    private void Start()
    {
        runningMsg = new HashSet<RewardMessage>();
        poolMsg = new PoolData<RewardMessage>(msg, msgParent, "GetRewardMessage");
        UIManager.uimanager.AGetGoldUpdateUi += AddMsg_Gold;
        UIManager.uimanager.AGetExpUpdateUi += AddMsg_Exp;

    }

    void AddMsg_Gold(int _gold)
    {
        string msg = " + " + _gold + " Gold를 얻었습니다.";
        AddMsg(msg);
    }
    void AddMsg_Exp(int _exp)
    {
        string msg = " + " + _exp+ " Gold를 얻었습니다.";
        AddMsg(msg);
    }




    void AddMsg(string _msg)
    {
        RewardMessage newMsg = poolMsg.GetData();
        newMsg.transform.SetParent(contents.transform);
        newMsg.SetMessage(RemoveMsg, _msg);

        runningMsg.Add(newMsg);
    }

    void RemoveMsg(RewardMessage _rwMsg)
    {   
        runningMsg.Remove(_rwMsg);
        poolMsg.Add(_rwMsg);
    }

    

}
