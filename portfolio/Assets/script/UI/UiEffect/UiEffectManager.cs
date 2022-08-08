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
    Dictionary<GameObject, Coroutine>       runningCo;
    Dictionary<GameObject, TextMeshProUGUI> runningNickName;
    Dictionary<GameObject, Image>           runningHpBar;

    Dictionary<Unit, MiniDialogue> runningMiniDialog;    
    




    
    private void Awake()
    {        
        textPool            = new PoolingManager<TextMeshProUGUI>(this.gameObject);
        imagePool           = new PoolingManager<Image>(this.gameObject);
        runningCo           = new Dictionary<GameObject, Coroutine>();
        runningHpBar        = new Dictionary<GameObject, Image>();
        runningNickName     = new Dictionary<GameObject, TextMeshProUGUI>();
        runningMiniDialog   = new Dictionary<Unit, MiniDialogue>();        
        miniDailog          = new PoolData<MiniDialogue>(sampleDialogText, this.gameObject, "MiniDialog");

        textPool.Add("DamageEffect", sampleDamageEffect);
        textPool.Add("NickName", sampleNicName);
        imagePool.Add("HpBar", sampleHpBar);
        textPool.Add("LevelUp", sampleLevelup);



        GameManager.gameManager.moveSceneReset += MoveToNextScene;
    }

  
    public void TextingMiniDialog(Unit _target,string _text)
    {
        MiniDialogue dialog = miniDailog.GetData();
        dialog.transform.SetParent(this.gameObject.transform);        
        
        runningMiniDialog.Add(_target, dialog);
        _target.UsingDialog = true;
        StartCoroutine(CoTextingDialog(dialog, _text,_target));       
    }

  
    public IEnumerator CoTextingDialog(MiniDialogue _dialog,string _text,Unit _target)
    {
        float timer = 0f;
        int currentText  = 0;        
        while (currentText != _text.Length)
        {
            yield return null;
            _dialog.transform.position = Camera.main.WorldToScreenPoint(_target.transform.position + new Vector3(0f, 3f, 0f));
            

            timer += Time.deltaTime;

            if (timer >= 0.1f)
            {
                currentText++;
                timer =  timer-0.1f;

                string dialogText = _text.Substring(0, currentText);
                string emptyText = string.Empty;
                int count = _text.Length - currentText - 1;
                while (count >= 0)
                {
                    count--;
                    emptyText += "  ";
                }
                emptyText += '\r';
                _dialog.SetText(dialogText + emptyText);
            }


        }

        yield return new WaitForSeconds(1f);

        RemoveRunningDialog(_target);
        _target.UsingDialog = false;
        //for (int i = 0; i < _text.Length; i++)
        //{
        //    string dialogText = _text.Substring(0, i);
        //    string emptyDialog = string.Empty;
        //    int count = _text.Length - i - 1;
        //    while (count != 0)
        //    {
        //        count--;
        //        emptyDialog += "  ";
        //    }
        //    emptyDialog += '\r';
        //    _dialog.SetText(dialogText + emptyDialog);



        //    yield return new WaitForSeconds(0.1f);
        //}

        //_dialog.SetText(_text);

        //yield return new WaitForSeconds(1f);
    }

    private void Start()
    {
        UIManager.uimanager.uicontrol_On += OnUiControl;
        UIManager.uimanager.uicontrol_Off += OffUiControl;
    }

    void OnUiControl(Unit _unit)
    {
        switch (_unit)
        {
            case Monster:
                {
                    OnMonsterUi((Monster)_unit);
                    break;
                }                
            case Npc:
                {
                    OnNpcUi((Npc)_unit);
                    break;
                }                
            default:
                {
                    Debug.LogError("알수 없는 형태의 Unit형태입니다.");
                }
                break;
        }
    }
    void OffUiControl(Unit _unit)
    {
        switch (_unit)
        {
            case Monster:
                {
                    OffMonsterUi(_unit.gameObject);
                    break;
                }                
            case Npc:
                {
                    OffNpcUi(_unit.gameObject);
                    break;
                }                
            default:
                {
                    Debug.LogError("알수 없는 형태의 Unit형태입니다.");
                }
                break;
        }
    }
    public void MoveToNextScene()
    {
        StopAllCoroutines();
        List<GameObject> runningList_Hp = new List<GameObject>(runningHpBar.Keys);
        List<GameObject> runningList_Nick = new List<GameObject>(runningNickName.Keys);
        List<Unit> runningList_Dialog = new List<Unit>(runningMiniDialog.Keys);
        foreach(GameObject hp in runningList_Hp)
        {
            RemoveRunningHpBar(hp);
        }
        foreach (GameObject nic in runningList_Nick)
        {
            RemoveRunningNickName(nic);
        }
        foreach (Unit dg in runningList_Dialog)
        {
            RemoveRunningDialog(dg);
        }
    }
    void AddRunningCO(GameObject _parent,Coroutine _co)
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
    void RemoveRunningCo(GameObject _parent)
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
    void AddRunningNickName(GameObject _parent,TextMeshProUGUI _nickName)
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
    void AddRunningHpBar(GameObject _parent,Image _hpbar)
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

    void RemoveRunningNickName(GameObject _parent)
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
    void RemoveRunningDialog(Unit _parent)
    {
        if (runningMiniDialog.ContainsKey(_parent))
        {
            MiniDialogue popDialog = runningMiniDialog[_parent];
            miniDailog.Add(popDialog);
            runningMiniDialog.Remove(_parent);
        }
        else
        {
            Debug.Log("없는 닉네임을 풀로 되돌리려합니다.");
        }
    }
    void RemoveRunningHpBar(GameObject _parent)
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
    void OnMonsterUi(Monster _mob)
    {        
        TextMeshProUGUI nickName  = textPool.GetData("NickName");
        Image           hpBar     = imagePool.GetData("HpBar");
        

        AddRunningHpBar(_mob.gameObject, hpBar);
        AddRunningNickName(_mob.gameObject, nickName);
        

        nickName.text = _mob.NickName;        
        
        hpBar.transform.SetParent(nickName.transform);
        hpBar.rectTransform.anchoredPosition = new Vector2(0, -120f);

        Coroutine activeCoroutine = StartCoroutine(CoMonsterNicName(_mob, nickName, hpBar));
        AddRunningCO(_mob.gameObject, activeCoroutine);

    }
    void OffMonsterUi(GameObject _parent)
    {
        RemoveRunningCo(_parent);
        RemoveRunningHpBar(_parent);
        RemoveRunningNickName(_parent);
    }
    IEnumerator CoMonsterNicName(Monster _mob,TextMeshProUGUI _nickName,Image _hpbar)
    {
        while (true)
        {
            if (_mob.MightyEnermy())
            {
                _nickName.color = Color.red;
            }
            else
            {
                _nickName.color = Color.white;
            }           
            _nickName.rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(_mob.transform.position + new Vector3(0f, _mob.Nick_YPos, 0f));


            if (_mob.Hp_Curent <= 0)
            {
                _hpbar.gameObject.SetActive(false);
            }
            else
            {
                _hpbar.fillAmount = _mob.Hp_Curent / _mob.Hp_Max;
            }
            

            yield return null;
        }
    }
    void OnNpcUi(Npc _npc)
    {
        TextMeshProUGUI nickName    = textPool.GetData("NickName");

        AddRunningNickName(_npc.gameObject, nickName);

        Coroutine activeCoroutine   = StartCoroutine(CoNpcNicName(_npc,nickName));

        AddRunningCO(_npc.gameObject, activeCoroutine);

        nickName.text = _npc.NickName;
        nickName.color = Color.green;
    }
    void OffNpcUi(GameObject _parent)
    {
        RemoveRunningCo(_parent);
        RemoveRunningNickName(_parent);
    }
    IEnumerator CoNpcNicName(Npc _npc,TextMeshProUGUI _nickName)
    {
        
        while (true)
        {            
            _nickName.rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(_npc.transform.position + new Vector3(0f, _npc.Nick_YPos, 0f));
            yield return null;
        }
    }
    
  
    
    #region LevelUp
    public void LevelUpEffect(GameObject _Target)
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

    public void LoadDamageEffect(float _Damage, GameObject _Target, DAMAGE _DamageState = DAMAGE.NOMAL)
    {
        TextMeshProUGUI DmgEffect = textPool.GetData("DamageEffect");
        DmgEffect.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        DmgEffect.fontSize = 100;
        DmgEffect.text = _Damage.ToString();

        if (_DamageState == DAMAGE.CRITICAL)
        {
            DmgEffect.color = Color.red;
            DmgEffect.fontSize = 200;
        }

        StartCoroutine(DamageEffecting(DmgEffect, _Target.transform.position));
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
