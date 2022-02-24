using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager effectManager;
    PoolingManager_Resource EffectRes;
    Dictionary<string, GameObject> effectParent;
    List<GameObject> ClickEffect;

    

    private void Awake()
    {
        if (effectManager == null)
        {
            effectManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        EffectRes = new PoolingManager_Resource(this.gameObject);
        effectParent = new Dictionary<string, GameObject>();
        ClickEffect = new List<GameObject>();
        ClickEffectRes();
    }

    #region ClickEffect
    void ClickEffectRes()
    {
        ClickEffect.Add(GameManager.gameManager.resource.GetGameObject("ClickEffect"));
        ClickEffect.Add(GameManager.gameManager.resource.GetGameObject("ClickEffect"));
        ClickEffect.Add(GameManager.gameManager.resource.GetGameObject("ClickEffect"));
    }

    public void ClickEffectOn(CLICKEFFECT _ClickEffect, Transform _Target)                      // 대상이 있음
    {
        GameObject clickEffect = ClickEffectCheck(_ClickEffect);
        clickEffect.transform.SetParent(_Target);
        clickEffect.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        clickEffect.gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
    }
    public void ClickEffectOn(CLICKEFFECT _ClickEffect, Vector3 _Pos)                           // 바닥
    {
        GameObject clickEffect = ClickEffectCheck(_ClickEffect);
        clickEffect.gameObject.transform.position = _Pos + new Vector3(0f, 1f, 0f);
    }
    GameObject ClickEffectCheck(CLICKEFFECT _ClickEffect)                                       // 클릭 대상에 따라 하나만 켜져있게함
    {
        GameObject SeletClickEffect = ClickEffect[(int)_ClickEffect];

        foreach (GameObject one in ClickEffect)
        {
            if (one == SeletClickEffect)
            {
                if (one.gameObject.activeSelf == false)
                {
                    one.gameObject.SetActive(true);
                }
            }
            else
            {
                if (one.gameObject.activeSelf == true)
                {
                    one.gameObject.SetActive(true);
                }
                one.gameObject.SetActive(false);
            }
        }

        return SeletClickEffect;
    }
    #endregion

    GameObject LoadResourceEffect(string _EffectName)                       // 리소스에서 받아오기
    {
        return GameManager.gameManager.resource.GetGameObject(_EffectName);
    }
    GameObject GetEffect(string _EffectName)                                // Effect 이름별로 꺼내오기
    {
        GameObject effectObj = EffectRes.GetData(_EffectName);
        if (effectObj == null)
        {
            GameObject NewEffect = LoadResourceEffect(_EffectName);         // 리소스 수정후 수정할 것
            AddEfect(NewEffect);
            return GetEffect(_EffectName);
        }
        else
        {
            effectObj.gameObject.SetActive(true);
            return effectObj;
        }

    }
    void AddEfect(GameObject _Effect)                                       // Effect 사용 후 저장
    {
        GameObject Parent;
        string EffectName = _Effect.name;
        if (!effectParent.TryGetValue(EffectName, out Parent))                // 오류 확인할 것!
        {
            Parent = new GameObject(EffectName);
            Parent.transform.SetParent(this.transform);
            Parent.gameObject.transform.position = Vector3.zero;
            effectParent.Add(EffectName, new GameObject(EffectName));
        }

        _Effect.transform.SetParent(Parent.transform);
        _Effect.gameObject.SetActive(false);


        EffectRes.Add(_Effect.name, _Effect);
    }


    public void LoadEffect(string _EffectName, Vector3 _Pos, float _Holdingtime)     // 이펙트 사용
    {
        GameObject Effect = GetEffect(_EffectName);
        StartCoroutine(TurnoffEffect(Effect, _Holdingtime));
    }

    IEnumerator TurnoffEffect(GameObject _Effect, float _Holdingtime)
    {
        yield return new WaitForSeconds(_Holdingtime);
        AddEfect(_Effect);
    }


}
