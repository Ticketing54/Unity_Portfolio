using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UiEffectManager : MonoBehaviour
{
    
    PoolingManager<TextMeshProUGUI> textPool;           // UI Effect 텍스트 풀링
    PoolingManager<Image> imagePool;                    // UI Effect 이미지 풀링 // 이모티콘 때 사용    
    PoolData<MiniDialogue> miniDailog;
    #region Samples
    [SerializeField]
    TextMeshProUGUI sampleDamageEffect;                 // 데미지 이펙트   
    [SerializeField]
    TextMeshProUGUI sampleNicName;                      // 닉네임
    [SerializeField]
    TextMeshProUGUI sampleLevelup;                      // 레벨업 텍스트
    [SerializeField]
    Image           sampleHpBar;                        // Hpbar    
    [SerializeField]
    MiniDialogue    sampleDialogText;                   // 텍스트
    #endregion
    Dictionary<UnitUiInfo, Coroutine>       runningCo;
    Dictionary<UnitUiInfo, TextMeshProUGUI> runningNickName;
    Dictionary<UnitUiInfo, Image>           runningHpBar;

    
    




    
    private void Awake()
    {        
        textPool            = new PoolingManager<TextMeshProUGUI>(this.gameObject);
        imagePool           = new PoolingManager<Image>(this.gameObject);
        runningCo           = new Dictionary<UnitUiInfo, Coroutine>();
        runningHpBar        = new Dictionary<UnitUiInfo, Image>();
        runningNickName     = new Dictionary<UnitUiInfo, TextMeshProUGUI>();        
        miniDailog          = new PoolData<MiniDialogue>(sampleDialogText, this.gameObject, "MiniDialog");

        textPool.Add("DamageEffect", sampleDamageEffect);
        textPool.Add("NickName", sampleNicName);
        imagePool.Add("HpBar", sampleHpBar);
        textPool.Add("LevelUp", sampleLevelup);
    }
    private void Start()
    {
        GameManager.gameManager.moveSceneReset += MoveToNextScene;
        UIManager.uimanager.ALevelUpEffect += LevelUpEffect;
        UIManager.uimanager.ALoadDamageEffect += LoadDamageEffect;
        UIManager.uimanager.AAddNearUnitOnUi += OnUiControl;
        UIManager.uimanager.ARemoveNearUnitUi += OffUiControl;
    }

    #region NickName

    void OnUiControl(UnitUiInfo _unit)
    {
        TextMeshProUGUI nickName = textPool.GetData("NickName");
        nickName.text = _unit.UnitName();
        if (_unit.IsEnermy())
        {
            nickName.color = Color.red;
        }
        else
        {
            nickName.color = Color.green;
        }
        AddRunningNickName(_unit, nickName);

        Image hpBar = null;
        if (_unit.HpMax > 0)
        {
            hpBar = imagePool.GetData("HpBar");
            AddRunningHpBar(_unit, hpBar);
            hpBar.transform.SetParent(nickName.transform);
            hpBar.rectTransform.anchoredPosition = new Vector2(0, -120f);
        }
        Coroutine activeCoroutine;
        if (hpBar == null)
        {
            activeCoroutine = StartCoroutine(CoRunningUi(_unit, nickName));
        }
        else
        {
            activeCoroutine = StartCoroutine(CoRunningUi(_unit, nickName, hpBar));
        }
        
        AddRunningCO(_unit, activeCoroutine);
    }
    void OffUiControl(UnitUiInfo _unit)
    {
        RemoveRunningCo(_unit);
        RemoveRunningHpBar(_unit);
        RemoveRunningNickName(_unit);
    }
    public void MoveToNextScene()
    {
        StopAllCoroutines();
        List<Image> runningList_Hp = new List<Image>(runningHpBar.Values);
        List<TextMeshProUGUI> runningList_Nick = new List<TextMeshProUGUI>(runningNickName.Values);        
        foreach(Image hp in runningList_Hp)
        {
            imagePool.Add("HpBar", hp);
        }
        foreach (TextMeshProUGUI nic in runningList_Nick)
        {
            textPool.Add("NickName", nic);
        }
        runningHpBar.Clear();
        runningList_Nick.Clear();
        runningCo.Clear();
    }
    void AddRunningCO(UnitUiInfo _parent,Coroutine _co)
    {
        if (runningCo.ContainsKey(_parent))
        {
            Debug.LogError("이미 있는 코루틴입니다.");
        }
        else
        {
            runningCo.Add(_parent, _co);
        }
    }
    void RemoveRunningCo(UnitUiInfo _parent)
    {
        if (runningCo.ContainsKey(_parent))
        {
            Coroutine popCo = runningCo[_parent];
            runningCo.Remove(_parent);
            StopCoroutine(popCo);
        }
        else
        {
            Debug.Log("없는 코루틴을 정지하려 합니다.");
        }
    }
    void AddRunningNickName(UnitUiInfo _parent,TextMeshProUGUI _nickName)
    {
        if (runningNickName.ContainsKey(_parent))
        {
            Debug.LogError("이미 있는 닉네임입니다.");
        }
        else
        {
            runningNickName.Add(_parent, _nickName);
        }
    }
    void AddRunningHpBar(UnitUiInfo _parent,Image _hpbar)
    {
        if (runningHpBar.ContainsKey(_parent))
        {
            Debug.LogError("이미있는 hpbar 입니다");
        }
        else
        {
            runningHpBar.Add(_parent, _hpbar);
        }
    }

    void RemoveRunningNickName(UnitUiInfo _parent)
    {
        if (runningNickName.ContainsKey(_parent))
        {
            TextMeshProUGUI popNick = runningNickName[_parent];
            textPool.Add("NickName", popNick);
            runningNickName.Remove(_parent);
        }
        else
        {
            Debug.Log("없는 닉네임을 풀로 되돌리려합니다.");
        }
    }

    void RemoveRunningHpBar(UnitUiInfo _parent)
    {
        if (runningHpBar.ContainsKey(_parent))
        {
            Image popImage = runningHpBar[_parent];
            imagePool.Add("HpBar", popImage);
            runningHpBar.Remove(_parent);
        }
        else
        {
            Debug.Log("없는 HpBar를 풀로 되돌리려합니다.");
        }
    }
   
    IEnumerator CoRunningUi(UnitUiInfo _unit,TextMeshProUGUI _nickName,Image _hpbar)
    {
        while (true)
        {          
            _nickName.rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(_unit.CurPostion() + new Vector3(0f, _unit.Nick_YPos(), 0f));
            yield return null;

            if (_unit.HpCur <= 0)
            {
                _hpbar.gameObject.SetActive(false);
            }
            else
            {
                _hpbar.fillAmount = _unit.HpCur / _unit.HpMax;
            }       
        }
    }

    IEnumerator CoRunningUi(UnitUiInfo _mob, TextMeshProUGUI _nickName)
    {
        while (true)
        {
            _nickName.rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(_mob.CurPostion() + new Vector3(0f, _mob.Nick_YPos(), 0f));
            yield return null;
        }
    }

    #endregion

    #region LevelUpEffect
    void LevelUpEffect(GameObject _Target)
    {
        TextMeshProUGUI LevelupText = textPool.GetData("LevelUp");
        StartCoroutine(LevelupMotion(LevelupText, _Target));
    }
    IEnumerator LevelupMotion(TextMeshProUGUI _levelUpText, GameObject _Target)
    {
        Vector3 targetPos = _Target.transform.position;
        float timer = 0f;
        float presetY = targetPos.y+1f;
        _levelUpText.fontSize = 50;
        _levelUpText.alpha = 1f;

        while (timer <= 2f)
        {   
            _levelUpText.transform.position = Camera.main.WorldToScreenPoint(new Vector3(targetPos.x, presetY += Time.deltaTime*2f, targetPos.z));
            _levelUpText.fontSize += Time.deltaTime * 20f;
            _levelUpText.alpha -= Time.deltaTime * 2f;
            timer += Time.deltaTime;
            yield return null;
        }

        

        textPool.Add("LevelUp", _levelUpText);

        yield break;
    }

    #endregion
    #region DamageEffect

    void LoadDamageEffect(int _damage, GameObject _target, bool _isCritical = false)
    {
        TextMeshProUGUI DmgEffect = textPool.GetData("DamageEffect");
        DmgEffect.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        DmgEffect.fontSize = 100;
        DmgEffect.text = _damage.ToString();

        if (_isCritical == true)
        {
            DmgEffect.color = Color.red;
            DmgEffect.fontSize = 200;
        }

        StartCoroutine(DamageEffecting(DmgEffect, _target.transform.position));
    }

    IEnumerator DamageEffecting(TextMeshProUGUI _text, Vector3 _target)
    {
        float timer = 0f;
        Vector3 targetPos = _target;
        float preset = targetPos.y + 1f;
        while (timer <= 1f)
        {
            timer += Time.deltaTime;
            
            _text.transform.position = Camera.main.WorldToScreenPoint(new Vector3(targetPos.x, preset += Time.deltaTime*2f, targetPos.z));
            _text.alpha -= Time.deltaTime*2;
            yield return null;
        }

        TextReset(_text);

        textPool.Add("DamageEffect", _text);

        yield break;
    }
    #endregion

    void TextReset(TextMeshProUGUI _Effect)
    {
        _Effect.fontSize = 100;
        _Effect.color = Color.white;
        _Effect.alpha = 1;
    }
    
}
