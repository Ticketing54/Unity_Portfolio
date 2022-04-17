using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UiEffectManager : MonoBehaviour
{
    
    PoolingManager<TextMeshProUGUI> textPool;           // UI Effect 텍스트 풀링
    PoolingManager<Image> imagePool;                    // UI Effect 이미지 풀링 // 이모티콘 때 사용    

    // Samples    
    [SerializeField]
    TextMeshProUGUI sampleDamageEffect;                 // 데미지 이펙트   
    [SerializeField]
    TextMeshProUGUI sampleNicName;                      // 닉네임
    [SerializeField]
    TextMeshProUGUI sampleLevelup;                      // 레벨업 텍스트
    [SerializeField]
    Image sampleHpBar;                                  // Hpbar

    LinkedList<TextMeshProUGUI>   activeNicNameList   = new LinkedList<TextMeshProUGUI>();
    LinkedList<Image>             activeHpBarList     = new LinkedList<Image>();
    LinkedList<Coroutine>         activeCoList        = new LinkedList<Coroutine>();
    private void Awake()
    {
        
        textPool = new PoolingManager<TextMeshProUGUI>(this.gameObject);
        imagePool = new PoolingManager<Image>(this.gameObject);

        textPool.Add("DamageEffect", sampleDamageEffect);
        textPool.Add("NickName", sampleNicName);
        imagePool.Add("HpBar", sampleHpBar);
        textPool.Add("LevelUp", sampleLevelup);        
    }    

    public void ActiveMonsterUi(Monster _mob)
    {        
        TextMeshProUGUI nickName = textPool.GetData("NickName");
        nickName.text = _mob.NickName;
        Image hpBar = imagePool.GetData("HpBar");
        Coroutine activeCoroutine = StartCoroutine(CoMonsterNicName(_mob, nickName, hpBar));

        hpBar.transform.SetParent(nickName.transform);
        hpBar.rectTransform.anchoredPosition = new Vector2(0, -45f);

        activeNicNameList.AddLast(nickName);
        activeHpBarList.AddLast(hpBar);
        activeCoList.AddLast(activeCoroutine);
    }

    IEnumerator CoMonsterNicName(Monster _mob,TextMeshProUGUI _nickName,Image _hpbar)
    {
        while (true)
        {
            if (Character.Player != null)
            {
                if (_mob.DISTANCE < 4f || _mob.IsTarget == true)
                {
                    _nickName.gameObject.SetActive(true);
                    _nickName.color = Color.white;
                    _nickName.rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(_mob.transform.position + new Vector3(0f, _mob.Nick_YPos, 0f));
                }
                else
                {
                    _nickName.gameObject.SetActive(false);
                }
            }
            yield return null;
        }
    }
    public void ActiveNpcUi(Npc _npc)
    {
        TextMeshProUGUI nickName    = textPool.GetData("NickName");        
        Coroutine activeCoroutine   = StartCoroutine(CoNpcNicName(_npc,nickName));       

        activeNicNameList.AddLast(nickName);        
        activeCoList.AddLast(activeCoroutine);
    }
    IEnumerator CoNpcNicName(Npc _npc,TextMeshProUGUI _nickName)
    {
        yield return null;
        while (true)
        {
            if (Character.Player != null)
            {
                if (_npc.DISTANCE < 4f || _npc.IsTarget == true)
                {
                    _nickName.gameObject.SetActive(true);
                    _nickName.color = Color.white;
                    _nickName.text = _npc.NickName;
                    _nickName.rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(_npc.transform.position + new Vector3(0f, _npc.Nick_YPos, 0f));
                }
                else
                {
                    _nickName.gameObject.SetActive(false);
                    
                }
            }
            yield return null;
        }
    }
    
  
    
    #region LevelUp
    public void LevelUpEffect(GameObject _Target)
    {
        TextMeshProUGUI LevelupText = textPool.GetData("LevelUp");
        StartCoroutine(LevelupMotion(LevelupText, _Target));
    }
    IEnumerator LevelupMotion(TextMeshProUGUI _LevelUpText, GameObject _Target)
    {
        float timer = 0f;
        float Moveto_Y = 0;
        float Preset_Y = 0;
        while (timer <= 2f)
        {
            Vector3 TargetPos = _Target.transform.position;
            Moveto_Y += Time.deltaTime;

            Preset_Y = _Target.transform.position.y + 1f;
            _LevelUpText.transform.position = Camera.main.WorldToScreenPoint(new Vector3(TargetPos.x, Preset_Y += Moveto_Y, TargetPos.z));
            _LevelUpText.fontSize += Time.deltaTime * 20f;
            timer += Time.deltaTime;
            yield return null;
        }

        _LevelUpText.fontSize = 50;

        textPool.Add("LevelUp", _LevelUpText);

        yield break;
    }

    #endregion
    #region DamageEffect

    void LoadDamageEffect(float _Damage, GameObject _Target, DAMAGE _DamageState = DAMAGE.NOMAL)
    {
        TextMeshProUGUI DmgEffect = textPool.GetData("DamageEffect");
        DmgEffect.gameObject.SetActive(true);
        DmgEffect.text = _Damage.ToString();

        if (_DamageState == DAMAGE.CRITICAL)
        {
            DmgEffect.color = Color.red;
            DmgEffect.fontSize = 100;
        }

        StartCoroutine(DamageEffecting(DmgEffect, _Target));
    }

    IEnumerator DamageEffecting(TextMeshProUGUI _Text, GameObject _Target)
    {
        float timer = 0f;
        Vector3 TargetPos = _Target.transform.position;

        while (timer <= 3f)
        {
            _Text.transform.position = Camera.main.WorldToScreenPoint(new Vector3(TargetPos.x, TargetPos.y += Time.deltaTime, TargetPos.z));
            yield return null;
        }

        TextReset(_Text);

        textPool.Add("DamageEffect", _Text);

        yield break;
    }
    #endregion

    void TextReset(TextMeshProUGUI _Effect)
    {
        _Effect.fontSize = 50;
        _Effect.color = Color.white;        
    }
    
}
