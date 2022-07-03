using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap_Minimum : MonoBehaviour
{
    [SerializeField]
    Image mapImage;
    [SerializeField]
    Image playerDot;
    [SerializeField]
    ScrollRect scrollRect;
    [SerializeField]
    GameObject activeDotParent;
    [SerializeField]
    Image example;



    Character character;

    Vector2 Pos     = Vector2.zero;    
    string mapName  = string.Empty;
    float mapSize_X = 0f;
    float mapSize_Y = 0f;
    PoolData<Image> dotsPool;
    Dictionary<Unit, Coroutine> runningCo;
    Dictionary<Unit, Image>     runningDots;

    private void Awake()
    {
        runningCo = new Dictionary<Unit, Coroutine>();
        runningDots = new Dictionary<Unit, Image>();
        dotsPool = new PoolData<Image>(example, this.gameObject, "Dots");
    }
    private void OnEnable()
    {
        SetMapInfo();
        UIManager.uimanager.uicontrol_On   += AddMiniDot;
        UIManager.uimanager.uicontrol_Off  += RemoveMiniDot;        
        OpenDotSetting();
    }
    private void OnDisable()
    {
        UIManager.uimanager.uicontrol_On   -= AddMiniDot;
        UIManager.uimanager.uicontrol_Off  -= RemoveMiniDot;        
        CloseDotSetting();
    }
    private void Update()
    {
        MovePosition();
    }


    void SetMapInfo()
    {
        if (mapName != GameManager.gameManager.mapName)
        {
            mapName = GameManager.gameManager.mapName;
            mapImage.sprite = ResourceManager.resource.GetImage(mapName + "Map");
            mapSize_X = GameManager.gameManager.mapSizeX;
            mapSize_Y = GameManager.gameManager.mapSizeY;
            character = GameManager.gameManager.character;
        }
    }

    void OpenDotSetting()
    {
        character = GameManager.gameManager.character;
        HashSet<Unit> nearUnit = character.nearUnit;

        foreach(Unit dot in nearUnit)
        {
            AddMiniDot(dot);
        }
    }
    void CloseDotSetting()
    {
        List<Unit> activeDots = new (runningCo.Keys);

        foreach(Unit dot in activeDots)
        {
            RemoveMiniDot(dot);
        }

    }

    void AddMiniDot(Unit _unit)
    {
        Image dot = dotsPool.GetData();
        dot.transform.SetParent(activeDotParent.transform);
        Coroutine co = StartCoroutine(CoMoveDots(_unit, dot));
        runningDots.Add(_unit, dot);
        runningCo.Add(_unit, co);
    }
    void RemoveMiniDot(Unit _unit)
    {
        if (runningCo.ContainsKey(_unit))
        {
            Coroutine co = runningCo[_unit];
            StopCoroutine(co);
            runningCo.Remove(_unit);
        }
        else
        {
            Debug.Log("없는 코루틴을 찾고있습니다. : Minimap_MiniMum");
        }

        if (runningDots.ContainsKey(_unit))
        {
            Image dot = runningDots[_unit];
            dotsPool.Add(dot);
            runningDots.Remove(_unit);
        }
        else
        {
            Debug.Log("없는 Dot을 찾고있습니다. : Minimap_MiniMum");
        }

    }





    IEnumerator CoMoveDots(Unit _unit, Image _dot)
    {
        _dot.sprite = UnitDotsSprite(_unit);

        while (true)
        {
            _dot.rectTransform.anchoredPosition = MoveDotPosition(_unit.transform.position);

            yield return null;
        }
    }

    Sprite UnitDotsSprite(Unit _unit)
    {
        return null;
    }
   
    Vector2 MoveDotPosition(Vector3 Pos)     //좌상단 미니맵상 위치
    {
        Vector2 End = Vector2.zero;
        float x = (Pos.x / mapSize_X) * 900;
        float y = (Pos.z / mapSize_Y) * 900;

       
        End.Set(x, y);
        return End;

    }


    public void MovePosition()      //좌상단 캐릭터의 위치
    {
        float x = character.transform.position.x / mapSize_X;
        float y = character.transform.position.z / mapSize_Y;

        playerDot.rectTransform.rotation = Quaternion.Euler(0, 0, -Camera.main.transform.eulerAngles.y);

        Pos.Set(x, y);

        scrollRect.normalizedPosition = Pos;
    }

   
}
