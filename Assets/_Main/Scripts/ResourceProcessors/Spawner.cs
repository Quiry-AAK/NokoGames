using Scripts.ObjectPools;
using Scripts.StackGrid;
using UnityEngine;

namespace Scripts.ResourceProcessors
{
    [RequireComponent(typeof(ResourceProcessor))]
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private SpawnerVisual spawnerVisual;
        [SerializeField] private ItemStackArea itemStackArea;
        [SerializeField] private GameObject spawnedObjectPrefab;

        private ResourceProcessor resourceProcessor;

        private void Awake()
        {
            resourceProcessor = GetComponent<ResourceProcessor>();
        }

        private void Start()
        {
            resourceProcessor.StartProcessing();
        }

        private void OnEnable()
        {
            resourceProcessor.OnProcessEndEvent += HandleProcessEnd;
            itemStackArea.OnGetAnItemEvent += (_) => CheckStartProcessing();
        }

        private void OnDisable()
        {
            resourceProcessor.OnProcessEndEvent -= HandleProcessEnd;
            itemStackArea.OnGetAnItemEvent -= (_) => CheckStartProcessing();
        }

        private void HandleProcessEnd()
        {
            SpawnItem();
            if (itemStackArea.IsCapacityMaxReached())
            {
                resourceProcessor.StopProcessing();
            }

            else
            {
                resourceProcessor.StartProcessing();
            }
        }

        private void CheckStartProcessing()
        {
            if (!resourceProcessor.IsProcessorEnabled && !itemStackArea.IsCapacityMaxReached())
            {
                resourceProcessor.StartProcessing();
            }
        }

        private void SpawnItem()
        {
            var poolType = spawnedObjectPrefab.GetComponent<IPoolTypeHolder>().MyType;
            var stackAreaPlaceableItem = PoolsManager.Instance.Spawn(poolType, transform.TransformPoint(spawnerVisual.EndPos),
            spawnedObjectPrefab.transform.rotation).GetComponent<StackedItem>();

            stackAreaPlaceableItem.StartPlacing(itemStackArea.GetPositionByPlacing(stackAreaPlaceableItem));
        }

    }
}