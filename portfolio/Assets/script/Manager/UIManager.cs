using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
public class UIManager : MonoBehaviour
{
    public static UIManager uimanager;
    public RectTransform UI;

    #region Ui_Effect

    public UiEffectControl uieffect;

    #endregion

    #region Middle_Top_HpBar    
    [SerializeField]
    TopEnermyInfoUi topEnermyInfoUi;
    public void Open_Top_EnermyInfo(MonsterUiControl _Monster)              // 상단 중앙 적정보 UI
    {
        topEnermyInfoUi.gameObject.SetActive(true);
        topEnermyInfoUi.Top_EnermyInfoUi(_Monster);
    }
    #endregion

    #region Middle_Bottom_HpBar        
    [SerializeField]
    Bottom_Character_Info bottom_Character_Info;
    public void Update_CharacterInfo_Ui()
    {
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
    public delegate void UpdateUiSlot(ITEMLISTTYPE _itemListType, int _Index);                      //
    public UpdateUiSlot updateUiSlot;                                                               // 따로 자료형을 만들어서 제작하면 여러개를 한번에 할수 있을듯
    
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
            LeftClick(Character.Player.ItemList_GetItem(startListType,startListIndex));
            ClickMoveReset();
            return;
        }

        Character.Player.ItemMove(startListType, _EndListType, startListIndex, _EndListIndex);
            
        ClickMoveReset();
    }

    void SetMoveIcon(Sprite _image)                  //드래그 아이콘 설정
    {
        moveicon.gameObject.SetActive(true);
        moveicon.ActiveMoveIcon(_image);
    }
    void LeftClick(Item _Miniinfoitem)
    {
        miniInfo.gameObject.SetActive(true);
        Vector2 Pos = Input.mousePosition;
        miniInfo.SetMiniInfo(_Miniinfoitem, Pos);
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

    #region Dialog
    public GameObject UiObj;
    public CanvasGroup FadeUi;
    public Image FadeInout;
    //public Dialogue dialog;
    //public Shop shop;
    #endregion

    #region UpdateMessage
    [SerializeField]
    PatchUi updateMessage;

    public delegate void UpdatePatchMessage(float _percent);
    public UpdatePatchMessage updatePatchMessage;
    public void OpenUpdateMessage(long _downloadFileSize)
    {
        updateMessage.gameObject.SetActive(true);
    }
    public void CloseUpdateMessage()
    {
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
     bool SkillActive = false;
    public bool QuestActive = false;
    #endregion    

    #region MiniMap
    public MiniMap minimap;
    public GameObject Minimap_n;
    public GameObject Minimap_M;
    #endregion

    #region Option
    public Option option;
    #endregion

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

  

    void Update()
    {       


        if(Character.Player != null && FadeUi.alpha == 1)
        {            
            
            if (Input.GetKeyDown(KeyCode.I))
            {
                //TryOpenInventory();
            }          
            if (Input.GetKeyDown(KeyCode.Q))
            {

            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                TryOpenMiniMap_n();
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                TryOpenMiniMap_M();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                TryOpenQuest();
            }

            
        }




    }
    public void TryOpenMiniMap_M()
    {
        minimap.miniMap_M_Max_Min();
    }
   
    public void TryOpenMiniMap_n()
    {
        minimap.miniMap_n_Max_Min();
    }
   
   

    //public void OpenShop(Npc _npc) // 상점열기
    //{
    //    if (InventoryActive == false)
    //    {
    //        TryOpenInventory();            
    //    }
            
    //    Inven.transform.position = shop.gameObject.transform.position + new Vector3(325f, 0, 0);

    //    shop.gameObject.SetActive(true);
    //    //shop.ShopUpdate(_npc);
        
    //}
    //public void CloseDialog(Npc _npc) // 대화창 닫기
    //{
    //    _npc.dialog_Done = false;
    //    _npc.Dialog_num = 1;
    //    _npc.NextDialog = -1;
    //    dialog.ChoiceReset();
    //    //UIManager.uimanager.DialogControl();

    //    if (shop.gameObject.activeSelf == true)
    //    {
    //        shop.gameObject.SetActive(false);
    //        //TryOpenInventory();
    //    }
        
    //}
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