using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using System;

public class MainManager : MonoBehaviour
{

    public static MainManager mainManager;

    private void Awake()
    {
        if(mainManager == null)
        {
            mainManager = this;
        }
        
    }
    public bool ClickMain = false;
    public Image Main;
    public CanvasGroup mainGroup;
    public CanvasGroup LoadGroup;
    public Image SaveSlot;
    public GameObject select;
    public TextMeshProUGUI CAONS;
    public int _num;
    
    public List<MainSaveSlot> SaveData = new List<MainSaveSlot>();
    bool turnalPha = false;


    public Camera subCamera;

  


    //새게임
    public Image MakeNick;
    public GameObject NewGameMessage;
    public CanvasGroup NewGameFadeCanVas;
    public TMP_InputField WriteNick;
    //타임라인
    public PlayableDirector director;
    public GameObject Cameraline;

    private void Start()
    {
        CameraManager.cameraManager.enabled = false;
        subCamera.enabled = true;
        director.Play();        
    }

   

    void Update()
    {
        if (ClickMain == false && Input.GetMouseButtonDown(0))
        {
            CAONS.gameObject.SetActive(false);
            ClickMain = true;            
            StartCoroutine(StartMainmene());

        }
        if(CAONS.gameObject.activeSelf == true)
        {
            if(turnalPha == true)
            {
                if(CAONS.alpha >= 1)
                {
                    turnalPha = false;
                }
                CAONS.alpha += Time.deltaTime * 0.5f;
            }
            else
            {
                if (CAONS.alpha <= 0)
                {
                    turnalPha = true;
                }
                CAONS.alpha -= Time.deltaTime * 0.5f;
            }
        }



    }

  
    
    IEnumerator StartMainmene()
    {
        Main.gameObject.SetActive(true);
        while (mainGroup.alpha <1)
        {          

            mainGroup.alpha += Time.deltaTime*4f;



            yield return null;
        }
        yield return null;
    }
    public void OpenSavedGames()
    {
        
        StartCoroutine(Fadeslot());
        
        for (int i = 0; i < SaveData.Count; i++)
        {

            if(UserDataManager.instance.LoadData(i) ==null)
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
    IEnumerator Fadeslot()      // 로드 그룹 페이드인아웃
    {
        SaveSlot.gameObject.SetActive(true);
        while (LoadGroup.alpha <= 1)
        {

            LoadGroup.alpha += Time.deltaTime * 4;



            yield return null;
        }
        yield return null;
    }

    public void CloseSave()
    {
        SaveSlot.gameObject.SetActive(false);
    }   
    public void NewGame()
    {
        StartCoroutine(MakeNick_Fade());
         
    }
    public void NewGameStart()
    {
        subCamera.enabled = false;
        CameraManager.cameraManager.enabled = true;
        
        GameManager.gameManager.NewGame(WriteNick.text);
        
        
    }



    IEnumerator MakeNick_Fade()
    {
        MakeNick.gameObject.SetActive(true);
        while(true)
        {
            if(NewGameFadeCanVas.alpha >= 1f)
            {
                NewGameMessage.gameObject.SetActive(true);


                yield break;
            }


            NewGameFadeCanVas.alpha += Time.deltaTime ;




            yield return null;
        }
        
    }
    public void LoadGame(int _num)
    {
        Camera.main.transform.parent = null;
        if (UserDataManager.instance.LoadData(_num)==null)
        {
            NewGame();
            return;
        }           
        
        GameManager.gameManager.Player_num = _num;
        string[] Data = UserDataManager.instance.LoadData(_num).Split('\n');
        string[] sData = Data[0].Split(',');             
        
        LoadingSceneController.Instance.LoadScene(sData[1]);
    }

    
}
