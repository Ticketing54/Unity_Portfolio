using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
public class UIManager : MonoBehaviour
{
    public static UIManager uimanager;
    
    [SerializeField]
    GameObject baseUi;

    private void Awake()
    {
        if (uimanager == null)
        {
            uimanager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        KeyboardSortCut = new Dictionary<KeyCode, Action>();
    }

    private void Update()
    {
        Inputkeyboard();
    }
    #region KeyboardSortCut
    Dictionary<KeyCode, Action> KeyboardSortCut;




    public void AddKeyBoardSortCut(KeyCode _keycode, Action _action)
    {
        if (KeyboardSortCut.ContainsKey(_keycode))
        {
            KeyboardSortCut.Remove(_keycode);
            KeyboardSortCut.Add(_keycode, _action);
        }
        else
        {
            KeyboardSortCut.Add(_keycode, _action);
        }
    }
    public void RemoveKeyBoardSortCut(KeyCode _keycode)
    {
        if (KeyboardSortCut.ContainsKey(_keycode))
        {
            KeyboardSortCut.Remove(_keycode);
        }
        else
        {
            Debug.Log("없는 단축키를 없애려 합니다.");
        }
    }

    void Inputkeyboard()
    {
        if (Input.anyKey)
        {
            foreach (KeyValuePair<KeyCode, Action> input in KeyboardSortCut)
            {
                if (Input.GetKeyDown(input.Key))
                {
                    input.Value();
                }
            }           
        }
    }
    #endregion
   
    public void OnBaseUI()
    {
        baseUi.SetActive(true);        
    }
    public void OffBaseUI()
    {
        baseUi.SetActive(false);        
    }

    #region StatusUi

    Action<int> aGetGoldUpdateUi;
    Action<int> aGetExpUpdateUi;
    Action aUpdateHp;
    Action aUpdateMp;
    Action aUpdateAtk;
    Action aUpdateExp;
    Action aUpdateLevel;
    Action aUpdateSkillPoint;


    public Action<int> AGetGoldUpdateUi
    {
        get
        {
            if (aGetGoldUpdateUi == null)
            {
                return (one) => { Debug.Log("GoldUpdateUi가 없습니다."); };
            }
            else
            {
                return aGetGoldUpdateUi;
            }
            
        }
        set
        {
            aGetGoldUpdateUi = value;
        }
    }
    public Action<int> AGetExpUpdateUi
    {
        get
        {
            if (aGetExpUpdateUi == null)
            {
                return (one) => { Debug.Log("aGetExpUpdateUi가 없습니다."); };
            }
            else
            {
                return aGetExpUpdateUi;
            }

        }
        set
        {
            aGetExpUpdateUi = value;
        }
    }
    public Action AUpdateHp
    {
        get
        {
            if (aUpdateHp == null)
            {
                return () => { Debug.Log("aUpdateHp가 없습니다."); };
            }
            else
            {
                return aUpdateHp;
            }

        }
        set
        {
            aUpdateHp = value;
        }
    }
    public Action AUpdateMp
    {
        get
        {
            if (aUpdateMp == null)
            {
                return () => { Debug.Log("aUpdateMp 가 없습니다."); };
            }
            else
            {
                return aUpdateMp;
            }

        }
        set
        {
            aUpdateMp = value;
        }
    }
    public Action AUpdateAtk
    {
        get
        {
            if (aUpdateAtk == null)
            {
                return () => { Debug.Log("aUpdateAtk 가 없습니다."); };
            }
            else
            {
                return aUpdateAtk;
            }

        }
        set
        {
            aUpdateAtk = value;
        }
    }
    public Action AUpdateExp
    {
        get
        {
            if (aUpdateExp == null)
            {
                return () => { Debug.Log("aUpdateExp 가 없습니다."); };
            }
            else
            {
                return aUpdateExp;
            }

        }
        set
        {
            aUpdateExp = value;
        }
    }
    public Action AUpdateLevel
    {
        get
        {
            if (aUpdateLevel == null)
            {
                return () => { Debug.Log("aUpdateLevel 가 없습니다."); };
            }
            else
            {
                return aUpdateLevel;
            }

        }
        set
        {
            aUpdateLevel = value;
        }
    }
    public Action AUpdateSkillPoint
    {
        get
        {
            if (aUpdateSkillPoint == null)
            {
                return () => { Debug.Log("aUpdateSkillPoint 가 없습니다."); };
            }
            else
            {
                return aUpdateSkillPoint;
            }

        }
        set
        {
            aUpdateSkillPoint = value;
        }
    }
    #endregion


    #region QuestEffect

    [SerializeField]
    AddQuestText_Ui questAddEffect;

    public void OnQuestEffect(QUESTSTATE _state)
    {
        questAddEffect.gameObject.SetActive(true);
        questAddEffect.SetQuestEffect(_state);
    }
    #endregion

    #region Middle_Top_HpBar    
    [SerializeField]
    TopEnermyInfoUi topEnermyInfoUi;
    public void Open_Top_EnermyInfo(Monster _Monster)              // 상단 중앙 적정보 UI
    {
        topEnermyInfoUi.gameObject.SetActive(true);
        topEnermyInfoUi.Top_EnermyInfoUi(_Monster);
    }
    #endregion

   
    #region DropBox

    public Action AOpenDropBox;
    public Action<int> OpenDropBoxCountMessage;
    public Action DropBoxUpdate;

    #endregion

    #region MiniInfo
    public Action<int, Vector2> OpenMiniInfo;
    public Action CloseMiniInfo;
    #endregion

    #region ClickMove    
    [SerializeField]
    Image DontClick;                // 드래그 시 클릭방지

    Action<ITEMLISTTYPE,int,Vector2> itemClickUp;
    Action<ITEMLISTTYPE, int> itemUpdateSlot;
    Action<ITEMLISTTYPE, int,Vector2> moveItemIcon;
    public Action<ITEMLISTTYPE, int, Vector2> ItemClickUp
    {
        get
        {
            if (itemClickUp == null)
            {
                return (type, index, pos) =>
                {

                };
            }
            else
            {
                return itemClickUp;
            }
        }
        set
        {
            itemClickUp = value;
        }
    }
    public Action<ITEMLISTTYPE, int> ItemUpdateSlot
    {
        get
        {
            if(itemUpdateSlot == null)
            {
                return (type, index) => { Debug.Log(type + " 의 " + index + " 번의 슬롯이 없습니다."); };
            }
            else
            {
                return itemUpdateSlot;
            }
        }
        set
        {
            itemUpdateSlot = value;
        }
    }
    public Action<ITEMLISTTYPE,int,Vector2> MoveItemIcon { get => moveItemIcon; set => moveItemIcon = value; }

    public void ItemClickEnd(ITEMLISTTYPE _startType,int _startIndex, Vector2 _pos)
    {
        ItemClickUp(_startType, _startIndex, _pos);
        ItemUpdateSlot(_startType, _startIndex);
    }
   
    #endregion

    #region Fade
    [SerializeField]
    Image fadeInout;

    public delegate void FadeInTurnOnOffUi();                       // Fade in ->UiOnOff ->Fade out
    
   
    public void FadeInOut(FadeInTurnOnOffUi _middleFuc)
    {
        
        StartCoroutine(CoFade(_middleFuc));
    }
    
    IEnumerator CoFade(FadeInTurnOnOffUi _middleFuc)
    {
        fadeInout.gameObject.SetActive(true);
        
        Color controlColor = fadeInout.color;
        controlColor.a = 0;
        fadeInout.color = controlColor;
        float timer = 0;
        bool isFadein = true;
        while(true)
        {
            yield return null;
            timer += Time.unscaledDeltaTime ;

            controlColor.a = isFadein ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
            fadeInout.color = controlColor;

            if(isFadein == true && controlColor.a >= 1)
            {
                _middleFuc();                               /// 어두워졌을때 작업
                isFadein = false;
                timer = 0;
            }

            if(isFadein == false && controlColor.a <= 0)
            {
                break;
            }
        }

        fadeInout.gameObject.SetActive(false);
        
    }
    #endregion

    #region WaitForDoingUi
    [SerializeField]
    WaitForDoingUi waitForDoing;

    public void RunningWaitForDoing(float _percent)
    {

        if(waitForDoing.gameObject.activeSelf == false)
        {
            Debug.LogError("대기화면창이 꺼져있습니다.");

        }

        waitForDoing.SetGauge(_percent);
    }
    public void OpenWaitForDoing(string _text)
    {
        waitForDoing.gameObject.SetActive(true);
        waitForDoing.SetGauge(_text);
    }
    public void ExitWaitForDoing()
    {
        waitForDoing.gameObject.SetActive(false);
    }

    #endregion

    #region Dialog
    [SerializeField]
    DialogueUi dialog;
    //public Shop shop;
    NpcUnit npc;
    public void OpenDialog(NpcUnit _npc)
    {
        npc = _npc;
        StartCoroutine(CoFade(SetDialog));        
    }
    void SetDialog()
    {
        baseUi.SetActive(false);
        dialog.gameObject.SetActive(true);
        dialog.StartDialogue(npc);        
    }
    void QuitDialog()
    {        
        baseUi.SetActive(true);
        dialog.gameObject.SetActive(false);
    }
    public void CloseDialog()
    {
        npc = null;
        StartCoroutine(CoFade(QuitDialog));        
    }
    #endregion

    #region Shop
    [SerializeField]
    Shop shop;
    List<int> itemList;

    public void OpenShop(List<int> _itemList)
    {
        shop.gameObject.SetActive(true);
        shop.StartShopSetting(_itemList);
    }
    public void OpenShopfromDialog(List<int> _itemList)
    {
        itemList = _itemList;
        StartCoroutine(CoFade(DialogtoShop));
    }
    void DialogtoShop()
    {
        QuitDialog();
        OpenShop(itemList);
    }
    #endregion

    #region MiniMap

    public delegate void SetMinimapInfo(string _mapName, float _xSize, float _ySize);
    public SetMinimapInfo miniMapSetting;

    public void MiniMapSetting(string _mapName, float _xSize, float _ySize)
    {
        if(miniMapSetting == null)
        {
            return;
        }
        else
        {
            miniMapSetting(_mapName,_xSize,_ySize);
        }
    }
    
    #endregion  

    #region Quest
    public MiniQuestSlot questSlot;
    public ScrollRect questList;
    public GameObject QuestList_M;
    public QuestMainUI questlist_M;

    #endregion

    #region Skill
    //스킬
    public SkillManager skillmanager;
    public GameObject skill;     
    public bool QuestActive = false;
    #endregion    
   
    #region BuffUi
    [SerializeField]
    UI_Buff uibuff;
    public delegate void UpdateBuffUi(string _buffName, float _fillAmount, int _holdTime);
    public UpdateBuffUi updateBuffUi;

    public void UpdateBuffUISetting(string _buffName, float _fillAmount, int _holdTime)         // 쿨타임도 똑같이 만들 것!
    {
        if (updateBuffUi == null)
        {
            return;
        }            
        else
        {
            updateBuffUi(_buffName, _fillAmount, _holdTime);
        }
    }

    #endregion

    #region Option
    public Option option;
    #endregion

    public UiEffectManager uiEffectManager;
    public delegate void OnUiControl(Unit _unit);
    public OnUiControl uicontrol_On;
    public OnUiControl uicontrol_Off;
   
    
    public void TryOpenQuest()
    {
        QuestActive = !QuestActive;
        if (QuestActive)
        {
            OpenQuest();
        }
        else
            CloseQuest();
    } 
    public void OpenQuest()
    {
        QuestList_M.SetActive(true);
        questlist_M.UpdateQuestSlot();
    }
    public void CloseQuest()
    {
        questlist_M.ClearInfo();        
        QuestList_M.SetActive(false);
    }
    

    public void OpenOtion()
    {
        option.gameObject.SetActive(true);
    }

    
}