using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LYNet
{
    public class PoolManager : MonoBehaviour
    {

        [System.Serializable]

        public class PoolObject
        {
            public enum EndType
            {
                EndByScript,
                EndByAni,
                EndByParticle,
                EndBySound,
                EndByAI,
            }
            
            public GameObject prefab;
            public EndType endType = EndType.EndByScript;
            public int maxCount = 50;
        }

        public class PoolObjectInternal : PoolObject
        {
            public int curCount = 0;
            public List<PoolAgent> objList = new List<PoolAgent>();
        }

        public string poolName;
        public PoolObject.EndType endType = PoolObject.EndType.EndByScript;
        public int maxCount = 50;
        public string path;

        public List<PoolObject> prefabList;
        public Dictionary<string, PoolObjectInternal> poolDict = new Dictionary<string, PoolObjectInternal>();


        private static Dictionary<string, PoolManager> poolManagerDict = new Dictionary<string, PoolManager>();
        public static GameObject Swap(string pool, string name, params object[] args)
        {
            return poolManagerDict[pool].GetObject(name, args);
        }

        public GameObject GetObject(string name, params object[] args)
        {
            if (poolDict.ContainsKey(name))
            {
                var pool = poolDict[name];
                if (pool.objList.Count > 0)
                {
                    PoolAgent agent = pool.objList[0];
                    pool.objList.Remove(agent);
                    agent.gameObject.SetActive(true);
                    agent.SwapEnd(args);
                    return agent.gameObject;
                }
                else if (pool.curCount < pool.maxCount)
                {
                    GameObject prefab = GameObject.Instantiate(pool.prefab);
                    if (prefab.GetComponent<RectTransform>() == null)
                    {
                        prefab.transform.parent = transform;
                    }

                    pool.curCount++;
                    PoolAgent agent = prefab.GetComponent<PoolAgent>();
                    if (agent == null)
                    {
                        agent = prefab.AddComponent<PoolAgent>();
                    }

                    agent.Init(pool);
                    agent.SwapEnd(args);
                    return prefab;
                }
                else
                {
                    GameObject prefab = GameObject.Instantiate(pool.prefab);
                    PoolAgent agent = prefab.GetComponent<PoolAgent>();
                    if (agent == null)
                    {
                        agent = prefab.AddComponent<PoolAgent>();
                    }
                   
                       
                    Debug.Log(string.Format("beyond pool's max pool:{0} prefab:{1}", poolName, name));
                    agent.Init(null, pool.endType);
                    agent.SwapEnd(args);
                    return prefab;
                }
            }
            else
            {
                PoolObjectInternal poolobj = new PoolObjectInternal();
                poolobj.prefab = Resources.Load<GameObject>(path + name);
                poolobj.endType = endType;
                poolobj.maxCount = maxCount;
                poolDict.Add(name, poolobj);
                Debug.Log("LOAD " + name);
                return GetObject(name, args);
            }
        }

        void Awake()
        {
            foreach (PoolObject pool in prefabList)
            {
                PoolObjectInternal poolobj = new PoolObjectInternal();
                poolobj.prefab = pool.prefab;
                poolobj.endType = pool.endType;
                poolobj.maxCount = pool.maxCount;
                poolDict.Add(pool.prefab.name, poolobj);
            }

            if (path.Length > 0)
            {
                if (path.ToCharArray()[path.Length - 1] != '/')
                {
                    path = path + "/";
                }
            }

            poolManagerDict[poolName] = this;
        }


    }
}