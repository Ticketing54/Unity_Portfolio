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
    Image sampleHpBar;

    private void Awake()
    {        
        textPool = new PoolingManager<TextMeshProUGUI>();
        imagePool = new PoolingManager<Image>();

        textPool.Add("DamageEffect", sampleDamageEffect);
        textPool.Add("NickName", sampleNicName);
        imagePool.Add("HpBar", sampleHpBar);
    }
    #region NickName
    void UnitUion(UIControl _Target)
    {
        StartCoroutine(IShowNickName(_Target));            
    }
    void UnitUion(BattleUiControl _Target)
    {
        StartCoroutine(IShowNickName(_Target));
        StartCoroutine(IShowHpbar(_Target));     
    }
    IEnumerator IShowNickName(UIControl _Target)
    {
        TextMeshProUGUI NickName = SetNickName(_Target);
        while(_Target.Distance >= 2f)
        {
            NickName.transform.position = NickName.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f,2f, 0f));
            yield return null;
        }
        TextReset(NickName);
        textPool.Add("NickName", NickName);
        _Target.UsingUi = false;
    }
    TextMeshProUGUI SetNickName(UIControl _Target)
    {
        TextMeshProUGUI NickName = textPool.GetData("NickName");
        NickName.gameObject.SetActive(true);
        NickName.text = _Target.NickName;
        return NickName;
    }

    #endregion
    #region HpBar
    void ShowHpBar(UIControl _Target)
    {
        StartCoroutine(IShowNickName(_Target));
    }
    IEnumerator IShowHpbar(BattleUiControl _Target)
    {
        if (_Target.HP_Current <= 0)
            yield break;
        Image hpBar = imagePool.GetData("HpBar");
        while (_Target.Distance >= 2f || _Target.HP_Current > 0 )
        {
            hpBar.transform.position = hpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, 2f, 0f));

            yield return null;
        }
        hpBar.gameObject.SetActive(false);
        imagePool.Add("HpBar", hpBar);
        yield break;
    }   
    #endregion



    void LoadDamageEffect(float _Damage,GameObject _Target,DAMAGE _DamageState = DAMAGE.NOMAL)
    {
        TextMeshProUGUI DmgEffect = textPool.GetData("DamageEffect");
        DmgEffect.gameObject.SetActive(true);
        DmgEffect.text = _Damage.ToString();

        if (_DamageState == DAMAGE.CRITICAL)
        {
            DmgEffect.color = Color.red;
            DmgEffect.fontSize = 100;
        }

        StartCoroutine(DamageEffecting(DmgEffect,_Target));
    }

    IEnumerator DamageEffecting(TextMeshProUGUI _Text,GameObject _Target)
    {
        float timer = 0f;
        Vector3 TargetPos = _Target.transform.position;        

        while (timer <= 3f)
        {
            _Text.transform.position = Camera.main.WorldToScreenPoint(new Vector3(TargetPos.x, TargetPos.y += Time.deltaTime,TargetPos.z));
            yield return null;
        }

        TextReset(_Text);        

        textPool.Add("DamageEffect", _Text);

        yield break;
    }
    void TextReset(TextMeshProUGUI _Effect)
    {
        _Effect.fontSize = 50;
        _Effect.color = Color.white;
        _Effect.gameObject.SetActive(false);
    }
   
}
