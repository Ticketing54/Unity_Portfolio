using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Minimap_Maximum : MonoBehaviour
{
    //Áß¾Ó ¹Ì´Ï¸Ê
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
    List<Image>runningDots;


    Character character;
    
    void Update()
    {
        MoveDot();
    }

    private void Awake()
    {   
        runningDots = new List<Image>();
        dotsPool = new PoolData<Image>(example, this.gameObject, "Dots");
    }
    private void OnEnable()
    {
        SetMapInfo();                
        OpenDotSetting();
    }
    private void OnDisable()
    {   
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
  


    public void MoveDot()      //Áß¾Ó ¹Ì´Ï¸Ê À§Ä¡
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
        AddNpcMiniDot();
        AddPotalMiniDot();
    }
    void CloseDotSetting()
    {
        StopAllCoroutines();
        for (int i = 0; i < runningDots.Count; i++)
        {
            dotsPool.Add(runningDots[i]);
        }
    }

    void AddNpcMiniDot()
    {
        List<Npc> npcList = ObjectManager.objManager.GetNpcList();
        for (int i = 0; i < npcList.Count; i++)
        {
            Image dot = dotsPool.GetData();
            dot.sprite = ResourceManager.resource.GetImage("DotN");
            dot.transform.SetParent(activeDotParent.transform);
            StartCoroutine(CoMoveDots(npcList[i], dot));
            runningDots.Add(dot);            
        }
    }
    void AddPotalMiniDot()
    {
        List<Potal> potalList = ObjectManager.objManager.GetPotalList();
        for (int i = 0; i < potalList.Count; i++)
        {
            Image dot = dotsPool.GetData();
            dot.transform.SetParent(activeDotParent.transform);
            dot.rectTransform.anchoredPosition = MoveDotPosition(potalList[i].transform.position);

            dot.sprite = ResourceManager.resource.GetImage("DotP");
            runningDots.Add(dot);
            
        }
        
    }
 
    IEnumerator CoMoveDots(Unit _unit, Image _dot)
    {
        while (true)
        {
            _dot.rectTransform.anchoredPosition = MoveDotPosition(_unit.transform.position);

            yield return null;
        }
    }

   

    


}
