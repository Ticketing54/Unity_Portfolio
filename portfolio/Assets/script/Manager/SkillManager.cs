using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour // IPointerUpHandler,IPointerDownHandler,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    
    public static SkillManager skillmanager;
    public TextMeshProUGUI SkillName;
    public TextMeshProUGUI SkillEx;
    public TextMeshProUGUI SkillPoint;
    public TextMeshProUGUI NeedLev;    

    public List<SkillSlot> list = new List<SkillSlot>();
    public List<SkillSlot> q_list = new List<SkillSlot>();
  
    private void Awake()
    {
        if (skillmanager== null)
        {
            skillmanager = this;            
        }        
    }
    int WorkingSlot = -1;
    public Vector2 OldClickPos;
    Skill ClickSkill;
    public Image MoveIcon;

    public GameObject skillobj;

    public bool Skillinfo = false; 
    public bool isQuick = false;     //퀵슬롯에서 시작했는가
    public bool isDrag = false;
    public bool RClick = false;

    public GameObject DontClick;


    public MoveWindow movewindow;                   //윈도우창 움직이기
    public RectTransform Window;                    //움직이는 대상
    public bool WindowDrag = false;                 //윈도우 바를 클릭했을때
    public Vector2 Window_Preset = Vector2.zero;
    //public void OnPointerDown(PointerEventData data)
    //{


    //    Debug.Log(data.position);
    //    OldClickPos = data.position; // 클릭 포인트        
    //    if (Input.GetMouseButton(0))
    //    {
    //        if (movewindow.isInRect(data.position))
    //        {
    //            WindowDrag = true;
    //            Window_Preset = data.position - (Vector2)Window.position;

    //        }

    //        for (int i = 0; i < list.Count; i++)
    //        {
    //            if (list[i].isInRect(data.position))
    //            {
    //                WorkingSlot = i;
    //                if (list[i].isactive == true)
    //                {
    //                    MoveIcon.gameObject.SetActive(true);
    //                    MoveIcon.sprite =  list[i].skillImage.sprite;
    //                    MoveIcon.transform.position = data.position;
    //                    isDrag = true;
    //                }
    //                ClickSkill = list[i].skill;
    //                OldClickPos = data.position;                    
    //                Skillinfo = true;
                    
    //                return;
    //            }
    //        }



    //        for (int j = 0; j < q_list.Count; j++)
    //        {
    //            if (q_list[j].isInRect(data.position) && q_list[j].skillImage.gameObject.activeSelf == true)
    //            {
    //                ClickSkill = q_list[j].skill;                 
    //                WorkingSlot = j;
    //                MoveIcon.gameObject.SetActive(true);
    //                MoveIcon.sprite = q_list[j].skillImage.sprite;
    //                MoveIcon.transform.position = data.position;
    //                OldClickPos = data.position;
    //                Skillinfo = true;
    //                isQuick = true;
    //                isDrag = true;
    //                return;
    //            }
    //        }

    //    }
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        for (int i = 0; i < list.Count; i++)
    //        {
    //            if (list[i].isInRect(data.position) && Character.Player.Stat.SkillPoint >= 1 && list[i].PosLev_Image.gameObject.activeSelf==true) 
    //            {

    //                OldClickPos = data.position;
    //                WorkingSlot = i;
    //                RClick = true;
    //                return;
    //            }

    //        }

            

    //    }

    //}
    //public void OnPointerUp(PointerEventData data)
    //{
    //    WindowDrag = false;



    //    if (Input.GetMouseButtonUp(0) && OldClickPos == data.position && Skillinfo == true)   //좌클릭시
    //    {
    //        if (WorkingSlot < 0)
    //            return;
           
    //        if(isQuick == true) // 아직미정
    //        {
    //            Vector3 Pos;
    //            Pos = new Vector3(data.position.x + 75f, data.position.y - 100f, 0);

    //            Skillinfo = false;
    //            isQuick = false;
    //            isDrag = false;
    //            WorkingSlot = -1;
                
    //        }
    //        else //스킬창에서의 좌클릭
    //        {
    //            Skill_Info(ClickSkill);
    //            MoveIcon.gameObject.SetActive(false);
    //            ClickSkill = null;                
    //            Skillinfo = false;
    //            isDrag = false;
    //            WorkingSlot = -1;

    //        }


    //        return;

            

    //    }
    //    if (Input.GetMouseButtonUp(1) && OldClickPos == data.position && Character.Player.Stat.SkillPoint>=1 && RClick == true ) // 우 클릭 시 // 스킬습득
    //    {
    //        Character.Player.Stat.SkillPoint--;
    //        list[WorkingSlot].isactive = true;
    //        list[WorkingSlot].Possible_Image.gameObject.SetActive(false);
    //        WorkingSlot = -1;
    //        RClick = false;
    //        return;



    //    }

    //    RClick = false;
    //    Skillinfo = false;   //클릭이 아닐 때

    
    //    if (Input.GetMouseButtonUp(0))
    //    {
    //        if(isQuick == true)
    //        {
    //            isQuick = false;
    //            for (int i = 0; i < q_list.Count; i++)
    //            {
    //                if (q_list[i].isInRect(data.position)  && q_list[i].skillImage.gameObject.activeSelf == true && isDrag == true) //다른 스킬이 있을때
    //                {
    //                    MoveIcon.gameObject.SetActive(false);
    //                    isDrag = false;
    //                    q_list[WorkingSlot].skill = q_list[i].skill;
    //                    q_list[WorkingSlot].skillImage.sprite = q_list[i].skillImage.sprite;
    //                    WorkingSlot = -1;
    //                    q_list[i].skill = ClickSkill;
    //                    q_list[i].skillImage.gameObject.SetActive(true);
    //                    q_list[i].skillImage.sprite = GameManager.gameManager.GetSprite(q_list[i].skill.skillSpriteName);
    //                    ClickSkill = null;
                        

    //                }
    //                else if(q_list[i].isInRect(data.position) && q_list[i].skillImage.gameObject.activeSelf == false && isDrag == true) // 다른 스킬이 없을때
    //                {
    //                    MoveIcon.gameObject.SetActive(false);
    //                    isDrag = false;
    //                    q_list[WorkingSlot].SlotClear();
    //                    q_list[i].skill = ClickSkill;
    //                    q_list[i].skillImage.gameObject.SetActive(true);
    //                    q_list[i].skillImage.sprite = GameManager.gameManager.GetSprite(q_list[i].skill.skillSpriteName);
    //                    ClickSkill = null;
    //                    WorkingSlot = -1;
                        


    //                }
                  
                   
    //            }
    //            if (WorkingSlot >= 0 && MoveIcon.gameObject.activeSelf == true)
    //            {
    //                ClickSkill = null;
    //                MoveIcon.gameObject.SetActive(false);
    //                return;

    //            }
    //        }
    //        else
    //        {
    //            for (int i = 0; i < q_list.Count; i++)
    //            {
    //                if (q_list[i].isInRect(data.position) && isDrag == true) 
    //                {
    //                    for(int j = 0; j < q_list.Count; j++)
    //                    {
    //                        if (q_list[j].skill != null&&q_list[j].skill.Index == ClickSkill.Index)
    //                        {
    //                            q_list[j].SlotClear();
    //                        }

                
    //                    }
    //                    MoveIcon.gameObject.SetActive(false);
    //                    isDrag = false;                        
    //                    WorkingSlot = -1;
    //                    q_list[i].skill = ClickSkill;
    //                    q_list[i].skillImage.gameObject.SetActive(true);
    //                    q_list[i].skillImage.sprite = GameManager.gameManager.GetSprite(q_list[i].skill.skillSpriteName);
    //                    ClickSkill = null;

    //                }                    
                    

    //            }
    //            if (WorkingSlot >= 0 && MoveIcon.gameObject.activeSelf == true)
    //            {
    //                ClickSkill = null;
    //                MoveIcon.gameObject.SetActive(false);
    //                return;

    //            }
    //        }


            
           
    //    }
    //}
    //public void OnDrag(PointerEventData data)
    //{
    //    MoveIcon.rectTransform.position = data.position;
    //    if (WindowDrag == true)
    //    {

    //        Window.position = data.position - Window_Preset;
    //    }
    //}
    

    //public void ApplySkill()
    //{
    //    skillobj.SetActive(true);
                
    //    for (int i = 0; i < list.Count; i++)
    //    {
    //        List<string> skillinfo = SkillTableManager.instance.skill_Table.GetData(i);
    //        list[i].Addskill(new Skill(int.Parse(skillinfo[0]), skillinfo[1], skillinfo[2], skillinfo[3], skillinfo[4], skillinfo[5], skillinfo[6], skillinfo[7], skillinfo[8], skillinfo[9]));
    //    }
    //    skillobj.SetActive(false);
    //}

    //public void UpdateSkill()
    //{        
    //    for(int i = 0; i < list.Count; i++)
    //    {
    //        if(list[i].skill.needLevel <= Character.Player.Stat.LEVEL && list[i].skill.needLevel != 0 &&list[i].isactive == false)
    //        {
    //            list[i].PosLev_Image.gameObject.SetActive(true);
    //        }
    //        else if (list[i].skill.needLevel <= Character.Player.Stat.LEVEL && list[i].skill.needLevel != 0 && list[i].isactive == true)
    //        {
    //            list[i].PosLev_Image.gameObject.SetActive(false);
    //        }
    //    }
    //}
    //public void Skill_Info(Skill skill)
    //{        
    //    SkillName.text = skill.skillName;
    //    if(Character.Player.Stat.LEVEL < skill.needLevel)
    //    {
    //        NeedLev.text = "필요레벨 : "+skill.needLevel;
    //        NeedLev.color = Color.red;
    //    }
    //    else
    //    {
    //        NeedLev.text = "필요레벨 : " + skill.needLevel;
    //        NeedLev.color = Color.white;
    //    }
    //    SkillEx.text = "종류 : " + skill.skillType + '\n' + "Mp 소모량 : " + skill.Mana + '\n' + skill.skillExplain;        
    //}
  
    //private void Update()
    //{
    //    if(skillobj.activeSelf == true)
    //    {
    //        SkillPoint.text = "스킬포인트 : " + Character.Player.Stat.SkillPoint.ToString();
    //        UpdateSkill();
    //    }
        
    //}



    

    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    DontClick.gameObject.SetActive(true);
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    DontClick.gameObject.SetActive(false);
        
    //}
}
