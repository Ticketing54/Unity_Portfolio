using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager effectManager;
    PoolingManager_PrimitiveObject<GameObject> EffectRes;
    Dictionary<string, GameObject> effectParent;

    

    private void Awake()
    {
        if(effectManager == null)
        {
            effectManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        EffectRes = new PoolingManager_PrimitiveObject<GameObject>();
        effectParent = new Dictionary<string, GameObject>();
    }


    public GameObject LoadResourceEffect(string _EffectName)
    {
        return null;
    }
    public GameObject GetEffect(string _EffectName)
    {
        GameObject effectObj = EffectRes.GetData(_EffectName);
        if (effectObj == null)
        {
            GameObject NewEffect = LoadResourceEffect(_EffectName);        // 리소스 수정후 수정할 것
            AddEfect(NewEffect);
            return GetEffect(_EffectName);
        }
        else
        {
            effectObj.gameObject.SetActive(true);
            return effectObj;
        }

    }
    public void AddEfect(GameObject _Effect)
    {
        GameObject Parent;
        string EffectName = _Effect.name;
        if(!effectParent.TryGetValue(EffectName,out Parent))            /// 오류 확인할 것!
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


    public void LoadEffect(string _EffectName, Vector3 _Pos,float _Holdingtime)
    {
        GameObject Effect = GetEffect(_EffectName);
        StartCoroutine(TurnoffEffect(Effect, _Holdingtime));
    }

    IEnumerator TurnoffEffect(GameObject _Effect,float _Holdingtime)
    {
        yield return new WaitForSeconds(_Holdingtime);
        AddEfect(_Effect);
    }
   

}
