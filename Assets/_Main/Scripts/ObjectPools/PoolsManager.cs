using Scripts.Utils;
using UnityEngine.Pool;
using UnityEngine;
using System.Collections.Generic;

namespace Scripts.ObjectPools
{
    public class PoolsManager : MonoSingleton<PoolsManager>
    {
        [SerializeField] private List<PoolEntry> poolsSetup;
        private Dictionary<PoolType, ObjectPool<GameObject>> pools;


        protected override void Awake()
        {
            base.Awake();
            pools = new Dictionary<PoolType, ObjectPool<GameObject>>();

            foreach (var entry in poolsSetup)
            {
                pools[entry.poolType] = new ObjectPool<GameObject>(
                    () => Instantiate(entry.prefab),
                    obj => obj.SetActive(true),
                    obj => obj.SetActive(false),
                    obj => Destroy(obj),
                    false,
                    entry.defaultCapacity
                );
            }
        }

        public GameObject Spawn(PoolType poolType, Vector3 pos, Quaternion rot)
        {
            if (pools.ContainsKey(poolType))
            {
                var obj = pools[poolType].Get();
                obj.transform.SetPositionAndRotation(pos, rot);
                return obj;
            }
            else
            {
                Debug.LogError($"Pool for {poolType} does not exist!");
                return null;
            }
        }

        public void Despawn(PoolType poolType, GameObject obj)
        {
            pools[poolType].Release(obj);
        }
    }

    [System.Serializable]
    public class PoolEntry
    {
        public PoolType poolType;
        public GameObject prefab;
        public int defaultCapacity = 10;
    }
}