using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface Pooling
{
    void Reset();
    void Reposition();

}
public class PoolingManager<T> where T : Pooling, new()
{
    class PoolingData 
    {        
        Queue<T> Pool;
        int count;        
        public PoolingData()
        {            
            Pool = new Queue<T>();            
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
                return new T();
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
            data = new PoolingData();
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
            data = new PoolingData();
            dic.Add(_Key,data);
            return data.GetData();
        }
        
    }

   


       


}
