using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsPool : MonoBehaviour
{

    public static ObjectsPool Instance; 

    [System.Serializable]
    public class Pool
    {
        string tag;
        int count;
        GameObject prefab;
    }

    public List<Pool> objectPools;
    public Dictionary<string, Queue<GameObject>> tagPools; 


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        tagPools = new Dictionary<string, Queue<GameObject>>();
    }
}
