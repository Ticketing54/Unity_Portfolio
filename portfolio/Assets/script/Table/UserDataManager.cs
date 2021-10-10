using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UserDataManager : SingleTon<UserDataManager>
{
 
    public void SaveData(int _num)
    {

        string Data = string.Empty;
        Data += Character.Player.Stat.NAME + ",";
        Data += GameManager.gameManager.MapName + ",";
        Data += Character.Player.transform.position.x + ",";        
        Data += Character.Player.transform.position.y + ",";        
        Data += Character.Player.transform.position.z + ",";        
        Data += Character.Player.Stat.LEVEL + ",";        
        Data += Character.Player.Stat.HP + ",";        
        Data += Character.Player.Stat.MP + ",";        
        Data += Character.Player.Stat.EXP + ",";         
        Data += Character.Player.Stat.SkillPoint + ",";         
        Data += Character.Player.Stat.GOLD + "\n";         
        
        
        string Inven = string.Empty;        //인벤
        for (int i = 0; i < Character.Player.myIven.Count; i++)
        {
            Inven += Character.Player.myIven[i].Index + "," + Character.Player.myIven[i].SlotNum + "," + Character.Player.myIven[i].ItemCount + "/";
        }
        if (Inven.Length > 0)
            Inven = Inven.Substring(0,Inven.Length - 1);
        
        
        string Equip = string.Empty;        //장비
        for (int i = 0; i < Character.Player.myEquip.Count; i++)
        {
            Equip += Character.Player.myEquip[i].Index + "," + Character.Player.myEquip[i].EquipType + "/";
        }        
        if(Equip.Length > 0)
        {
            Equip =Equip.Substring(0,Equip.Length - 1);
        }
        string QuickItem = string.Empty;            //퀵아이템
        for (int i = 0; i < Character.Player.myQuick.Count; i++)
        {
            QuickItem += Character.Player.myQuick[i].Index + "," + Character.Player.myQuick[i].SlotNum + "," + Character.Player.myQuick[i].ItemCount + "/";
        }
        
        if(QuickItem.Length >0)
            QuickItem = QuickItem.Substring(0,QuickItem.Length - 1);

        string Quest = string.Empty;        //퀘스트
        for(int i = 0; i < Character.Player.myQuest.Count; i++)
        {
            Quest += Character.Player.myQuest[i].Index + "," + Character.Player.myQuest[i].QuestComplete + ","+ Character.Player.myQuest[i].goal_C+"/";
        }
        
        if(Quest.Length >0)
            Quest = Quest.Substring(0,Quest.Length - 1);


        Data += Inven + "\n";
        Data += Equip + "\n";
        Data += QuickItem + "\n";
        Data += Quest + "\n";

        PlayerPrefs.SetString(_num.ToString(), Data);
    }
    public string LoadData(int _num)
    {
        
       if(PlayerPrefs.GetString(_num.ToString()) != null && PlayerPrefs.GetString(_num.ToString()) !="")
       {
            return PlayerPrefs.GetString(_num.ToString());
       }
       return null;
    }
 
}
