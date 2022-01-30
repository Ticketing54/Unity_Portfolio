using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;


public class Npc : MonoBehaviour
{
    public string NpcName = string.Empty;
    public int index;
    public ResourceManager Dialog = new ResourceManager();
    public List<Item> item_list = new List<Item>();
    public List<Quest> quest_list = new List<Quest>();
    float Dis;
    public int Dialog_num = 0;
    public int NextDialog = 0;
    public bool dialog_Done = false;
    public bool ExitDialog = false;
    public Vector3 startPos = Vector3.zero;

    float turnspeed = 10f;
    public float QuestMarkerNum = -1;



    public float Nick_y = 0;
    public NavMeshAgent nav;
    public TextMeshProUGUI NickName;
    public TextMeshProUGUI NpcTalk;
    public Image MiniMap_Dot;
    public Image MiniMap_Dot_M;
    public GameObject QuestMark;


    public float DISTANCE
    {
        get
        {
            Dis = Vector3.Distance(Character.Player.transform.position, this.transform.position);
            return Dis;
        }
    }

    private void Start()
    {
        nav = this.GetComponent<NavMeshAgent>();
        if (nav != null)
            nav.Warp(startPos);



        StartCoroutine(NickUpdate());
        StartCoroutine(Mini_DotMove());
        StartCoroutine(Mini_Dot_MMove());

    }
    IEnumerator NickUpdate()
    {

        while (true)
        {
            if (Character.Player != null && Camera.main !=null)
            {
                if (DISTANCE < 4f && NickName == null)
                {
                    NickName = ObjectPoolManager.objManager.PoolingNickName();
                    NickName.text = NpcName;
                }                    
                if (DISTANCE < 4f && NickName != null || Character.Player.Target ==this.gameObject)
                {
                    if(NickName == null)
                    {
                        NickName = ObjectPoolManager.objManager.PoolingNickName();
                        NickName.text = NpcName;
                    }
                    NickName.color = Color.white;
                    NickName.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, Nick_y, 0f));
                }
                else
                {

                    if (NickName != null)
                    {
                        NickName.gameObject.SetActive(false);
                        NickName = null;
                    }
                }

                if(NpcTalk != null)
                {
                    NpcTalk.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, Nick_y+0.3f, 0f));
                }

                
            }


            



            yield return null;


        }

    }
    private void Update()
    {
        
        QuestMarkControl();
    }




  
    IEnumerator Mini_DotMove()
    {
        while (true)
        {
            if (UIManager.uimanager.minimap.Minimap_n.gameObject.activeSelf == true && Character.Player != null)
            {
                if (DISTANCE <= 20f && MiniMap_Dot == null )
                {

                    MiniMap_Dot = ObjectPoolManager.objManager.PoolingMiniDot_n();
                    MiniMap_Dot.sprite = GameManager.gameManager.resource.GetImage("Dot_N");

                }
                else if (DISTANCE <= 20f && MiniMap_Dot != null)
                {

                    MiniMap_Dot.rectTransform.anchoredPosition = UIManager.uimanager.minimap.MoveDotPosition(transform.position);

                }
                else if ((DISTANCE > 20f && MiniMap_Dot != null) )
                {
                    MiniMap_Dot.gameObject.SetActive(false);
                    MiniMap_Dot = null;
                }




            }

            yield return null;
        }
    }

    IEnumerator Mini_Dot_MMove()
    {
        while (true)
        {
            if (UIManager.uimanager.minimap.MiniMap_MActive == true && Character.Player != null)
            {               
                if(MiniMap_Dot_M == null)
                {
                    MiniMap_Dot_M = ObjectPoolManager.objManager.PoolingMiniDot_M();
                    MiniMap_Dot_M.sprite = GameManager.gameManager.resource.GetImage("Dot_N");
                }
                else
                {
                    MiniMap_Dot_M.rectTransform.anchoredPosition = UIManager.uimanager.minimap.MoveDotPosition(transform.position,700);
                }

            }
            else
            {
                if(MiniMap_Dot_M !=null)
                {
                    MiniMap_Dot_M.gameObject.SetActive(false);
                    MiniMap_Dot_M = null;
                }
                
            }

            yield return null;
        }
    }

    public void QuestMarkControl()
    {
        

        if (QuestMarkerNum == 2)
        {
            if (QuestMark == null ||QuestMark.name != "Clear")
            {
                if (QuestMark != null)
                    QuestMark.gameObject.SetActive(false);
                QuestMark = ObjectPoolManager.objManager.QuestMarkPooling("Clear");
                QuestMark.gameObject.SetActive(true);
            }
           
            QuestMark.transform.position = new Vector3(transform.position.x, Nick_y + 0.5f, transform.position.z);
            QuestMark.transform.Rotate(Vector3.up, turnspeed * Time.deltaTime);

        }
        else if (QuestMarkerNum == 0)
        {
            if (QuestMark == null || QuestMark.name != "Quest")
            {
                if(QuestMark != null)
                    QuestMark.gameObject.SetActive(false);

                QuestMark = ObjectPoolManager.objManager.QuestMarkPooling("Quest");
                QuestMark.gameObject.SetActive(true);

            }
            
            QuestMark.transform.position = new Vector3(transform.position.x, Nick_y+0.5f, transform.position.z);
            QuestMark.transform.Rotate(Vector3.up, turnspeed * Time.deltaTime);
                

            

        }       
        else if (QuestMarkerNum == 1)
        {
            if (QuestMark == null || QuestMark.name != "NoClear")
            {
                if (QuestMark != null)
                    QuestMark.gameObject.SetActive(false);
                QuestMark = ObjectPoolManager.objManager.QuestMarkPooling("NoClear");
                QuestMark.gameObject.SetActive(true);
            }
           
            QuestMark.transform.position = new Vector3(transform.position.x, Nick_y + 0.5f, transform.position.z);
            QuestMark.transform.Rotate(Vector3.up, turnspeed * Time.deltaTime);




        }
        else
        {
            if (QuestMark != null)
            {
                QuestMark.gameObject.SetActive(false);
                QuestMark = null;
            }
                

        }
    }
    public void npctalk(int _num)
    {
        StartCoroutine(npcTalkdialog(_num));
    }
    IEnumerator npcTalkdialog(int _num)
    {
        if(NpcTalk != null)
        {
            //List<string> talk_list = Dialog.GetData(_num);
            string talk = "";
            for( int i = 0; i<= talk.Length; i++)
            {
                NpcTalk.text = talk.Substring(0, i);
                if (i == talk.Length)
                {
                    NpcTalk.text = "";
                    dialog_Done = true;
                    yield break;
                        
                }





                yield return new WaitForSeconds(0.1f);
            }




        }
    }
}
