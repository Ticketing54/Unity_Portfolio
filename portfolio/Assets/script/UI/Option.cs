using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour
{
    public List<MainSaveSlot> SaveData = new List<MainSaveSlot>();
    public List<MainSaveSlot> LoadData = new List<MainSaveSlot>();
    public GameObject SaveSlot;
    public GameObject LoadSlot;
    public GameObject SoundControl;



    //확인메세지
    public GameObject Save_Message;
    public GameObject Load_Message;
    public GameObject Main_Message;


  

    //누른 슬롯
    public int ClickNum = -1;


    void Update()
    {
        EscControl();
    }

  
    public void EscControl()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Save_Message.activeSelf == true)
            {
                Save_Message.SetActive(false);
                return;
            }
            if(Load_Message.activeSelf == true)
            {
                Load_Message.SetActive(false);
                return;
            }
            if(Main_Message.activeSelf == true)
            {
                Main_Message.SetActive(false);
                return;
            }
            if(SoundControl.activeSelf == true)
            {
                SoundControl.SetActive(false);
                return;
            }
            if(SaveSlot.activeSelf == true)
            {
                SaveSlot.SetActive(false);
                return;
            }
            if (LoadSlot.activeSelf == true)
            {
                LoadSlot.SetActive(false);
                return;
            }
            this.gameObject.SetActive(false);




        }
    }
    public void SaveYesMessage()
    {
        int num = ClickNum;
        ClickNum = -1;
        UIManager.uimanager.SaveItemInfo();
        //UserDataManager.instance.SaveData(num);
        OpenSavedGames();
        Save_Message.gameObject.SetActive(false);
        
    }
    public void SaveNoMessage()
    {
        Save_Message.gameObject.SetActive(false);        
        ClickNum = -1;
    }
    public void LoadYesMessage()
    {
       
        GameManager.gameManager.Player_num = ClickNum;
        string tmp = UserDataManager.instance.LoadData(ClickNum);
        string[] Data = tmp.Split('\n');
        string map = Data[0];
        string[] sData = map.Split(',');
        GameManager.gameManager.isCharacter = false;
        Load_Message.SetActive(false);
        LoadSlot.SetActive(false);
        this.gameObject.SetActive(false);
        LoadingSceneController.Instance.LoadScene(sData[1]);
    }
    public void LoadNoMessage()
    {
        Load_Message.gameObject.SetActive(false);
        ClickNum = -1;
    }

    public void CallSaveMassage(int _num)
    {
        Save_Message.gameObject.SetActive(true);
        ClickNum = _num;

    }
    public void CallLoadMassage(int _num)
    {
        if (UserDataManager.instance.LoadData(_num) == null || UserDataManager.instance.LoadData(_num) == "")
            return;
        Load_Message.gameObject.SetActive(true);
        ClickNum = _num;
    }


    public void CloseOption()
    {
        this.gameObject.SetActive(false);
    }
    public void OpenMainMenu()
    {
        Main_Message.gameObject.SetActive(true);
    }
    public void OpenSoundControl()
    {
        SoundControl.SetActive(true);
    }
    public void Main_Message_Yes()
    {
        UIManager.uimanager.UiObj.SetActive(false);
        Main_Message.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
        GameManager.gameManager.isCharacter = false;        
        LoadingSceneController.Instance.LoadScene("Main");
    }
    public void Main_Message_No()
    {
        Main_Message.gameObject.SetActive(false);
    }
    public void OpenSave()
    {
        SaveSlot.gameObject.SetActive(true);
        OpenSavedGames();
    }
    public void OpenLoad()
    {
        LoadSlot.gameObject.SetActive(true);
        OpenLoadGames();
    }
    public void OpenSavedGames()
    {       

        for (int i = 0; i < SaveData.Count; i++)
        {

            if (UserDataManager.instance.LoadData(i) == null)
            {
                SaveData[i].Name.text = "Empty";
                SaveData[i].PlayTime.text = string.Empty;
                SaveData[i].StartPos.text = string.Empty;
            }
            else
            {
                string[] Data = UserDataManager.instance.LoadData(i).Split('\n');
                string[] sData = Data[0].Split(',');

                SaveData[i].Name.text = sData[0];
                SaveData[i].PlayTime.text = "0";
                SaveData[i].StartPos.text = sData[1];
            }

        }


    }
    public void OpenLoadGames()
    {

        for (int i = 0; i < SaveData.Count; i++)
        {

            if (UserDataManager.instance.LoadData(i) == null)
            {
                LoadData[i].Name.text = "Empty";
                LoadData[i].PlayTime.text = string.Empty;
                LoadData[i].StartPos.text = string.Empty;
            }
            else
            {
                string[] Data = UserDataManager.instance.LoadData(i).Split('\n');
                string[] sData = Data[0].Split(',');

                LoadData[i].Name.text = sData[0];
                LoadData[i].PlayTime.text = "0";
                LoadData[i].StartPos.text = sData[1];
            }

        }


    }
   
  
}
