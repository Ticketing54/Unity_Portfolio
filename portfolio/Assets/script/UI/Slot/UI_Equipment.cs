using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UI_Equipment : UI_ItemSlots
{
    [SerializeField]
    TextMeshProUGUI NickName;
    [SerializeField]
    TextMeshProUGUI Lv;
    [SerializeField]
    TextMeshProUGUI Exp;
    [SerializeField]
    TextMeshProUGUI Hp;
    [SerializeField]
    TextMeshProUGUI Mp;
    [SerializeField]
    TextMeshProUGUI atk;

    

    private void Awake()
    {
        itemListType = ITEMLISTTYPE.EQUIP;
        UIManager.uimanager.AOpenEquipment += () => gameObject.SetActive(true);
        UIManager.uimanager.ACloseEquipment += () => gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
 

    public override void UpdateAllSlot()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            Item equipitem = character.equip.GetItem(i);

            if(equipitem != null)
            {
                itemSlots[i].Add(equipitem.itemSpriteName);
            }
            else
            {
                itemSlots[i].Clear();
            }
        }
    }
    public override void OnEnable()
    {
        base.OnEnable();             
        OnEpuipUi();
        CharacterInfoSetting();
    }

    public override void OnDisable()
    {
        base.OnDisable();        
        OffEquipUi();
    }

    void CharacterInfoSetting()
    {
        NickName.text = character.name;
        SetHpUi();
        SetMpUi();
        SetAtkUi();
        SetExpUi();
        SetLevelUi();
    }

    void OnEpuipUi()
    {
        UIManager.uimanager.AUpdateHp    += SetHpUi;
        UIManager.uimanager.AUpdateMp    += SetMpUi;
        UIManager.uimanager.AUpdateLevel += SetLevelUi;
        UIManager.uimanager.AUpdateExp   += SetExpUi;
        UIManager.uimanager.AUpdateAtk   += SetAtkUi;
    }
    void OffEquipUi()
    {
        UIManager.uimanager.AUpdateHp    -= SetHpUi;
        UIManager.uimanager.AUpdateMp    -= SetMpUi;
        UIManager.uimanager.AUpdateLevel -= SetLevelUi;
        UIManager.uimanager.AUpdateExp   -= SetExpUi;
        UIManager.uimanager.AUpdateAtk   -= SetAtkUi;
    }

    void SetHpUi()
    {
        int curhp = (int)character.stat.Hp;
        int maxHp = (int)character.stat.MaxHp;
        int e_Hp = (int)character.stat.E_Hp;
        string text;
        if(e_Hp != 0)
        {
            text = curhp+" / "+ maxHp+"( " + maxHp + " + " + e_Hp + " )";
        }
        else
        {
            text = curhp + " / " + maxHp;
        }
        Hp.text = text;
    }

    void SetMpUi()
    {
        int curMp = (int)character.stat.Mp;
        int maxMp = (int)character.stat.MaxMp;
        int e_Mp = (int)character.stat.E_Mp;
        string text;
        if (e_Mp != 0)
        {
            text = curMp + " / " + maxMp + "( " + maxMp + " + " + e_Mp + " )";
        }
        else
        {
            text = curMp + " / " + maxMp;
        }
        Mp.text = text;
    }

    void SetAtkUi()
    {   
        int c_Atk = (int)character.stat.Atk;
        int e_Atk = (int)character.stat.E_Atk;
        int allatk = c_Atk + e_Atk;
        int minatk = (int)(allatk * 0.8);
        int maxatk = (int)(allatk * 1.2);
        string text;
        if (e_Atk != 0)
        {
            text = minatk + " ~ " + maxatk + "( " + c_Atk + " + " + "<color=#0000ffff>"+e_Atk + "</color>"+" )";
        }
        else
        {
            text = minatk + " ~ " + maxatk;
        }


        atk.text = text;
    }
    void SetExpUi()
    {
        Exp.text = character.stat.Exp + " / " + character.stat.MaxExp;
    }
    void SetLevelUi()
    {
        Lv.text = character.stat.Level.ToString();
    }
   
}
