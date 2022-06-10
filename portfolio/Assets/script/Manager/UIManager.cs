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

   
    public void UpdateUISlots(ITEMLISTTYPE _type,int index)
    {
        if(updateUiSlot == null)
        {
            return;
        }
        else
        {
            updateUiSlot(_type, index);
        }
    }
    public void OnBaseUI()
    {
        baseUi.SetActive(true);        
    }
    public void OffBaseUI()
    {
        baseUi.SetActive(false);        
    }    

    #region StatusUi
    public Action<int> AGetGoldUpdateUi;
    public Action<int> AGetExpUpdateUi;
    public Action AUpdateHp;
    public Action AUpdateMp;
    public Action AUpdateAtk;
    public Action AUpdateExp;
    public Action AUpdateLevel;
    public Action AUpdateSkillPoint;    
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
    MoveIcon moveicon;              // 드래그 아이콘
    [SerializeField]
    Image DontClick;                // 드래그 시 클릭방지

    // Left 클릭 정보    
    ITEMLISTTYPE startListType = ITEMLISTTYPE.NONE;   
    int          startListIndex = -1;
    ItemSlot startItemSlot = null;

    Coroutine RunningCoroutine;                              // 증복 실행 방지
    


    public delegate void EndClicItemMove(Vector2 _Pos);
    public EndClicItemMove itemoveEnd;

    // 각각의 아이템슬롯 업데이트
    public delegate void UpdateUiSlot(ITEMLISTTYPE _itemListType, int _Index);                      
    public UpdateUiSlot updateUiSlot;                                                               
    
    public void StartDragItem(ItemSlot _itemSlot,ITEMLISTTYPE _startListType, int _startListIndex)
    {
        if (RunningCoroutine != null)
        {
            StopCoroutine(RunningCoroutine);
        }

                
        startItemSlot = _itemSlot;
        startListType = _startListType;
        startListIndex = _startListIndex;
        SetMoveIcon(startItemSlot.ICON);
        startItemSlot.ClickedSlot_Start();
        RunningCoroutine = StartCoroutine(ClickStart());
    }
    IEnumerator ClickStart()
    {
        DontClick.gameObject.SetActive(true);
        while (true)
        {
            if (Input.GetMouseButtonUp(0))
            {
                itemoveEnd(Input.mousePosition);
                ClickMoveReset();

                yield break;
            }
            yield return null;
        }
    }    
    public void ClickUpitemSlot(ITEMLISTTYPE _EndListType, int _EndListIndex)
    {
        if (startListType == ITEMLISTTYPE.NONE || startListIndex < 0)           // 클릭이 되지 않았을떄
        {   
            return;
        }  
        
        if(startListType == ITEMLISTTYPE.ITEMBOX && _EndListType == ITEMLISTTYPE.INVEN)
        {
            OpenDropBoxCountMessage(startListIndex);
            return;
        }

        GameManager.gameManager.character.ItemMove(startListType, _EndListType, startListIndex, _EndListIndex);
    }

    void SetMoveIcon(Sprite _image)                  //드래그 아이콘 설정
    {
        moveicon.gameObject.SetActive(true);
        moveicon.ActiveMoveIcon(_image);
    }
   
    void ClickMoveReset()
    {
        DontClick.gameObject.SetActive(false);        
        startListIndex = -1;        
        startListType = ITEMLISTTYPE.NONE;

        if (startItemSlot != null)
        {
            startItemSlot.ClickedSlot_End();
        }
        startItemSlot = null;
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