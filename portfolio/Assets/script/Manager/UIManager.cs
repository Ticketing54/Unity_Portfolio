using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager uimanager;
    List<Monster> Moblist = new List<Monster>();    

    //상단 체력바
    public TextMeshProUGUI NickName;
    public GameObject Enermy_HpImage;
    public Text Enermy_Name;

    //Hp,Mp바
    public TextMeshProUGUI Enermy_Hp_Text;
    public Image Enermy_Hp;
    public Image Hp_bar;
    public Image Mp_bar;
    public TextMeshProUGUI Hp_text;
    public TextMeshProUGUI Mp_text;           

    //인벤토리
    public GameObject Inven;
    public ITemUiManager Inv;
    public bool InventoryActive    = false;
    public MiniInfo miniinfo;        
    // Exp
    public Image Exp_bar;
    public TextMeshProUGUI Exp_Text;
    public TextMeshProUGUI Lev;
    public RectTransform UI;
    //퀵슬롯
    public List<Slot> QuickSlot;
    public List<SkillSlot> QuickSlot_Skill;
    //드랍박스
    public DropBox dropBox;

    //대화    
    public GameObject UiObj;
    public CanvasGroup FadeUi;
    public Image FadeInout;
    public Dialogue dialog;
    public Shop shop;

    //퀘스트
    public MiniQuestSlot questSlot;
    public ScrollRect questList;
    public GameObject QuestList_M;
    public Questlist questlist_M;
    
    

    //스킬
    public SkillManager skillmanager;
    public GameObject skill;
     bool SkillActive = false;
    public bool QuestActive = false;
    //미니맵 
    public MiniMap minimap;
    public GameObject Minimap_n;
    public GameObject Minimap_M;

    //옵션창
    public Option option;    
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

    private void Start()
    {
        StartCoroutine(NickUpdate());
    }



    void Update()
    {
        
        if(Character.Player != null && FadeUi.alpha == 1)
        {
            if (Moblist == null)
            {
                Moblist = Character.Player.MobList;
            }            
            Enermy_Hpbar();
            statusControl();
            if (Input.GetKeyDown(KeyCode.I))
            {
                TryOpenInventory();
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

            UseQuickSlot();
            UseQuickSkill();
            EscControl();
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
   


    public void OpenShop(Npc _npc) // 상점열기
    {
        if (InventoryActive == false)
        {
            TryOpenInventory();
        }
            
        Inven.transform.position = shop.gameObject.transform.position + new Vector3(325f, 0, 0);

        shop.gameObject.SetActive(true);
        //shop.ShopUpdate(_npc);
        
    }
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
            TryOpenInventory();
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


    }
    public void CloseQuest()
    {
        questlist_M.emptyinfo();
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

    
    public void InfoUpdate()
    {
        Inven.gameObject.SetActive(true);
        //Inv.SlotUpdate();
        Inven.gameObject.SetActive(false);
        skill.gameObject.SetActive(true);
        //skillmanager.UpdateSkill();
        skill.gameObject.SetActive(false);
        //for (int i = 0; i < Character.Player.myQuick.Count; i++)
        //{
        //    Inv.Q_list[Character.Player.myQuick[i].SlotNum].Add(Character.Player.myQuick[i]);
        //    Inv.Q_list[Character.Player.myQuick[i].SlotNum].SetSlotCount();
        //}
    }
    public void InfoReset()
    {
        Inven.gameObject.SetActive(true);        
        Inven.gameObject.SetActive(false);
        QuestManager.questManager.Quest_Reset();

    }
    public void UseQuickSlot()
    {        
       
    }
    public void UseQuickSkill()
    {
       
    }
   

    public void EscControl()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(Inven.activeSelf == true)
            {
                TryOpenInventory();
                return;
            }
            if(skill.activeSelf == true)
            {
                TryOpenSkill();
                return;
            }
            if(QuestList_M.activeSelf == true)
            {
                TryOpenQuest();
                return;     
            }            
            if(option.gameObject.activeSelf == false)
            {
                OpenOtion();
                return;
            }


        }
    }
    public void OpenOtion()
    {
        option.gameObject.SetActive(true);


    }
    public void TryOpenInventory()
    {        
        InventoryActive = !InventoryActive;
        if (InventoryActive)
        {
            OpenInventory();


        }
        else
            CloseInventory();
    } // 인벤토리 오픈
    public void OpenInventory()
    {
        
        Inven.SetActive(true);        

        
    }
    public void CloseInventory()
    {
        if (miniinfo.gameObject.activeSelf == true)
            miniinfo.gameObject.SetActive(false);
        Inven.SetActive(false);

    }  
    public void TryOpenSkill()
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
    public void SaveItemInfo()
    {
        if (Inven.activeSelf == false)
            Inven.gameObject.SetActive(true);



        //Character.Player.myIven = new List<Item>();
        
        //for (int i = 0; i < Inv.Inven.Count; i++)
        //{
        //    if (Inv.Inven[i].Icon.gameObject.activeSelf == true)
        //    {
        //        //Character.Player.myIven.Add(Inv.list[i].item);

        //    }
        //}
        //for (int i = 0; i < Inv.Quick.Count; i++)
        //{
        //    if (Inv.Quick[i].Icon.gameObject.activeSelf == true)
        //    {
        //        Item item = Inv.Quick[i].item;
        //        item.SlotNum = i;
        //        //Character.Player.myQuick.Add(item);

        //    }
        //}
        //for (int i = 0; i < Inv.Equip.Count; i++)
        //{
        //    if (Inv.Equip[i].Icon.gameObject.activeSelf == true)
        //    {
        //        //Character.Player.myEquip.Add(Inv.E_list[i].item);

        //    }
        //}

        if (Inven.activeSelf == true)
        {
            Inven.gameObject.SetActive(false);
            InventoryActive = false;
        }
            
    }    
    

    IEnumerator NickUpdate()
    {
        
        while (true)
        {
            if(Character.Player != null && Camera.main !=null)
            {
                if (NickName.gameObject.activeSelf == false)
                    NickName.gameObject.SetActive(true);
                NickName.text = Character.Player.Stat.NAME;
                NickName.transform.position = Camera.main.WorldToScreenPoint(Character.Player.transform.position + new Vector3(0f, 2f, 0f));

                
            }


            yield return new WaitForFixedUpdate();


        }
        
    }

    
 
    void Enermy_Hpbar()
    {
        if (Character.Player.Target == null)
            Enermy_HpImage.SetActive(false);

        if (Character.Player.Target != null && Character.Player.Target.tag == "Monster")
        {
            if (Character.Player.GetMonster(Character.Player.Target).Lev == 0)
                return;

            Enermy_HpImage.SetActive(true);
            Enermy_Hp.fillAmount = (float)(Character.Player.GetMonster(Character.Player.Target).Hp / Character.Player.GetMonster(Character.Player.Target).Hp_max);
            Enermy_Name.text = Character.Player.GetMonster(Character.Player.Target).MobName;
            Enermy_Hp_Text.text = Character.Player.GetMonster(Character.Player.Target).Hp.ToString() + " / " + Character.Player.GetMonster(Character.Player.Target).Hp_max.ToString();


        }
        
    }
    void statusControl()
    {
        Hp_bar.fillAmount = Character.Player.Stat.HP / Character.Player.Stat.MAXHP;
        Hp_text.text = ((int)Character.Player.Stat.HP).ToString() + " / " + ((int)Character.Player.Stat.HP).ToString();
        Mp_bar.fillAmount = Character.Player.Stat.MP / Character.Player.Stat.MAXMP;
        Mp_text.text = ((int)Character.Player.Stat.MP).ToString() + " / " + ((int)Character.Player.Stat.MAXMP).ToString();
        //

        Lev.text = "Level : " + Character.Player.Stat.LEVEL.ToString();
        Exp_Text.text = Character.Player.Stat.EXP.ToString() + " / " + Character.Player.Stat.MAXEXP.ToString();
        Exp_bar.fillAmount = Character.Player.Stat.EXP / Character.Player.Stat.MAXEXP;


    }   
   
}
