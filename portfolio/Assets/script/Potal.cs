using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Potal : MonoBehaviour,InteractInterface
{
    MAPTYPE mapType;
    string mapName;
    Vector3 pos;
    string cutScene;
    public string MapName { get => mapName; }
    
    public  Vector3 Pos { get => pos; }
    
    public string CutScene { get => cutScene; }
    public MAPTYPE MapType { get => mapType; }

    bool interactSuccess;
    bool nearUnit;
    Coroutine waitForDoing;

    public void SettingPotal(string[] _info)
    {
        mapName = _info[0];
        
        MAPTYPE _type;
        if(Enum.TryParse(_info[1],out _type))
        {
            mapType = _type;
        }
        else
        {
            Debug.LogError("mapType Error");
        }
        transform.position = new Vector3(float.Parse(_info[2]), float.Parse(_info[3]), float.Parse(_info[4]));
        pos = new Vector3(float.Parse(_info[5]), float.Parse(_info[6]), float.Parse(_info[7]));

        if (!string.IsNullOrEmpty(_info[11]))
        {
            cutScene = _info[11];
        }
        interactSuccess = false;
        nearUnit = false;

    }

    private void OnDisable()
    {
        GameManager.gameManager.character.RemoveInteract(this);
    }
    private void Update()
    {
        if (Distance < 6 &&nearUnit == false)
        {
            nearUnit = true;
            GameManager.gameManager.character.AddNearInteract(this);
        }
        else if(Distance>=6 && nearUnit == true)
        {
            nearUnit = false;
            GameManager.gameManager.character.RemoveInteract(this);
        }
    }
    public void interact()
    {
        if(interactSuccess == true)
        {
            return;
        }
        MoveToScene();
    }

    public float DISTANCE
    {
        get
        {
            if(GameManager.gameManager.character != null)
            {
                
                return Vector3.Distance(transform.position, GameManager.gameManager.character.transform.position);
            }
            else
            {
                return 100;
            }   
        }
    }

    public float Distance
    {
        get
        {
            if (GameManager.gameManager.character != null)
            {

                return Vector3.Distance(transform.position, GameManager.gameManager.character.transform.position);
            }
            else
            {
                return 100;
            }
        }
    }

    void MoveToScene()
    {
        if (waitForDoing != null)
        {
            return;
        }

        waitForDoing = StartCoroutine(CoMoveToScene());
    }

    IEnumerator CoMoveToScene()
    {
        string waitForDoingText = mapName + " 으로 이동 하는중 입니다.";
        yield return StartCoroutine(CoWaitForDoing(5f, waitForDoingText));


        if (interactSuccess == true)
        {
            GameManager.gameManager.MoveToScene(mapName, pos, MapType, CutScene);
            interactSuccess = false;
        }
        waitForDoing = null;
    }

    IEnumerator CoWaitForDoing(float _timerMax, string _text)
    {
        UIManager.uimanager.OpenWaitForDoing(_text);
        interactSuccess = true;
        float timer = 0.01f;
        while (timer <= _timerMax)
        {
            yield return null;
            if (Input.anyKeyDown)
            {
                interactSuccess = false;
                UIManager.uimanager.ExitWaitForDoing();
                yield break;
            }
            UIManager.uimanager.RunningWaitForDoing(timer / _timerMax);
            timer += Time.deltaTime;
            
        }
        
        UIManager.uimanager.ExitWaitForDoing();
    }
}
