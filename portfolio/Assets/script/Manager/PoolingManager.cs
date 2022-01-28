using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface Pooling
{
    void Reset();
    void Reposition();

}
public class PoolingManager<T> where T : MonoBehaviour, Pooling
{
    class PoolingData 
    {
        Queue<T> Pool;
        GameObject Prefab;
        int count;        
        public PoolingData(string _PrefabName)
        {            
            Pool = new Queue<T>();
            Prefab = GameManager.gameManager.GetResource(_PrefabName, _PrefabName);
            count = 1;
        }

        public void Add(T _data)
        {
            _data.Reset();
            _data.Reposition();
            Pool.Enqueue(_data);
            count++;
        }        
        public T GetData()
        {
            if(count <= 0)
            {
                GameObject obj = GameObject.Instantiate(Prefab);               

                return obj.AddComponent<T>();
            }

            return Pool.Dequeue();
        }

        
        public int Count { get { return count;} }

        
    }


    Dictionary<string, PoolingData> dic = new Dictionary<string, PoolingData>();
    
    
    public void Add(string _Key,T _t)
    {
        PoolingData data;
        if (dic.TryGetValue(_Key,out data))
        {
            data.Add(_t);            
        }
        else
        {
            data = new PoolingData(_Key);
            data.Add(_t);
            dic.Add(_Key,data);        
        }
        data = null;
    }

    public T GetData(string _Key)
    {
        PoolingData data;
        if (dic.TryGetValue(_Key, out data))
        {
            return data.GetData();
        }
        else
        {
            data = new PoolingData(_Key);
            dic.Add(_Key,data);
            return data.GetData();
        }
        
    }

   


       


}
