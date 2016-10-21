using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class PrefabPoolAgent : IPoolAgent
{
    public enum PrefabType
    {
        Non,
        Particle,
    }

    GameObject entity;

    public PrefabType type = PrefabType.Non;
    public PrefabPool pool;
    public UnityAction<PrefabPoolAgent> callback;
    public ParticleSystem particle;

    public bool IsExistEntity 
    {
        get
        {
            if (entity != null) return true;
            else return false;
        }
    }

    public GameObject GetEntity()
    {
        return entity;
    }

    public void Reset()
    {
        if (!entity) return;
        entity.SetActive(true);
        entity.transform.localScale = Vector3.one;
        entity.transform.localPosition = Vector3.zero;
        entity.transform.localRotation = Quaternion.identity;

        if (particle != null)
        {
            type = PrefabType.Particle;
            pool.StartCoroutine(pool.AutoReleaseParticle(entity.name, particle, this));
        }
    }

    public void OnInit(string tag, Component who)
    {
        Global.AssetLoadManager.LoadAsset<GameObject>("prefab/" + tag, tag, (prefab) =>
        {
            GameObject go = GameObject.Instantiate(prefab) as GameObject;
            if (go == null) return;
            entity = go;
            entity.name = tag;
            pool = who as PrefabPool;

            //判断类型， 后期类型多了后，会改条件
            particle = entity.GetComponentInChildren<ParticleSystem>();

            Reset();

            if (callback != null) callback(this);
        });
    }

    public void OnReset()
    {
        Reset();
    }

    public void OnRelease()
    {
        if (!entity) return;
        entity.SetActive(false);
        entity.transform.SetParent(pool.transform);
    }

    public void OnClear()
    {
        GameObject.Destroy(entity);
        Global.AssetLoadManager.UnloadAssetBundle("prefab/" + entity.name.ToLower() + Global.BundleExtName);
    }
}


public class PrefabPool : MonoBehaviour
{
    //Prefab与其的ObjectPool一一对应
    static Dictionary<string, ObjectPool<PrefabPoolAgent>> poolDict = new Dictionary<string, ObjectPool<PrefabPoolAgent>>();
    int DefaultPoolSize = 30;

    private PrefabPool() { }
    private static PrefabPool _instance;
    public static PrefabPool Instance
    {
        get
        {
            if (null == _instance)
            {
                string rName = "PrefabPool";
                GameObject pool = GameObject.Find(rName);
                if (pool == null)
                {
                    pool = new GameObject(rName);
                    pool.name = rName;
                }
                _instance = pool.GetComponentSafe<PrefabPool>();
                //DontDestroyOnLoad(pool);
            }
            return _instance;
        }
    }

    public void Init()
    {
        string rName = "PrefabPool";
        GameObject pool = GameObject.Find(rName);
        if (pool == null)
        {
            pool = new GameObject(rName);
            pool.name = rName;
        }
        pool.AddComponent<PrefabPool>();
    }

    public ObjectPool<PrefabPoolAgent> GetPool(string prefabName)
    {
        if (poolDict.ContainsKey(prefabName))
        {
            return poolDict[prefabName];
        }
        return null;
    }

    public void ClearPool(string prefabName)
    {
        if (poolDict.ContainsKey(prefabName))
        {
            poolDict[prefabName].Clear();
        }
    }

    public void ClearAllPool()
    {
        while(poolDict.Count > 0)
        {
            var e = poolDict.GetEnumerator();
            if (e.MoveNext())
            {
                e.Current.Value.Clear();
            }
        }

    }

    public void Spawn(string prefabName, UnityAction<PrefabPoolAgent> callback)
    {
        ObjectPool<PrefabPoolAgent> pool;
        if (!poolDict.ContainsKey(prefabName))
        {
            pool = new ObjectPool<PrefabPoolAgent>(prefabName, DefaultPoolSize);
            poolDict.Add(prefabName, pool);
        }
        else
        {
            pool = poolDict[prefabName];
        }

        if (pool == null)
        {
            DebugConsole.LogError("无法获取对象池:" + prefabName);
            return;
        }

        var agent = pool.Get(this);
        
        if (agent.IsExistEntity)
        {
            callback(agent);
        }
        else
        {
            agent.callback = callback;
        }
    }

    public void Despawn(string prefabName, PrefabPoolAgent agent)
    {
        if (poolDict.ContainsKey(prefabName))
        {
            poolDict[prefabName].Release(agent);
        }
    }

    public IEnumerator AutoReleaseParticle(string prefabName, ParticleSystem particle, PrefabPoolAgent agent)
    {
        yield return new WaitForSeconds(particle.startDelay + 0.25f);

        GameObject go = particle.gameObject;
        while (particle.IsAlive(true) && go.activeInHierarchy)
        {
            yield return null;
        }

        if (go.activeInHierarchy)
        {
            Despawn(prefabName, agent);
            particle.Clear(true);
        }
    }

}
