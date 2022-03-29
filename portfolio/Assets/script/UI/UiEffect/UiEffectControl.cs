using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UiEffectControl : MonoBehaviour
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
    

    private void Awake()
    {
        
        textPool = new PoolingManager<TextMeshProUGUI>(this.gameObject);
        imagePool = new PoolingManager<Image>(this.gameObject);

        textPool.Add("DamageEffect", sampleDamageEffect);
        textPool.Add("NickName", sampleNicName);
        imagePool.Add("HpBar", sampleHpBar);
        textPool.Add("LevelUp", sampleLevelup);
    }
    #region NickName
    public void UnitUion(Unit _Target)                         // 수정할 것
    {
        //StartCoroutine(IShowNickName(_Target));            
    }
    void UnitUion(BattleUnit _Target)
    {
        StartCoroutine(IShowNickName(_Target));
        StartCoroutine(IShowHpbar(_Target));     
    }
    IEnumerator IShowNickName(Unit _Target)
    {
        TextMeshProUGUI NickName = SetNickName(_Target);
        while(_Target.DISTANCE >= 2f)
        {
            NickName.transform.position = NickName.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f,2f, 0f));
            yield return null;
        }
        TextReset(NickName);
        textPool.Add("NickName", NickName);
        _Target.USINGUI = false;
    }
    TextMeshProUGUI SetNickName(Unit _Target)
    {
        TextMeshProUGUI NickName = textPool.GetData("NickName");
        NickName.gameObject.SetActive(true);
        NickName.text = _Target.NICKNAME;
        return NickName;
    }

    #endregion
    #region HpBar
    void ShowHpBar(Unit _Target)
    {
        StartCoroutine(IShowNickName(_Target));
    }
    IEnumerator IShowHpbar(BattleUnit _Target)
    {
        if (_Target.HP_CURENT <= 0)
            yield break;
        Image hpBar = imagePool.GetData("HpBar");
        while (_Target.DISTANCE >= 2f || _Target.HP_CURENT > 0 )
        {
            hpBar.transform.position = hpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, 2f, 0f));

            yield return null;
        }
        hpBar.gameObject.SetActive(false);
        imagePool.Add("HpBar", hpBar);
        yield break;
    }
    #endregion
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
