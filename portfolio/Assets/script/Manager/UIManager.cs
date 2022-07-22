using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
public class UIManager : MonoBehaviour
{
    public static UIManager uimanager;
    Canvas mainCanvas;
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
        mainCanvas = GetComponent<Canvas>();
    }
    public void CanvasEnabled(bool _state)
    {
        mainCanvas.enabled = _state;
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


    public Action OpenQuickQuest
    {
        get
        {
            if(openQuickQuest == null)
            {
                return () => { Debug.Log("OpenQuickQuest is null"); };
            }
            else
            {
                return openQuickQuest;
            }
        }
        set
        {
            openQuickQuest = value;
        }
    }
    
    Action openQuickQuest;
    public Action CloseQuickQuest
    {
        get
        {
            if(closeQuickQuest == null)
            {
                return () => { Debug.Log("CloseQuickQuest is null"); };
            }
            else
            {
                return closeQuickQuest;
            }
        }
        set
        {
            closeQuickQuest = value;
        }
    }
    
    Action closeQuickQuest;



    public void OnBaseUI()
    {
        baseUi.SetActive(true);        
    }
    public void OffBaseUI()
    {
        baseUi.SetActive(false);        
    }


    public Action<int,float> AQuickSlotItemCooltime
    {
        get
        {
            if(aQuickSlotItemCooltime == null)
            {
                return (Index, cooltime) => { Debug.Log("aQuickSlotUi_itemUpdate is null"); };
            }
            else
            {
                return aQuickSlotItemCooltime;
            }
        }
        set
        {
            aQuickSlotItemCooltime = value;
        }
    }       
    public Action<int, float,int> AQuickSlotSkillCooltime
    {
        get
        {
            if (aQuickSlotSkillCooltime == null)
            {
                return (Index, cooltime,count) => { Debug.Log("aQuickSlotUi_SKillUpdate is null"); };
            }
            else
            {
                return aQuickSlotSkillCooltime;
            }
        }
        set
        {
            aQuickSlotSkillCooltime = value;
        }
    }
    Action<int, float> aQuickSlotItemCooltime;
    Action<int, float,int> aQuickSlotSkillCooltime;
    
    #region StatusUi

    Action<int> aGetGoldUpdateUi;
    Action<int> aGetExpUpdateUi;
    Action aUpdateHp;
    Action aUpdateMp;
    Action aUpdateAtk;
    Action aUpdateExp;
    Action aUpdateLevel;
    Action aUpdateSkillPoint;
    Action<int> aAddQuestUi;    
    Action<int,QUESTSTATE> aQuestUpdateUi;
    Action<Quest> aAddQuickQuestUi;
    Action<Quest> aUpdateQuickQuestUi;

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
    public Action<int> AAddQuestUi
    {
        get
        {
            if(aAddQuestUi == null)
            {
                return (questindex)=> { Debug.Log("aAddQuestUi 가 없습니다."); };
            }
            else
            {
                return aAddQuestUi;
            }
        }
        set
        {
            aAddQuestUi = value;
        }
    }    
    public Action<int,QUESTSTATE> AQuestUpdateUi
    {
        get
        {
            if(aQuestUpdateUi == null)
            {
                return (questindex,state) => { Debug.Log("AQuestUpdateUi 가 없습니다."); };
            }
            else
            {
                return aQuestUpdateUi;
            }
        }
        set
        {
            aQuestUpdateUi = value;
        }
    }
    public Action<Quest> AAddQuickQuestUi
    {
        get
        {
            if (aAddQuickQuestUi == null)
            {
                return (questindex) => { Debug.Log("aAddQuickQuestUi is null."); };
            }
            else
            {
                return aAddQuickQuestUi;
            }
        }
        set
        {
            aAddQuickQuestUi = value;
        }
    }
    public Action<Quest> AUpdateQuickQuestUi
    {
        get
        {
            if (aUpdateQuickQuestUi == null)
            {
                return (questindex) => { Debug.Log("aUpdateQuickQuestUi is null."); };
            }
            else
            {
                return aUpdateQuickQuestUi;
            }
        }
        set
        {
            aUpdateQuickQuestUi = value;
        }
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

    Action aOpenDropBox;
    Action<int> aUpdateDropBox;
    public Action AOpenDropBox
    {
        get
        {
            if(aOpenDropBox == null)
            {
                return () => { Debug.Log("AOpenDropBox is Null"); };
            }
            else
            {
                return aOpenDropBox;
            }
        }
        set
        {
            aOpenDropBox = value;
        }
    }
    public Action<int> AUpdateDropBox
    {
        get
        {
            if(aUpdateDropBox == null)
            {
                return (index) => { Debug.Log("AUpdateDropBox is Null"); };
            }
            else
            {
                return aUpdateDropBox;
            }
        }
        set
        {
            aUpdateDropBox = value;
        }
    }
    

    #endregion

    #region MiniInfo
    public Action<int, Vector2> OpenMiniInfo;
    public Action CloseMiniInfo;
    #endregion

    #region BufImage

    public Action<string, float, float> AUpdateBuf
    {
        get
        {
            if(aUpdateBuf == null)
            {
                return (key, total, duration) => { Debug.Log("버프 이미지가 활성화 되지 않았습니다.UpdateBuf"); };
            }
            else
            {
                return aUpdateBuf;
            }
        }
        set
        {
            aUpdateBuf = value;
        }
    }
    public Action<string> ARemoveBuf
    {
        get
        {
            if (aRemoveBuf == null)
            {
                return (key) => { Debug.Log("버프 이미지가 활성화 되지 않았습니다. Remove"); };
            }
            else
            {
                return aRemoveBuf;
            }
        }
        set
        {
            aRemoveBuf = value;
        }
    }

    Action<string> aRemoveBuf;
    Action<string, float, float> aUpdateBuf;
    #endregion

    #region ClickMove    
    [SerializeField]
    Image DontClick;                // 드래그 시 클릭방지

    Action<ITEMLISTTYPE,int,Vector2> itemClickUp;
    Action<ITEMLISTTYPE, int> itemUpdateSlot;
    Action<ITEMLISTTYPE, int,Vector2> moveItemIcon;
    Action<int, Vector2> aMoveSkillIcon;
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

    public Action<int,Vector2> AMoveSkillIcon
    {
        get
        {
            if(aMoveSkillIcon == null)
            {
                return (skillIndex, pos) => { Debug.Log("aMoveSkillIcon isNull"); };
            }
            else
            {
                return aMoveSkillIcon;
            }
        }
        set
        {
            aMoveSkillIcon = value;
        }
    }
    Action<int, Vector2> aMoveSkillQuick;
    public Action<int,Vector2> AMoveSkillQuick
    {
        get
        {
            if(aMoveSkillQuick == null)
            {
                return (index, Pos) => { Debug.Log("aMoveSkillQuick is null"); };
            }
            else
            {
                return aMoveSkillQuick;
            }
        }
        set
        {
            aMoveSkillQuick = value;
        }
    }
    Action<int> aUpdateSkillMain;
    public Action<int> AUpdateSkillMain
    {
        get
        {
            if (aUpdateSkillMain == null)
            {
                return (index) => { Debug.Log("aUpdateSlotMain is Null"); };
            }
            else
            {
                return aUpdateSkillMain;
            }
        }
        set
        {
            aUpdateSkillMain = value;
        }
    }
    Action<int> aUpdateSkillSlot;
    public Action<int> AUpdateSkillSlot
    {
        get
        {
            if(aUpdateSkillSlot == null)
            {
                return (index) => { Debug.Log("aUpdateSkillSlot is Null"); };
            }
            else
            {
                return aUpdateSkillSlot;
            }
        }
        set
        {
            aUpdateSkillSlot = value;
        }
    }


   
    #endregion

    #region Fade
    [SerializeField]
    Image fadeInout;
    
    public void FadeinFucout(Action _actionO=null, Action _action1=null)
    {
        StartCoroutine(CoFadeinFucout(_actionO, _action1));
    }


    IEnumerator CoFadeinFucout(Action _fadeinAction = null, Action _fadeOutAction = null)
    {
        yield return StartCoroutine(CoFadeIn());
        if(_fadeinAction != null)
        {
            _fadeinAction();
        }
        
        StartCoroutine(CoFadeOut());
        if (_fadeOutAction != null)
        {
            _fadeOutAction();
        }
    }
    IEnumerator CoFadeIn()
    {
        fadeInout.gameObject.SetActive(true);
        Color controlColor = fadeInout.color;
        controlColor.a = 0;
        fadeInout.color = controlColor;
        float timer = 0;        
        while (controlColor.a<1)
        {
            yield return null;
            timer += Time.unscaledDeltaTime;

            controlColor.a = Mathf.Lerp(0f, 1f, timer) ;
            fadeInout.color = controlColor;
        }
    }
    IEnumerator CoFadeOut()
    {
        Color controlColor = fadeInout.color;
        controlColor.a = 0;
        fadeInout.color = controlColor;
        float timer = 0;        
        while (controlColor.a > 0)
        {
            yield return null;
            timer += Time.unscaledDeltaTime;

            controlColor.a = Mathf.Lerp(1f, 0f, timer);
            fadeInout.color = controlColor;
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

    Action<Npc> aOpenDialog;
    Action aCloseDialog;
    public Action<Npc> AOpenDialog
    {
        get
        {
            if(aOpenDialog == null)
            {
                return (npc) => { Debug.Log("aOpenDialog is null"); };
            }
            else
            {
                return aOpenDialog;
            }

        }
        set => aOpenDialog = value;        
    }    
    public Action ACloseDialog
    {
        get
        {
            if (aCloseDialog == null)
            {
                return () => { Debug.Log("aCloseDialog is null"); };
            }
            else
            {
                return aCloseDialog;
            }

        }
        set => aCloseDialog = value;
    }


    
    
    
  
    #endregion

    
    Action<NpcUnit> aOpenShop;
    public Action<NpcUnit> AOpenShop
    {
        get
        {
            if(aOpenShop == null)
            {
                return (npc) => { Debug.Log("OpenShop is null."); };
            }
            else
            {
                return aOpenShop;
            }
        }
        set
        {
            aOpenShop = value;
        }
    }

    
  
    #region Quest
    public MiniQuestSlot questSlot;
    public ScrollRect questList;
    public GameObject QuestList_M;
    public Ui_QuestMain questlist_M;

    #endregion

    #region Skill
    //스킬
    public UI_Skill skillmanager;
    public GameObject skill;     
    public bool QuestActive = false;
    #endregion    
   
  
    #region Option
    public Option option;
    #endregion

    public UiEffectManager uiEffectManager;
    public delegate void OnUiControl(Unit _unit);
    public OnUiControl uicontrol_On;
    public OnUiControl uicontrol_Off;
   
    

    public void OpenOtion()
    {
        option.gameObject.SetActive(true);
    }

    
}