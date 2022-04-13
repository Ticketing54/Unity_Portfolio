using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;


public class Npc :NpcUnit
{   
    
    protected NavMeshAgent nav;   

    public void SetNpc(int _index,string _npcName, float _nickYPos,List<int> _quests,List<int> _items,string _dialogue)
    {
        npcIndex = _index;
        unitName = _npcName;        
        nick_YPos = _nickYPos;        
        items = _items;                 
        dialogue = _dialogue;
        quests = _quests;
        SetQuestMark();
    }

    public void SetQuestMark()
    {
        if (quests == null)
        {
            return;
        }
        else
        {
            
            for (int i = 0; i < quests.Count; i++)
            {
                Quest quest = Character.Player.quest.GetQuest(quests[i]);
                if(quest == null)
                {
                    List<string> questTable = ResourceManager.resource.GetTable_Index("QuestTable", quests[i]);
                    int startNpcIndex ;

                    if(int.TryParse(questTable[8],out startNpcIndex))
                    {
                        if (Character.Player.quest.ClearPrecedQuest(quests[i]) && startNpcIndex == npcIndex)
                        {
                            EffectManager.effectManager.UpdateQuestMark(this, QUESTMARKTYPE.EXCLAMATION, QUESTSTATE.NONE);
                            return;
                        }
                    }
                                                        
                }
                else
                {
                    switch (quest.State)
                    {
                        case QUESTSTATE.PLAYING:
                            EffectManager.effectManager.UpdateQuestMark(this, QUESTMARKTYPE.QUESTION, QUESTSTATE.PLAYING);
                            return;
                        case QUESTSTATE.COMPLETE:
                            EffectManager.effectManager.UpdateQuestMark(this, QUESTMARKTYPE.QUESTION, QUESTSTATE.COMPLETE);
                            return;
                        case QUESTSTATE.DONE:
                            continue;
                        default:
                            Debug.LogError("npc 퀘스트 마크 생성 오류");
                            break;
                    }
                }
                
            }
        }            
    }
    public bool HasQuest(int _questIndex)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            if(quests[i] == _questIndex)
            {
                return true;
            }
        }
        return false;
    }
    private void OnEnable()
    {
        
    }
    public virtual void Start()
    {
        UIManager.uimanager.uiEffectManager.ActiveNpcUi(this);
        nav = this.GetComponent<NavMeshAgent>();        
        //if (nav != null)
        //    nav.Warp(startPos);
        //StartCoroutine(NickUpdate());
        //StartCoroutine(Mini_DotMove());
        //StartCoroutine(Mini_Dot_MMove());
    }
    IEnumerator ApprochChracter()
    {
        yield return null;

        while (true)
        {

        }
    }
    
    //private void Update()
    //{

    //    QuestMarkControl();
    //}





    //IEnumerator Mini_DotMove()
    //{
    //    while (true)
    //    {
    //        if (UIManager.uimanager.minimap.Minimap_n.gameObject.activeSelf == true && Character.Player != null)
    //        {
    //            if (DISTANCE <= 20f && MiniMap_Dot == null )
    //            {

    //                MiniMap_Dot = ObjectPoolManager.objManager.PoolingMiniDot_n();
    //                MiniMap_Dot.sprite = ResourceManager.resource.GetImage("Dot_N");

    //            }
    //            else if (DISTANCE <= 20f && MiniMap_Dot != null)
    //            {

    //                MiniMap_Dot.rectTransform.anchoredPosition = UIManager.uimanager.minimap.MoveDotPosition(transform.position);

    //            }
    //            else if ((DISTANCE > 20f && MiniMap_Dot != null) )
    //            {
    //                MiniMap_Dot.gameObject.SetActive(false);
    //                MiniMap_Dot = null;
    //            }




    //        }

    //        yield return null;
    //    }
    //}

    //IEnumerator Mini_Dot_MMove()
    //{
    //    while (true)
    //    {
    //        if (UIManager.uimanager.minimap.MiniMap_MActive == true && Character.Player != null)
    //        {               
    //            if(MiniMap_Dot_M == null)
    //            {
    //                MiniMap_Dot_M = ObjectPoolManager.objManager.PoolingMiniDot_M();
    //                MiniMap_Dot_M.sprite = ResourceManager.resource.GetImage("Dot_N");
    //            }
    //            else
    //            {
    //                MiniMap_Dot_M.rectTransform.anchoredPosition = UIManager.uimanager.minimap.MoveDotPosition(transform.position,700);
    //            }

    //        }
    //        else
    //        {
    //            if(MiniMap_Dot_M !=null)
    //            {
    //                MiniMap_Dot_M.gameObject.SetActive(false);
    //                MiniMap_Dot_M = null;
    //            }

    //        }

    //        yield return null;
    //    }
    //}

    //public void QuestMarkControl()
    //{


    //    if (QuestMarkerNum == 2)
    //    {
    //        if (QuestMark == null ||QuestMark.name != "Clear")
    //        {
    //            if (QuestMark != null)
    //                QuestMark.gameObject.SetActive(false);
    //            QuestMark = ObjectPoolManager.objManager.QuestMarkPooling("Clear");
    //            QuestMark.gameObject.SetActive(true);
    //        }

    //        QuestMark.transform.position = new Vector3(transform.position.x, Nick_y + 0.5f, transform.position.z);
    //        QuestMark.transform.Rotate(Vector3.up, turnspeed * Time.deltaTime);

    //    }
    //    else if (QuestMarkerNum == 0)
    //    {
    //        if (QuestMark == null || QuestMark.name != "Quest")
    //        {
    //            if(QuestMark != null)
    //                QuestMark.gameObject.SetActive(false);

    //            QuestMark = ObjectPoolManager.objManager.QuestMarkPooling("Quest");
    //            QuestMark.gameObject.SetActive(true);

    //        }

    //        QuestMark.transform.position = new Vector3(transform.position.x, Nick_y+0.5f, transform.position.z);
    //        QuestMark.transform.Rotate(Vector3.up, turnspeed * Time.deltaTime);




    //    }       
    //    else if (QuestMarkerNum == 1)
    //    {
    //        if (QuestMark == null || QuestMark.name != "NoClear")
    //        {
    //            if (QuestMark != null)
    //                QuestMark.gameObject.SetActive(false);
    //            QuestMark = ObjectPoolManager.objManager.QuestMarkPooling("NoClear");
    //            QuestMark.gameObject.SetActive(true);
    //        }

    //        QuestMark.transform.position = new Vector3(transform.position.x, Nick_y + 0.5f, transform.position.z);
    //        QuestMark.transform.Rotate(Vector3.up, turnspeed * Time.deltaTime);




    //    }
    //    else
    //    {
    //        if (QuestMark != null)
    //        {
    //            QuestMark.gameObject.SetActive(false);
    //            QuestMark = null;
    //        }


    //    }
    //}
    //public void npctalk(int _num)
    //{
    //    StartCoroutine(npcTalkdialog(_num));
    //}
    //IEnumerator npcTalkdialog(int _num)
    //{
    //    if(NpcTalk != null)
    //    {
    //        //List<string> talk_list = Dialog.GetData(_num);
    //        string talk = "";
    //        for( int i = 0; i<= talk.Length; i++)
    //        {
    //            NpcTalk.text = talk.Substring(0, i);
    //            if (i == talk.Length)
    //            {
    //                NpcTalk.text = "";
    //                dialog_Done = true;
    //                yield break;

    //            }





    //            yield return new WaitForSeconds(0.1f);
    //        }




    //    }
    //}
}
