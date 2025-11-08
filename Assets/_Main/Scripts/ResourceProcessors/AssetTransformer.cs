using DG.Tweening;
using Scripts.ObjectPools;
using Scripts.StackGrid;
using UnityEngine;

namespace Scripts.ResourceProcessors
{
    public class AssetTransformer : MonoBehaviour
    {
        [SerializeField] private AssetTransformerVisual assetTransformerVisual;
        [SerializeField] private ItemStackArea storageItemStackArea, receiveItemStackArea;
        [SerializeField] private GameObject spawnedObjectPrefab;
        private ResourceProcessor resourceProcessor;

        private bool somethingPlacing;

        private void Awake()
        {
            resourceProcessor = GetComponent<ResourceProcessor>();
        }

        private void OnEnable()
        {
            storageItemStackArea.OnGetAnItemEvent += (_) => CheckStartProcessing();
            receiveItemStackArea.OnPlaceEvent += (_, _) => CheckStartProcessing();
            resourceProcessor.OnProcessEndEvent += HandleProcessEnd;
        }

        private void OnDisable()
        {
            resourceProcessor.OnProcessEndEvent -= HandleProcessEnd;
            storageItemStackArea.OnGetAnItemEvent -= (_) => CheckStartProcessing();
            receiveItemStackArea.OnPlaceEvent -= (_, _) => CheckStartProcessing();
        }


        private void HandleProcessEnd()
        {
            SpawnItem();
            somethingPlacing = false;
            CheckStartProcessing();
        }

        private void SpawnItem()
        {
            var poolType = spawnedObjectPrefab.GetComponent<IPoolTypeHolder>().MyType;
            var stackAreaPlaceableItem = PoolsManager.Instance.Spawn(poolType, transform.TransformPoint(assetTransformerVisual.EndPos),
            spawnedObjectPrefab.transform.rotation).GetComponent<StackedItem>();

            stackAreaPlaceableItem.StartPlacing(storageItemStackArea.GetPositionByPlacing(stackAreaPlaceableItem));
        }

        private void CheckStartProcessing()
        {
            if (!storageItemStackArea.IsCapacityMaxReached() && receiveItemStackArea.IsThereAnyItem() && !somethingPlacing)
            {
                somethingPlacing = true;
                var temp = receiveItemStackArea.GetAnItem();
                temp.StartPlacing(assetTransformerVisual.InitialPos, () =>
                {
                    resourceProcessor.StartProcessing();
                    PoolsManager.Instance.Despawn(temp.MyType, temp.gameObject);
                }, assetTransformerVisual.SpawnedObjectVisual.rotation);
            }
        }
    }
}