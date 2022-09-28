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
    Dictionary<UnitUiInfo, Coroutine> runningCo;
    Dictionary<UnitUiInfo, Image>     runningDots;

    private void Awake()
    {
        runningCo   = new Dictionary<UnitUiInfo, Coroutine>();
        runningDots = new Dictionary<UnitUiInfo, Image>();
        dotsPool    = new PoolData<Image>(example, this.gameObject, "Dots");
    }
    private void OnEnable()
    {
        SetMapInfo();        
        OpenDotSetting();
        UIManager.uimanager.AAddNearUnit += AddMiniDot;
        UIManager.uimanager.ARemoveNearUnit+= RemoveMiniDot;
    }
    private void OnDisable()
    {        
        CloseDotSetting();
        UIManager.uimanager.AAddNearUnit -= AddMiniDot;
        UIManager.uimanager.ARemoveNearUnit -= RemoveMiniDot;
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
        List<UnitUiInfo> units = UIManager.uimanager.NearUnitList;

        for (int i = 0; i < units.Count; i++)
        {
            AddMiniDot(units[i]);
        }
    }

    void CloseDotSetting()
    {
        StopAllCoroutines();

        List<Image> activeDots = new (runningDots.Values);
        for (int i = 0; i < activeDots.Count; i++)
        {
            dotsPool.Add(activeDots[i]);
        }
        runningCo.Clear();
        runningDots.Clear();
    }

    void AddMiniDot(UnitUiInfo _unit)
    {   
        Image dot = dotsPool.GetData();
        dot.sprite = ResourceManager.resource.GetImage(_unit.MiniDotSpriteName());
        dot.transform.SetParent(activeDotParent.transform);        
        Coroutine co = StartCoroutine(CoMoveDots(_unit, dot));
        runningDots.Add(_unit, dot);
        runningCo.Add(_unit, co);
    }
    void RemoveMiniDot(UnitUiInfo _unit)
    {
        if (runningCo.ContainsKey(_unit))
        {
            StopCoroutine(runningCo[_unit]);
        }

        if(runningDots.ContainsKey(_unit))
        {
            Image dot = runningDots[_unit];
            runningDots.Remove(_unit);
            dotsPool.Add(dot);
        }
    }





    IEnumerator CoMoveDots(UnitUiInfo _unit, Image _dot)
    {   
        while (true)
        {
            _dot.rectTransform.anchoredPosition = MoveDotPosition(_unit.CurPostion());
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
