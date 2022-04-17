using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueUi : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    //public Choice choice;
    [SerializeField]
    Choice clickChoiceBar;
    [SerializeField]
    List<Choice> choice_List = new List<Choice>();
    
    PoolData<Choice> choicePool;    
    
    int runningIndex = -1;
    [SerializeField]
    ScrollRect scrollect;
    [SerializeField]
    GameObject choice_NotUsed;
    [SerializeField]
    TextMeshProUGUI npc_name;
    [SerializeField]
    TextMeshProUGUI npc_dialog;
    [SerializeField]
    GameObject reward;              // reward->Parent
    [SerializeField]
    TextMeshProUGUI rewards_Exp;
    [SerializeField]
    TextMeshProUGUI reward_Gold;    
    [SerializeField]
    TextMeshProUGUI rewards_Item;
    [SerializeField]
    AddQuestText_Ui addQuestEffect;     // 퀘스트를 수락하셨습니다. << 메세지

    List<int> items;
    List<int> questList;
    List<List<string>> npc_DialogData;

    bool isDoneTexting = false;
    int nextDilogNum = -3;

    private void Awake()
    {
        choicePool = new PoolData<Choice>(clickChoiceBar, choice_NotUsed, "Choice");
        
    }

    private void OnDisable()
    {        
        addQuestEffect.gameObject.SetActive(false);             // 퀘스트 등록 이벤트 
        ChoiceReset();
        npc_DialogData = null;
        questList = null;
        DialogTextReset();
        Character.Player.isCantMove = false;
    }
    
   
    public void OnPointerDown(PointerEventData data)
    {
        runningIndex = -1;

        if (Input.GetMouseButtonDown(0))
        {
            if(isDoneTexting == true)
            {
                isDoneTexting = false;
                Npc_Texting(nextDilogNum);
                return;
            }

            for (int i = 0; i < choice_List.Count; i++)
            {
                if (choice_List[i].gameObject.activeSelf == true && choice_List[i].isInRect(data.position) )
                {
                    runningIndex = i;
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (runningIndex < 0)
            {
                return;
            }
            if (choice_List[runningIndex].isInRect(data.position))
            {                
                Npc_Texting(choice_List[runningIndex].moveToDialogNum);
            }
        }        
    }



  
  
    public void StartDialogue(NpcUnit _npc)
    {
        if (_npc.ITEMS != null)
        {            
            items = _npc.ITEMS;
        }
        if(_npc.QUEST != null)
        {
            questList = _npc.QUEST;
        }
        List<string> dialogResData = ResourceManager.resource.GetDialogue(_npc.DIALOGUE);
        List<List<string>> dialogData = new List<List<string>>();
        for (int i = 0; i < dialogResData.Count; i++)
        {
            List<string> subdialog = new List<string>();
            string[] data = dialogResData[i].Split(',');
            for (int j = 0; j < data.Length; j++)
            {
                subdialog.Add(data[j]);
            }
            dialogData.Add(subdialog);
        }
        npc_DialogData = dialogData;
        npc_name.text = _npc.NickName;
        FirstDialog(_npc);
    }

    void CheckQuest(NpcUnit _npc)
    {
        if (questList == null)
        {
            Npc_Texting(1);
            return;
        }
        else
        {
            for (int i = 0; i < questList.Count; i++)
            {
                Quest quest = Character.Player.quest.GetQuest(questList[i]);
                if (quest == null)
                {
                    List<string> questTable = ResourceManager.resource.GetTable_Index("QuestTable", questList[i]);
                    int startNpcIndex;

                    if (int.TryParse(questTable[8], out startNpcIndex))
                    {
                        if (Character.Player.quest.ClearPrecedQuest(questList[i]) && startNpcIndex == _npc.NpcIndex)
                        {
                            Npc_Texting(int.Parse(questTable[10]));
                            return;
                        }
                        else
                        {
                            continue;
                        }
                    }

                }
                else
                {
                    switch (quest.State)
                    {
                        case QUESTSTATE.PLAYING:
                            {
                                Npc_Texting(quest.PlayingDialogIndex);
                                return;
                            }
                            
                        case QUESTSTATE.COMPLETE:
                            {
                                if(quest.Goal_Npc == _npc.NpcIndex)
                                {
                                    Npc_Texting(quest.EndDialogIndex);
                                    return;
                                }
                                else
                                {
                                    Npc_Texting(quest.PlayingDialogIndex);
                                    return;
                                }
                            }                            
                        case QUESTSTATE.DONE:
                            continue;
                        default:
                            Debug.LogError("npc 퀘스트 마크 생성 오류");
                            return;
                    }
                }

            }
            Npc_Texting(1);
            return;
        }


    }

    void FirstDialog(NpcUnit _npc)
    {
        if (reward.gameObject.activeSelf == true)
        {
            reward.gameObject.SetActive(false);
        }
        CheckQuest(_npc);        
    }

    
   
    public void DialogTextReset()
    {
        npc_name.text = "";
        npc_dialog.text = "";
        rewards_Exp.text = "";
        if (reward_Gold.gameObject.activeSelf == true)
            reward_Gold.gameObject.SetActive(false);
        if (rewards_Item.gameObject.activeSelf == true)
            rewards_Item.gameObject.SetActive(false);
    }

    void AddChoice(Choice _addChoice)
    {
        _addChoice.transform.SetParent(scrollect.content.transform);
        choice_List.Add(_addChoice);
    }
    void AddShop()
    {
        Choice shop = choicePool.GetData();
        shop.Text.text = "상점 이용";
        shop.moveToDialogNum = -1;
        AddChoice(shop);
    }
    void AddChoice(string _choice)
    {
        string[] choiceData = _choice.Split('/');

        for (int i = 0; i < choiceData.Length; i++)
        {
            string[] subChoiceData = choiceData[i].Split('#');
            Choice choice = choicePool.GetData();
            choice.Text.text = subChoiceData[0];
            choice.moveToDialogNum = int.Parse(subChoiceData[1]);           
            AddChoice(choice);
        }
    }
   
    public void ChoiceReset()
    {        
        for (int i = 0; i < choice_List.Count; i++)
        {            
            choice_List[i].ResetChoice();
            choicePool.Add(choice_List[i]);
        }
        choice_List.Clear();
        if(reward.gameObject.activeSelf == true)
        {
            reward.gameObject.SetActive(false);
        }        
    }

    public void Npc_Texting(int _dialog_Index)
    {
        if (_dialog_Index == -2)
        {
            UIManager.uimanager.CloseDialog();
            return;
        }
        ChoiceReset();
        if (_dialog_Index == -1)
        {            
            UIManager.uimanager.OpenShopfromDialog(items);
            return;
        }
        
        List<string> dialogData = npc_DialogData[_dialog_Index-1];
        StartCoroutine(CoDialog_Texting(dialogData)); // 대화 입력
    }


    IEnumerator CoDialog_Texting(List<string> _dialogData)
    {
        string questStartIndex    = _dialogData[1];           // 퀘스트 시작 Index
        string questEndIndex      = _dialogData[2];           // 퀘스트 종료 Index
        string dialog             = _dialogData[3];           // 대화
        string choice             = _dialogData[4];           // 선택지
        string nextDialog         = _dialogData[5];           // 다음 대화
        bool npcReward               = false;

        if (!string.IsNullOrEmpty(questStartIndex))                                                                       // 퀘스트
        {
            Character.Player.quest.AddQuest(int.Parse(questStartIndex));            
            addQuestEffect.gameObject.SetActive(true);
            addQuestEffect.SetQuestEffect(true);
        }

        if (!string.IsNullOrEmpty(questEndIndex))                                                                       // 퀘스트
        {
            Character.Player.quest.QuestComplete(int.Parse(questEndIndex));
            npcReward = true;
            addQuestEffect.gameObject.SetActive(true);
            addQuestEffect.SetQuestEffect(false);
        }


        for (int i = 0; i <= dialog.Length; i++)
        {
            npc_dialog.text = dialog.Substring(0, i);

            if (i == dialog.Length)
            {
               
                if (npcReward == true)        
                {
                    Quest quest = new Quest(int.Parse(questEndIndex), QUESTSTATE.NONE);

                    reward.gameObject.SetActive(true);                    
                    rewards_Exp.text = quest.Reward_Exp + "EXP";                                             //경험치          
                    
                    if (quest.Reward_Gold != 0)                                                              
                    {
                        reward_Gold.gameObject.SetActive(true);
                        reward_Gold.text = quest.Reward_Gold + "GOLD";                                       //골드
                    }
                    else
                    {
                        reward_Gold.gameObject.SetActive(false);
                    }

                    if (quest.Reward_Item != null)                                                           //아이템
                    {
                        rewards_Item.gameObject.SetActive(true);
                        List<List<int>> rewardsItem = quest.Reward_Item;
                        string rewardsStr = string.Empty;
                        for (int items = 0; items < rewardsItem.Count; items++)
                        {
                            List<int> itemList = rewardsItem[items];
                            Item item = new Item(itemList[0]);
                            rewardsStr += item.itemName + " " + itemList[1] + " 개"+"\n";                            
                        }
                        rewards_Item.text = rewardsStr;

                    }
                    else
                    {
                        rewards_Item.gameObject.SetActive(false);
                    }
                }
                if (!string.IsNullOrEmpty(choice))                                                           // 선택지
                {
                    AddChoice(choice);
                    nextDilogNum = -3;
                }
                else                                                                                        // 선택지가 없을경우
                {
                    nextDilogNum = int.Parse(nextDialog); 
                    isDoneTexting = true;
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
}
