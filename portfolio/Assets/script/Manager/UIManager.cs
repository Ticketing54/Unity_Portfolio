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


    #region ClickMove
    [SerializeField]
    MiniInfo miniInfo;

    UI_ItemSlots startUiItemSlot = null;
    ITEMLISTTYPE startListType = ITEMLISTTYPE.NONE;   
    int          startListIndex = -1;
        
    
    public void ClickUpitemSlot(UI_ItemSlots _endUiItemSlot,ITEMLISTTYPE _EndListType, int _EndListIndex)
    {
        if(startListIndex == _EndListIndex)
        {
            LeftClick(Character.Player.ItemList_GetItem(startListType,startListIndex));
            ClickMoveReset();
            return;
        }
        if (Character.Player.ItemMove(startListType, _EndListType, startListIndex, _EndListIndex))
        {
            startUiItemSlot.UpdateSlot(startListIndex, Character.Player.ItemList_GetItem(startListType, startListIndex));
            _endUiItemSlot.UpdateSlot(_EndListIndex, Character.Player.ItemList_GetItem(_EndListType, _EndListIndex));
        }
        ClickMoveReset();
    }

    public void LeftClick(Item _Miniinfoitem)
    {
        miniInfo.gameObject.SetActive(true);
        Vector2 Pos = Input.mousePosition;
        miniInfo.SetMiniInfo(_Miniinfoitem, Pos);
    }
    public bool ClickUpPossable()
    {
        if(startListType == ITEMLISTTYPE.NONE || startListIndex < 0)
        {
            return false;
        }
        return true;
    } 
    void ClickMoveReset()
    {
        startUiItemSlot = null;
        startListIndex = -1;        
        startListType = ITEMLISTTYPE.NONE;
    }
    public void StartDragItem(UI_ItemSlots _startUiItemSlot,ITEMLISTTYPE _startListType,int _startListIndex)
    {
        startUiItemSlot = _startUiItemSlot;
        startListType = _startListType;
        startListIndex = _startListIndex;           
    }
    

    #endregion

    #region Quick_Slots
    [SerializeField]
    List<Slot> QuickSlot_Item;
    [SerializeField]
    List<SkillSlot> QuickSlot_Skill;
    [SerializeField]
    QuickQuest quickQuest;


   



    void QuickSlot_Item_Update()
    {

    }
    void QuickSlot_Skill_Update()
    {

    }
    #endregion

    #region DropBox    
    public DropBox dropBox;


    #endregion

    #region Dialog
    public GameObject UiObj;
    public CanvasGroup FadeUi;
    public Image FadeInout;
    public Dialogue dialog;
    public Shop shop;
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
            if (Input.GetKeyDown(KeyCode.K))
            {
                TryOpenSkill();
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
    public void CloseDialog(Npc _npc) // 대화창 닫기
    {
        _npc.dialog_Done = false;
        _npc.Dialog_num = 1;
        _npc.NextDialog = -1;
        dialog.ChoiceReset();
        UIManager.uimanager.DialogControl();

        if (shop.gameObject.activeSelf == true)
        {
            shop.gameObject.SetActive(false);
            //TryOpenInventory();
        }
        
    }
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
    public void DialogControl()
    {
        StartCoroutine(Fade(!dialog.gameObject.activeSelf));        
    }

    IEnumerator Fade(bool UiAtice)
    {
        Color color = FadeInout.color;
        if(color.a <= 0)
        {
            FadeInout.gameObject.SetActive(true);
            while (true)
            {
                color.a += Time.deltaTime;
                FadeInout.color = color;

                if(color.a >= 1)
                {
                    if (UiAtice)
                    {
                        FadeUi.alpha = 0;
                    }
                    else
                    {
                        FadeUi.alpha = 1;
                    }

                    dialog.gameObject.SetActive(UiAtice);
                    dialog.dialogreset();
                    StartCoroutine(Fade(UiAtice));
                    yield break;
                }




                yield return null;
            }



        }
        else if (color.a >= 1)
        {
            while (true)
            {
                color.a -=Time.deltaTime;
                FadeInout.color = color;


                if (color.a <= 0)
                {
                    FadeInout.gameObject.SetActive(false);
                    yield break;
                }
                    




                yield return null;
            }






        }

        

        
    }

    
    //public void InfoUpdate()
    //{
    //    Inven.gameObject.SetActive(true);
    //    //Inv.SlotUpdate();
    //    Inven.gameObject.SetActive(false);
    //    skill.gameObject.SetActive(true);
    //    //skillmanager.UpdateSkill();
    //    skill.gameObject.SetActive(false);
    //    //for (int i = 0; i < Character.Player.myQuick.Count; i++)
    //    //{
    //    //    Inv.Q_list[Character.Player.myQuick[i].SlotNum].Add(Character.Player.myQuick[i]);
    //    //    Inv.Q_list[Character.Player.myQuick[i].SlotNum].SetSlotCount();
    //    //}
    //}
    //public void InfoReset()
    //{
    //    Inven.gameObject.SetActive(true);        
    //    Inven.gameObject.SetActive(false);       

    //}
    
    //public void EscControl()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        if(Inven.activeSelf == true)
    //        {
    //            //TryOpenInventory();
    //            return;
    //        }
    //        if(skill.activeSelf == true)
    //        {
    //            TryOpenSkill();
    //            return;
    //        }
    //        if(QuestList_M.activeSelf == true)
    //        {
    //            TryOpenQuest();
    //            return;     
    //        }            
    //        if(option.gameObject.activeSelf == false)
    //        {
    //            OpenOtion();
    //            return;
    //        }


    //    }
    //}
    public void OpenOtion()
    {
        option.gameObject.SetActive(true);


    }
void TryOpenSkill()
    {
        SkillActive = !SkillActive;
        if (SkillActive)
        {
            OpenSkill();


        }
        else
            CloseSkill();
    } // 스킬창 오픈
    public void OpenSkill()
    {
        
        skill.SetActive(true);        

        
    }
    public void CloseSkill()
    {     
        skill.SetActive(false);

    }

   
   
}