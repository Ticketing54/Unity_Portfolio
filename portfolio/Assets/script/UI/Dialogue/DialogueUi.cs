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
    List<int> quests;
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
        quests = null;
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
            quests = _npc.QUEST;
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
        FirstDialog();
    }

    void QuestStateToChoice()
    {
        if (quests == null)
            return;

        List<string> questToChoice = new List<string>();
        string[] arrQuesttoChoice = npc_DialogData[0][3].Split('/');
        for (int i = 0; i < quests.Count; i++)
        {
            string[] arrchoiceData = arrQuesttoChoice[i].Split('#');    // 퀘스트받기전 # 퀘스트 진행중(미완료) # 퀘스트 진행중(완료시) # 말
            QUESTSTATE state = Character.Player.quest.ChracterState_Quest(quests[i]);
            Choice choice = choicePool.GetData();
            choice.transform.SetParent(scrollect.content.transform);
            choice.Text.text = arrchoiceData[3];
            switch (state)
            {
                case QUESTSTATE.READY:
                    {
                        choice.moveToDialogNum = int.Parse(arrchoiceData[0]);
                        choice_List.Add(choice);
                        break;
                    }
                case QUESTSTATE.PLAYING:
                    {
                        choice.moveToDialogNum = int.Parse(arrchoiceData[1]);
                        choice_List.Add(choice);
                        break;
                    }
                case QUESTSTATE.COMPLETE:
                    {
                        choice.moveToDialogNum = int.Parse(arrchoiceData[2]);
                        choice_List.Add(choice);
                        break;
                    }
                default:
                    {
                        choice.ResetChoice();
                        choicePool.Add(choice);
                        break;
                    }

            }

        }
    }

    void FirstDialog()
    {                                                                        
        QuestStateToChoice();
        List<string> subDialogData = npc_DialogData[1];
        if (items !=null)
        {
            AddShop();
        }
        if (subDialogData[3] != "")
        {
            AddChoice(subDialogData[3]);
        }
        npc_dialog.text = subDialogData[2];

        if(reward.gameObject.activeSelf == true)
        {
            reward.gameObject.SetActive(false);
        }
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
        
        List<string> dialogData = npc_DialogData[_dialog_Index];
        StartCoroutine(CoDialog_Texting(dialogData)); // 대화 입력
    }


    IEnumerator CoDialog_Texting(List<string> _dialogData)
    {
        string quest        = _dialogData[1];           // 퀘스트
        string dialog       = _dialogData[2];           // 대화
        string choice       = _dialogData[3];           // 선택지
        string nextDialog   = _dialogData[4];           // 다음 대화
        string rewards      = _dialogData[5];                                                                   // 보상

        if (!string.IsNullOrEmpty(quest))                                                                       // 퀘스트
        {
            Character.Player.quest.AddQuest(int.Parse(quest));            
            addQuestEffect.gameObject.SetActive(true);
        }


        for (int i = 0; i <= dialog.Length; i++)
        {
            npc_dialog.text = dialog.Substring(0, i);

            if (i == dialog.Length)
            {
               
                if (!string.IsNullOrEmpty(rewards))                                                             // 보상
                {
                    reward.gameObject.SetActive(true);
                    string[] tmp = rewards.Split('/');
                    rewards_Exp.text = tmp[0] + "EXP"; //경험치
                    Character.Player.stat.EXP += int.Parse(tmp[0]);
                    if (!string.IsNullOrEmpty(tmp[1]))                                                          //골드
                    {
                        reward_Gold.gameObject.SetActive(true);
                        reward_Gold.text = tmp[1] + "GOLD";
                        Character.Player.inven.gold += int.Parse(tmp[1]);
                    }
                    else
                    {
                        reward_Gold.gameObject.SetActive(false);
                    }

                    if (!string.IsNullOrEmpty(tmp[2]))                                                          //아이템
                    {
                        rewards_Item.gameObject.SetActive(true);
                        string[] item = tmp[2].Split('#');                                  
                        
                        Item newitem = new Item(int.Parse(item[0]));                      // 아이템 획득
                        Character.Player.inven.PushItem(newitem);

                        if (item[1] != null)                                                                   // 여러개일때
                        {
                            newitem.ItemCount = int.Parse(item[1]);
                            rewards_Item.text = newitem.itemName + " " + newitem.ItemCount + " 개";
                        }

                        else
                        {
                            newitem.ItemCount = 1;
                            rewards_Item.text = newitem.itemName;
                        }
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
