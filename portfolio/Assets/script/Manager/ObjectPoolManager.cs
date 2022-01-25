using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager objManager;
    public GameObject UIEffect;   
    //공격 이펙트
    public List<GameObject> Effect = new List<GameObject>();
    //데미지텍스트 이펙트
    public List<TextMeshProUGUI> DamageEffect = new List<TextMeshProUGUI>();    
    public TextMeshProUGUI Text;    
    public Dictionary<GameObject, Vector3> EffectPos_Dic = new Dictionary<GameObject, Vector3>();
    //클릭 이펙트
    public List<GameObject> ClickEffect_list = new List<GameObject>();
    public string Target_name = string.Empty;
    //닉네임 
    public TextMeshProUGUI NickName;
    public List<TextMeshProUGUI> NickNameList = new List<TextMeshProUGUI>();
    public List<Image> EnermyHpBar = new List<Image>();
    public Image Hp_Bar;
    public GameObject Nick;
    //buf 이미지
    public bufimage buf_image;
    public List<bufimage> buf_list = new List<bufimage>();
    public ScrollRect bufline;
    public GameObject ClcikFolder;
    public TextMeshProUGUI string_one;
    public List<TextMeshProUGUI> stringEffect_list = new List<TextMeshProUGUI>();
    

    //Quest마크 관리

    public List<GameObject> QuestMarkList = new List<GameObject>();

    //미니맵 이미지관리
    public List<Image> Mini_Dotn_list = new List<Image>();
    public List<Image> Mini_Dotm_list = new List<Image>();
    public Image Mini_Dot;
    public GameObject Mini_Dot_Map;
    public GameObject Mini_Dot_Map_M;

    private void Awake()
    {
        if(objManager == null)
        {
            objManager =this;
            DontDestroyOnLoad(gameObject);
            Load_ClickEffect();
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public void Load_ClickEffect()                               //Click이펙트 불러 넣기
    {

        GameObject[] obj = Resources.LoadAll<GameObject>("Click/");
        for (int i = 0; i < obj.Length; i++)
        {
            GameObject click = Instantiate(obj[i]);
            click.gameObject.name = obj[i].name;
            click.gameObject.transform.SetParent(ClcikFolder.transform);
            click.gameObject.SetActive(false);
            ObjectPoolManager.objManager.ClickEffect_list.Add(click);

        }


    }

    public GameObject QuestMarkPooling(string _name)
    {

        foreach (GameObject one in QuestMarkList)
        {

            if (one.activeSelf == false && one.name.Equals(_name))
            {

                one.SetActive(true);
                return one;
            }


        }

        GameObject newone = GameManager.gameManager.GetResource("QuestMark", _name);
        GameObject obj = Instantiate(newone);
        obj.name = _name;        
        QuestMarkList.Add(obj);
        obj.transform.SetParent(this.transform);
        return obj;

    }

    public void MessageEffect(string _text)
    {
        TextMeshProUGUI one = stringEffect();
        one.color = Color.blue;
        StartCoroutine(stringMove(one, _text));
        StartCoroutine(WaitForstring(one.gameObject));
        
    }
    IEnumerator stringMove(TextMeshProUGUI _string, string _text)
    {

        Vector3 Pos = Character.Player.transform.position;
        Pos.y += 1f;
        _string.fontSize = 25;
        _string.text = _text;
        while (_string.gameObject.activeSelf == true)
        {
            
            _string.transform.position = Camera.main.WorldToScreenPoint(new Vector3(Pos.x, Pos.y += Time.deltaTime * 0.6f, Pos.z));
            _string.fontSize += Time.deltaTime * 20f;
            _string.alpha -= Time.deltaTime * 0.6f;
            yield return null;

        }

        yield return null;

    }
    IEnumerator WaitForstring(GameObject _string)
    {
        yield return new WaitForSeconds(2.5f);        
        _string.SetActive(false);
    }


    public TextMeshProUGUI stringEffect()  //레벨업
    {
        foreach (TextMeshProUGUI one in stringEffect_list)
        {
            if (one.gameObject.activeSelf == false)
            {
                
                one.gameObject.SetActive(true);
                one.alpha = 1;
                one.fontSize = 34;
                return one;
            }
        }
        TextMeshProUGUI tmp = Instantiate(string_one);
        tmp.transform.localScale = new Vector3(1f, 1f, 1f);
        tmp.transform.SetParent(UIEffect.transform);
        stringEffect_list.Add(tmp);
        return tmp;
    }

    public bufimage PoolingbufControl()
    {
        foreach(bufimage one in buf_list)
        {
            if(one.gameObject.activeSelf == false)
            {                
                one.gameObject.SetActive(true);
                return one;
            }
        }
        bufimage tmp = Instantiate(buf_image);
        tmp.transform.localScale = new Vector3(1f, 1f, 1f);
        tmp.transform.SetParent(bufline.content.transform);
        buf_list.Add(tmp);
        return tmp;

    }       //버프이미지
   
    // Update is called once per frame
    public Image PoolingHpBar()
    {
        foreach(Image one in EnermyHpBar)
        {
            if(one.gameObject.activeSelf == false)
            {
                one.gameObject.SetActive(true);
                return one;
            }
        }
        Image tmp = Instantiate(Hp_Bar);
        tmp.transform.SetParent(Nick.transform);
        tmp.transform.localScale = new Vector3(1f, 1f, 1f);
        EnermyHpBar.Add(tmp);

        return tmp;
    }           // HP바 관리
    public Image PoolingMiniDot_n()
    {
        foreach (Image one in Mini_Dotn_list)
        {
            if (one.gameObject.activeSelf == false)
            {
                one.gameObject.SetActive(true);
                return one;
            }
        }
        Image tmp = Instantiate(Mini_Dot);
        tmp.transform.localPosition = Vector2.zero;
        tmp.transform.SetParent(Mini_Dot_Map.transform);
        tmp.transform.localScale = new Vector3(1f, 1f, 1f);
        Mini_Dotn_list.Add(tmp);
        return tmp;
    }       //좌상단 미니맵
    public Image PoolingMiniDot_M()
    {
        foreach (Image one in Mini_Dotm_list)
        {
            if (one.gameObject.activeSelf == false)
            {
                one.gameObject.SetActive(true);
                return one;
            }
        }
        Image tmp = Instantiate(Mini_Dot);
        tmp.transform.localPosition = Vector2.zero;
        tmp.transform.SetParent(Mini_Dot_Map_M.transform);
        tmp.transform.localScale = new Vector3(1f, 1f, 1f);
        Mini_Dotm_list.Add(tmp);
        return tmp;
    }       //메인 미니맵
    public TextMeshProUGUI PoolingNickName()    //닉네임
    {
        foreach(TextMeshProUGUI one in NickNameList) 
        {
            if(one.gameObject.activeSelf == false)
            {
                one.gameObject.SetActive(true);
                return one;
            }
        }

        TextMeshProUGUI tmp = Instantiate(NickName);
        tmp.transform.SetParent(Nick.transform);
        tmp.transform.localScale = new Vector3(1f, 1f,1f);
        NickNameList.Add(tmp);

        return tmp;


    }

    public TextMeshProUGUI PoolingDamageImage(Color _color,float _size =1)
    {
        foreach(TextMeshProUGUI one in DamageEffect)
        {
           if(one.gameObject.activeSelf == false)
           {
                one.gameObject.SetActive(true);
                one.color = _color;
                one.transform.localScale = new Vector3(_size, _size, _size);



                return one;
           }
        }
        
        TextMeshProUGUI obj = Instantiate(Text);
        obj.name = "Damage";
        obj.transform.localScale = new Vector3(_size, _size, _size);
        obj.color = _color;
        obj.transform.SetParent(UIEffect.transform);
        DamageEffect.Add(obj);
        return obj;
    }   //데미지 숫자 이미지
    public void LoadDamage(GameObject Target, float Damage,Color _Color,float _size) // 데미지 효과
    {
        TextMeshProUGUI E_text = PoolingDamageImage(_Color,_size);
        E_text.text = Damage.ToString();
        GameObject Ef = E_text.gameObject;
        StartCoroutine(MoveEffect(Ef, Target));
        StartCoroutine(WaitForIt(Ef));
    }
    IEnumerator WaitForIt(GameObject obj)
    {
        
        yield return new WaitForSeconds(1f);        
        obj.SetActive(false);
    }
    IEnumerator MoveEffect(GameObject Ef,GameObject Target) //데미지 움직임
    {
        Vector3 tmp = Target.transform.position;
        float y = tmp.y;
        float x = Random.Range(tmp.x - 0.5f, tmp.x + 0.5f);

        while (true)
        {
            if (Ef.gameObject.activeSelf == false)
            {

                yield break;
            }

            Ef.transform.position = Camera.main.WorldToScreenPoint(new Vector3(x, y += Time.deltaTime, tmp.z) + new Vector3(0f, 2f, 0f));
            

            yield return null;
        }
    }


    public void ClickMove(string _name, Vector3 _pos)       //이동
    {
        for(int i=0; i< ClickEffect_list.Count; i++)
        {
            if(ClickEffect_list[i].gameObject.activeSelf == true && ClickEffect_list[i].gameObject.name != _name)
            {
                ClickEffect_list[i].transform.SetParent(this.transform);
                ClickEffect_list[i].gameObject.SetActive(false);
            }
            if(ClickEffect_list[i].gameObject.activeSelf == false && ClickEffect_list[i].gameObject.name != _name)
            {
                ClickEffect_list[i].gameObject.SetActive(true);
                ClickEffect_list[i].transform.SetParent(this.transform);
                ClickEffect_list[i].gameObject.SetActive(false);
            }
            else if(ClickEffect_list[i].gameObject.name == _name)
            {
                ClickEffect_list[i].gameObject.SetActive(true);                                
                ClickEffect_list[i].gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                _pos += new Vector3(0f, 0.1f, 0f);
                ClickEffect_list[i].gameObject.transform.position = _pos;
            }
            
        }
    }
    
    public void ClickEffect(string _name, Transform _Target)
    {        
        
        for (int i = 0; i < ClickEffect_list.Count; i++)
        {
            if (ClickEffect_list[i].gameObject.activeSelf == true && ClickEffect_list[i].gameObject.name == _name && Target_name == _Target.name)
            {
                return;
            }            
        }

        for (int i = 0; i < ClickEffect_list.Count; i++)
        {
            if (ClickEffect_list[i].gameObject.activeSelf == true )
            {
                ClickEffect_list[i].gameObject.SetActive(false);
                
            }
        }


        for (int j = 0; j < ClickEffect_list.Count; j++)
        {
            if(ClickEffect_list[j].name == _name)
            {
                
                ClickEffect_list[j].gameObject.SetActive(true);
                ClickEffect_list[j].transform.SetParent(_Target);
                ClickEffect_list[j].gameObject.transform.localPosition = new Vector3(0f,0.1f,0f);
                ClickEffect_list[j].gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                Target_name = _Target.name;

                return;
            }
        }







    }   //타겟선택
    public GameObject EffectPooling(string _name)
    {
       foreach(GameObject one in Effect)
        {          

            if (one.activeSelf == false && one.name.Equals(_name))
            {
                
                one.SetActive(true);
                return one;
            }
            
            
        }
        
        GameObject newone = GameManager.gameManager.GetResource("Effect",_name);
        GameObject obj = Instantiate(newone);
        obj.name = _name;
        EffectPos_Dic.Add(obj, obj.transform.position);
        Effect.Add(obj);
        obj.transform.SetParent(this.transform);
        

        
        return obj;
        
    }       //이펙트 관리
 
    

    public void PoolingReset_Load()
    {
        foreach(GameObject one in Effect)
        {
            if (one.activeSelf == true)
                one.SetActive(false);
        }
        foreach(TextMeshProUGUI one in DamageEffect)
        {
            if (one.gameObject.activeSelf == true)
                one.gameObject.SetActive(false);
        }
        foreach(TextMeshProUGUI one in NickNameList)
        {
            if (one.gameObject.activeSelf == true)
                one.gameObject.SetActive(false);
        }
        foreach(TextMeshProUGUI one in DamageEffect)
        {
            if (one.gameObject.activeSelf == true)
                one.gameObject.SetActive(false);
        }
        foreach(Image one in EnermyHpBar)
        {
            if (one.gameObject.activeSelf == true)
                one.gameObject.SetActive(false);
        }
        foreach(bufimage one in buf_list)
        {
            if (one.gameObject.activeSelf == true)
                one.gameObject.SetActive(false);
        }
        foreach (GameObject one in ClickEffect_list)
        {
            one.SetActive(true);
            one.transform.SetParent(this.transform);
            one.SetActive(false);
        }
        foreach (GameObject one in QuestMarkList)
        {
            if (one.activeSelf == true)
            {
                one.SetActive(false);
            }
        }
        foreach (Image one in Mini_Dotm_list)
        {
            if (one.gameObject.activeSelf == true)
                one.gameObject.SetActive(false);
        }

        foreach (Image one in Mini_Dotn_list)
        {
            if (one.gameObject.activeSelf == true)
                one.gameObject.SetActive(false);
        }

    }
    public void PoolingReset_newArea()
    {
       
        foreach (TextMeshProUGUI one in DamageEffect)
        {
            if (one.gameObject.activeSelf == true)
                one.gameObject.SetActive(false);
        }
        foreach (TextMeshProUGUI one in NickNameList)
        {
            if (one.gameObject.activeSelf == true)
                one.gameObject.SetActive(false);
        }       
        foreach (Image one in EnermyHpBar)
        {
            if (one.gameObject.activeSelf == true)
                one.gameObject.SetActive(false);
        }       
        foreach (GameObject one in ClickEffect_list)
        {
           one.SetActive(true);
            one.transform.SetParent(this.transform);
            one.SetActive(false);
        
        }
        foreach (GameObject one in QuestMarkList)
        {
            if (one.activeSelf == true)
            {
                one.SetActive(false);
            }
        }
        foreach (Image one in Mini_Dotm_list)
        {
            if (one.gameObject.activeSelf == true)
                one.gameObject.SetActive(false);
        }

        foreach (Image one in Mini_Dotn_list)
        {
            if (one.gameObject.activeSelf == true)
                one.gameObject.SetActive(false);
        }
    }
}
