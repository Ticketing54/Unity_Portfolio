using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dialogue : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{    
    public Choice choice;
    public Choice ClickChoice;
    public List<Choice> Choice_list = new List<Choice>();
    public List<string> Npc_Text = new List<string>();
    public ScrollRect scrollrect;
    public GameObject NotUse;
    public TextMeshProUGUI Npc_name;
    public TextMeshProUGUI Npc_data;
    public TextMeshProUGUI Rewards;
    public TextMeshProUGUI Rewards_Exp;
    public TextMeshProUGUI Rewards_Gold;
    public TextMeshProUGUI Rewards_Item;
    

    

    public Npc npc = null;


    public void dialogreset()
    {
        Npc_name.text = "";
        Npc_data.text = "";
        Rewards_Exp.text = "";
        if (Rewards_Gold.gameObject.activeSelf == true)
            Rewards_Gold.gameObject.SetActive(false);
        if (Rewards_Item.gameObject.activeSelf == true)
            Rewards_Item.gameObject.SetActive(false);
    }



    public void OnPointerDown(PointerEventData data)
    {


        if (Input.GetMouseButtonDown(0))
        {
            for(int i = 0; i < Choice_list.Count; i++)
            {
                if(Choice_list[i].gameObject.activeSelf == true && Choice_list[i].isInRect(data.position) && npc != null)
                {

                    
                    ClickChoice = Choice_list[i];
                    ClickChoice.clickImage.gameObject.SetActive(true);

                }
            }
        }



        
    }

    public void OnPointerUp(PointerEventData data)
    {
        if(ClickChoice != null && ClickChoice.clickImage.gameObject.activeSelf == true)
            ClickChoice.clickImage.gameObject.SetActive(false);

        if (Input.GetMouseButtonUp(0))
        {
            if (ClickChoice != null &&ClickChoice.isInRect(data.position))
            {
                npc.Dialog_num = ClickChoice.num;
                Npc_Texting(npc.Dialog_num);
            }
          
        }

        ClickChoice = null;
    }
    
    public void Npc_Texting(int Dialog_num)
    {
        ChoiceReset();
        Rewards.gameObject.SetActive(false);
        if(Dialog_num == 0)
        {
            UIManager.uimanager.OpenShop(npc);
            
        }
        if(Dialog_num == -1)
        {
            npc.ExitDialog = true;
            return;
        }
        Npc_Text = npc.Dialog.GetData(Dialog_num);
        if (Npc_Text[1] != "")
        {
            QuestManager.questManager.AddQuest(int.Parse(Npc_Text[1]));
            foreach(Quest one in npc.quest_list)
            {
                if (one.Index.Equals(int.Parse(Npc_Text[1]))) 
                {

                    one.QuestComplete = 1;
                }
            }
        }
        
            
        Npc_name.text = npc.NpcName;        
        StartCoroutine(Npc_Dialog_Texting(int.Parse(Npc_Text[0]), Npc_Text[2], Npc_Text[3], Npc_Text[4],Npc_Text[5])); // 대화 입력
        

       
    }



    IEnumerator Npc_Dialog_Texting(int _Index,string _dialog,string _Choice, string _nextDialog, string _rewards)
    {

        for (int i = 0; i <= _dialog.Length; i++)
        {
            Npc_data.text = _dialog.Substring(0, i);

            if (i == _dialog.Length)
            {
                if(_rewards != "") // 경험치 골드 아이템 순
                {
                    Rewards.gameObject.SetActive(true);
                    string[] tmp = _rewards.Split('/');
                    Rewards_Exp.text = tmp[0] +"EXP"; //경험치
                    Character.Player.Stat.EXP += int.Parse(tmp[0]);
                    if(tmp[1] != "" ) //골드
                    {
                        Rewards_Gold.gameObject.SetActive(true);
                        Rewards_Gold.text =tmp[1] +"GOLD";
                        Character.Player.Stat.GOLD += int.Parse(tmp[1]);
                    }
                    else
                    {
                        Rewards_Gold.gameObject.SetActive(false);
                    }


                    if(tmp[2] != "") //아이템
                    {
                        Rewards_Item.gameObject.SetActive(true);
                        string[] item = tmp[2].Split('#');
                        UIManager.uimanager.TryOpenInventory();
                        List<string> iteminfo = ItemTableManager.instance.Item_Table.GetData(int.Parse(item[0]));
                        Item newitem = new Item(int.Parse(iteminfo[0]), int.Parse(iteminfo[1]), iteminfo[2], iteminfo[3], iteminfo[4], iteminfo[5], int.Parse(iteminfo[6]), int.Parse(iteminfo[7]));
                        if (item[1] != null) // 여러개일때
                        {
                            newitem.ItemCount = int.Parse(item[1]);
                            Rewards_Item.text = newitem.ItemName + " " + newitem.ItemCount + " 개";
                        }

                        else
                        {
                            newitem.ItemCount = 1;
                            Rewards_Item.text = newitem.ItemName ;
                        }
                            
                        
                        
                        UIManager.uimanager.Inv.getItem(newitem);
                        UIManager.uimanager.TryOpenInventory();
                    }
                    else
                    {
                        Rewards_Item.gameObject.SetActive(false);
                    }
                    npc.dialog_Done = true;
                    npc.NextDialog = int.Parse(_nextDialog);
                    npc.Dialog_num = npc.NextDialog;
                    




                }
                if (_Choice != "")  // 선택지가 있으면
                {
                    npc.NextDialog = -1;

                    string[] tmp = _Choice.Split('/');
                    for (int j = 0; j < tmp.Length; j++)
                    {
                        Choice cho;
                        string[] sdata = tmp[j].Split('#');
                        cho = ChoicePooling();
                        cho.Text.text = sdata[0];
                        cho.num = int.Parse(sdata[1]);
                        cho.gameObject.transform.SetParent(scrollrect.content.transform);
                                                
                    }                    

                }                
                else  // 선택지가 없으면
                {
                    npc.dialog_Done = true;
                    npc.NextDialog = int.Parse(_nextDialog);
                    npc.Dialog_num = npc.NextDialog;
                }
                    
            }
            
            yield return new WaitForSeconds(0.05f);
        }
    }



    public void ChoiceReset()
    {
        for(int i = 0; i < Choice_list.Count; i++)
        {
            Choice_list[i].NotUsed();
        }
        Rewards.gameObject.SetActive(false);
    }
 

    public  Choice  ChoicePooling()
    {
        foreach(Choice one in Choice_list)
        {
            if (one.gameObject.activeSelf == false)
            {
                one.gameObject.SetActive(true);
                return one;

            }
                
        }


        Choice tmp = Instantiate(choice);
        tmp.transform.localScale = new Vector3(1f, 1f, 1f);
        tmp.transform.SetParent(NotUse.transform);
        Choice_list.Add(tmp);
        return tmp;
    }

  
}
