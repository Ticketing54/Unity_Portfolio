using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolData<Type> where Type : Component
{ 
    protected Type Prefab;
    protected Queue<Type> Pool;
    protected int count = 0;
    GameObject Parent;
    string Key;
    public PoolData(Type _Prefab, GameObject _Parent,string _Key)
    {
        Pool = new Queue<Type>();
        Prefab = _Prefab;

        GameObject NewParent = new GameObject(Key + " : " + count);
        NewParent.transform.SetParent(_Parent.transform);
        
        _Prefab.gameObject.transform.SetParent(NewParent.transform);
        _Prefab.gameObject.SetActive(false);
        count = 0;
        Key = _Key;
        
        

        Parent = NewParent;
    }

    public void Add(Type _PoolingObject)
    {
        _PoolingObject.transform.SetParent(Parent.transform);
        _PoolingObject.gameObject.SetActive(false);
        Pool.Enqueue(_PoolingObject);
        count++;
        SetParentName();
    }
    
    public Type GetData()
    {
        Type NewType;
        if (count <= 0)
        {          
            NewType = Object.Instantiate<Type>(Prefab);            
        }
        else
        {
            NewType = Pool.Dequeue();
            count--;
        }
        NewType.transform.SetParent(Parent.transform);
        NewType.gameObject.SetActive(true);        
        SetParentName();
        return NewType;
    }

    public int Count { get { return count; } }
    void SetParentName()
    {
        Parent.name = Key + " : " + count;
    }
}
