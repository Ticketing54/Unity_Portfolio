using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class UI_Skill : MonoBehaviour , IPointerUpHandler,IPointerDownHandler,IDragHandler
{   
    [SerializeField]
    TextMeshProUGUI skillName;
    [SerializeField]
    TextMeshProUGUI skillExplain;
    [SerializeField]
    TextMeshProUGUI skillPoint;
    [SerializeField]
    TextMeshProUGUI needLev;
    

    [SerializeField]
    MoveWindow movewindow;
    [SerializeField]
    RectTransform windowRect;
    
    [SerializeField]
    GameObject dontClick;

    Character character;

    int left_WorkingSlot;
    int left_SkillIndex;
    int right_WorkingSlot;

    bool ismovePossable;
    bool clickWindow;

    [SerializeField]
    List<SkillSlot> skillslotsList;

    private void Start()
    {
        left_WorkingSlot     = -1;
        left_SkillIndex      = -1;
        right_WorkingSlot    = -1;
        clickWindow     = false;
        ismovePossable  = false;        
        UIManager.uimanager.AOpenSKill += () => gameObject.SetActive(true);
        UIManager.uimanager.ACloseSKill += () => gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    void ClickDown(Vector2 _clickPos)
    {
        ResetClickinfo();

        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < skillslotsList.Count; i++)
            {
                if (skillslotsList[i].isInRect(_clickPos))
                {

                    Skill skill = character.skill.GetSkill(i + 1);
                    if (skill != null)
                    {
                        left_SkillIndex = skill.index;
                        ismovePossable = true;
                    }

                    left_WorkingSlot = i;
                }
            }
        }


        if (Input.GetMouseButtonDown(1))
        {
            for (int i = 0; i < skillslotsList.Count; i++)
            {
                if (skillslotsList[i].isInRect(_clickPos))
                {

                    right_WorkingSlot = i;
                }
            }
        }
    }

    void ClickUp(Vector2 _clickPos)
    {
        if(left_WorkingSlot >=0 )
        {
            if (skillslotsList[left_WorkingSlot].isInRect(_clickPos))
            {
                int skillIndex = left_WorkingSlot+1;
                SkillInfoSetting(new Skill(skillIndex));
            }
            else
            {
                if(ismovePossable == true)
                {
                    int skillIndex = left_WorkingSlot + 1;
                    UIManager.uimanager.AMoveSkillQuick(skillIndex, _clickPos);
                }
            }
            
        }

        if (right_WorkingSlot>=0)
        {
            if (skillslotsList[right_WorkingSlot].isInRect(_clickPos))
            {
                int skillIndex = right_WorkingSlot + 1;
                character.skill.Add(skillIndex);
            }
        }
        ResetClickinfo();
    }
    void UpdateSkillPoint()
    {
        if(character == null)
        {
            return;
        }
        else
        {
            skillPoint.text = "스킬포인트 : "+character.stat.SkillPoint.ToString();
            UpdateAllSlot();
        }
    }


    void TryOpenSkill()
    {
        if(gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
    private void OnEnable()
    {
        character = GameManager.gameManager.character;
        UIManager.uimanager.AUpdateSkillPoint += UpdateSkillPoint;
        UIManager.uimanager.AUpdateSkillMain += UpdateSlot;
        UpdateSkillPoint();
        ResetClickinfo();        
    }

    private void OnDisable()
    {
        UIManager.uimanager.AUpdateSkillPoint -= UpdateSkillPoint;
        UIManager.uimanager.AUpdateSkillMain -= UpdateSlot;
        AllReset();
        ResetSkillInfo();
        ResetClickinfo();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        ClickDown(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ClickUp(eventData.position);
    }

    
    public void OnDrag(PointerEventData eventData)
    {
        if(left_WorkingSlot >= 0 && ismovePossable == true)
        {
            int skillIndex = left_WorkingSlot + 1;
            UIManager.uimanager.AMoveSkillIcon(skillIndex, eventData.position);
        }
    }


    void UpdateAllSlot()
    {
        if(character == null)
        {
            return;
        }

        for (int i = 0; i < skillslotsList.Count; i++)
        {
            UpdateSlot(i);           
        }        
    }
    void UpdateSlot(int _slotIndex)
    {
        int skillIndex = _slotIndex + 1;
        Skill skill = new Skill(skillIndex);

        if (character.skill.GetSkill(skillIndex) != null)
        {
            skillslotsList[_slotIndex].Add(skill.spriteName);
            skillslotsList[_slotIndex].SetHaveSkill();
        }
        else
        {
            if (skill.needLevel <= character.stat.Level && character.stat.SkillPoint > 0)
            {
                skillslotsList[_slotIndex].Add(skill.spriteName);
                skillslotsList[_slotIndex].Setlearn();
            }
            else
            {
                skillslotsList[_slotIndex].Add(skill.spriteName);
                skillslotsList[_slotIndex].SetCantLearn();
            }
        }
    }
    void AllReset()
    {
        for (int i = 0; i < skillslotsList.Count; i++)
        {
            skillslotsList[i].Clear();
        }
    }
    void ResetClickinfo()
    {
        right_WorkingSlot   = -1;
        left_WorkingSlot    = -1;
        left_SkillIndex     = -1;
        ismovePossable      = false;
    }
    void ResetSkillInfo()
    {
        skillName.text = "";
        skillExplain.text = "";
        needLev.text = "";
    }
    void SkillInfoSetting(Skill _skill)
    {
        skillName.text = _skill.name;
        skillExplain.text = _skill.explain;

        if(character.stat.Level < _skill.needLevel)
        {
            needLev.text = _skill.needLevel.ToString();
            needLev.color = Color.red;
        }
        else
        {
            needLev.text = _skill.needLevel.ToString();
            needLev.color = Color.white;
        }
        
    }

}
