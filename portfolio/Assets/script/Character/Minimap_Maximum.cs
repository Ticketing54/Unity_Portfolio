using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Minimap_Maximum : MonoBehaviour
{
    //중앙 미니맵
    [SerializeField]
    GameObject activeDotParent;
    [SerializeField]
    Image mapImage;
    [SerializeField]
    Image playerDot;
    [SerializeField]
    Image example;

    float mapSize_X = 0;
    float mapSize_Y = 0;

    string mapName = string.Empty;


    PoolData<Image> dotsPool;
    Dictionary<Unit, Coroutine> runningCo;
    Dictionary<Unit, Image> runningDots;


    Character character;
    
    void Update()
    {
        MoveDot();
    }

    private void Awake()
    {
        runningCo = new Dictionary<Unit, Coroutine>();
        runningDots = new Dictionary<Unit, Image>();
        dotsPool = new PoolData<Image>(example, this.gameObject, "Dots");
    }
    private void OnEnable()
    {
        UIManager.uimanager.uicontrol_On += AddMiniDot;
        UIManager.uimanager.uicontrol_Off += RemoveMiniDot;
        UIManager.uimanager.miniMapSetting += SetStartMapInfo;
        SetMapInfo();
        OpenDotSetting();
    }
    private void OnDisable()
    {
        UIManager.uimanager.uicontrol_On -= AddMiniDot;
        UIManager.uimanager.uicontrol_Off -= RemoveMiniDot;
        UIManager.uimanager.miniMapSetting -= SetStartMapInfo;
        CloseDotSetting();
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
    void SetStartMapInfo(string _mapName, float _xPos, float _yPos)
    {
        mapName = _mapName;
        mapSize_X = _xPos;
        mapSize_Y = _yPos;
        character = GameManager.gameManager.character;
    }



    public void MoveDot()      //중앙 미니맵 위치
    {
        playerDot.rectTransform.anchoredPosition = MoveDotPosition(character.transform.position);

    }
    public Vector2 MoveDotPosition(Vector3 Pos)    
    {
        Vector2 End = Vector2.zero;
        float x = (Pos.x / mapSize_X) * 700;
        float y = (Pos.z / mapSize_Y) * 700;
        End.Set(x, y);
        return End;

    }

    void OpenDotSetting()
    {
        HashSet<Unit> nearUnit = character.nearUnit;

        foreach (Unit dot in nearUnit)
        {
            AddMiniDot(dot);
        }
    }
    void CloseDotSetting()
    {
        List<Unit> activeDots = new List<Unit>(runningCo.Keys);

        foreach (Unit dot in activeDots)
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
            Debug.LogError("없는 코루틴을 찾고있습니다. : Minimap_MiniMum");
        }

        if (runningDots.ContainsKey(_unit))
        {
            Image dot = runningDots[_unit];
            dotsPool.Add(dot);
            runningDots.Remove(_unit);
        }
        else
        {
            Debug.LogError("없는 Dot을 찾고있습니다. : Minimap_MiniMum");
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


    


}
