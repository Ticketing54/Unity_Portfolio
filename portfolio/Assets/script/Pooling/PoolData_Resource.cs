using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolData_Resource
{
    protected GameObject Prefab;
    protected Queue<GameObject> Pool;
    protected int count = 0;
    GameObject Parent;
    string Key;
    public PoolData_Resource(string _Key, GameObject _Parent,GameObject _Prefab)
    {
        Pool = new Queue<GameObject>();
        Prefab = _Prefab;
        Key = _Key;
        count = 0;        
        GameObject NewParent = new GameObject(Key + " : " + count);
        NewParent.transform.SetParent(_Parent.transform);

        Parent = NewParent;
    }

    public void Add(GameObject _PoolingObject)
    {
        _PoolingObject.transform.SetParent(Parent.transform);
        _PoolingObject.gameObject.SetActive(false);
        Pool.Enqueue(_PoolingObject);
        count++;
        SetParentName();
    }

    public GameObject GetData()
    {
        GameObject NewType;
        if (count <= 0)
        {
            NewType = GameObject.Instantiate(Prefab);
            NewType.transform.SetParent(Parent.transform);

            return NewType;
        }
        NewType = Pool.Dequeue();
        NewType.gameObject.SetActive(true);
        count--;
        SetParentName();
        return NewType;
    }

    public int Count { get { return count; } }
    void SetParentName()
    {
        Parent.name = Key + " : " + count;
    }
}
