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
        
    }
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
        AddkeyboardShortcut();
    }
    public void OffBaseUI()
    {
        baseUi.SetActive(false);
        RemovekeyboardShortcut();
    }
    void AddkeyboardShortcut()
    {
        GameManager.gameManager.character.keyboardShortcut.Add(KeyCode.I, TryOpenInventory);        
        GameManager.gameManager.character.keyboardShortcut.Add(KeyCode.L, TryOpenQuest);
    }
    void RemovekeyboardShortcut()
    {
        GameManager.gameManager.character.keyboardShortcut.Remove(KeyCode.I);
        GameManager.gameManager.character.keyboardShortcut.Remove(KeyCode.N);
        GameManager.gameManager.character.keyboardShortcut.Remove(KeyCode.M);
        GameManager.gameManager.character.keyboardShortcut.Remove(KeyCode.L);
    }


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
    public void Open_Top_EnermyInfo(BattleUnit _Monster)              // 상단 중앙 적정보 UI
    {
        topEnermyInfoUi.gameObject.SetActive(true);
        topEnermyInfoUi.Top_EnermyInfoUi(_Monster);
    }
    #endregion

    #region Middle_Bottom_Status      
    [SerializeField]
    Bottom_Character_Info bottom_Character_Info;
    public void Update_CharacterInfo_Ui()
    {
        if(bottom_Character_Info.gameObject.activeSelf == false)
        {
            return;
        }
        bottom_Character_Info.InfoUpdate();
    }
    #endregion

    #region Ui_Inventory

    [SerializeField]
    UI_Inventory Inv;
    public delegate void UpdateInventoryUi();
    public UpdateInventoryUi updateInven;
    
    bool inventoryActive = false;    
    public bool InventoryActive { get { return inventoryActive; } }
    public void TryOpenInventory()
    {
        inventoryActive = !inventoryActive;
        if (inventoryActive)
        {
            OpenInventory();            
        }
        else
        {
            CloseInventory();
        }            
    } 
    public void OpenInventory()
    {
        Inv.gameObject.SetActive(true);
    }
    public void CloseInventory()
    {        
        Inv.gameObject.SetActive(false);
              
    }


    #endregion

    #region Equipment
    public UI_Equipment uI_Equipment;
    bool equipmentActive = false;
    public bool EquipmentActive { get { return equipmentActive; } }
    public void TryOpenEquipment()
    {
        equipmentActive = !equipmentActive;
        if (equipmentActive)
        {
            OpenEquipment();
        }
        else
        {
            CloseEquipment();
        }
    }
    public void OpenEquipment()
    {
        uI_Equipment.gameObject.SetActive(true);
    }
    public void CloseEquipment()
    {
        uI_Equipment.gameObject.SetActive(false);

    }



    #endregion

    #region ClickMove
    [SerializeField]                // 클릭시 아이템 정보
    MiniInfo miniInfo;

 
    public void OpenMiniInfo(int _itemIndex,Vector2 _Pos)
    {
        miniInfo.gameObject.SetActive(true);
        miniInfo.SetMiniInfo(_itemIndex,_Pos);
    }
    
    public void CloseMiniInfo()
    {
        miniInfo.gameObject.SetActive(false);
    }
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


                yield break;
            }
            yield return null;
        }
    }

    public void ClickUpitemSlot(ITEMLISTTYPE _EndListType, int _EndListIndex)
    {
        if (startListType == ITEMLISTTYPE.NONE || startListIndex < 0)           // 클릭이 되지 않았을떄
        {
            ClickMoveReset();
            return;
        }            


        if(startListIndex == _EndListIndex && startListType == _EndListType)                // 같은곳을 클릭 했을때
        {
            OpenMiniInfo(GameManager.gameManager.character.ItemList_GetItem(startListType,startListIndex).index,Input.mousePosition);
            ClickMoveReset();
            return;
        }

        GameManager.gameManager.character.ItemMove(startListType, _EndListType, startListIndex, _EndListIndex);
            
        ClickMoveReset();
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

    #region DropBox    
    public DropBox dropBox;


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

    #region UpdateMessage
    [SerializeField]
    PatchUi updateMessage;    
    [SerializeField]
    CanvasGroup patchCanvasGroup;



    public void OpenUpdateMessage(long _downloadFileSize)
    {        
        updateMessage.SetUpdateMessage(_downloadFileSize);
    }    
    public void ClosePatchUi()
    {
        StartCoroutine(FadeoutPatch());
    }
    IEnumerator FadeoutPatch()
    {        
        while (true)
        {            
            patchCanvasGroup.alpha -= Time.unscaledDeltaTime*0.5f;
            if(patchCanvasGroup.alpha <= 0)
            {
                break;
            }

            yield return null;
        }

        updateMessage.gameObject.SetActive(false);
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