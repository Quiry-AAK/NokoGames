using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Scripts.ObjectPools;
using Scripts.StackGrid;
using Scripts.Tail;
using UnityEngine;

namespace Scripts.Building
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private Transform blocksParent;
        [SerializeField] private float triggerDelay = .1f, triggerRadius;
        [SerializeField] private LayerMask tailLayer;

        [Header("Animation")]
        [SerializeField] private float animationDuration;
        [SerializeField] private float animationScaleEnd;
        [SerializeField] private GameObject psGameObject;
        [Header("Gizmos")]
        [SerializeField] private Color gizmosColor;

        private Dictionary<PoolType, List<BuildingBlock>> buildingBlocksDictionary;

        private float triggerDelayChecker;

        private bool buildingCompleted;


        private void Awake()
        {
            var list = new List<BuildingBlock>();
            for (int i = 0; i < blocksParent.childCount; i++)
            {
                list.Add(blocksParent.GetChild(i).GetComponent<BuildingBlock>());
            }

            buildingBlocksDictionary = new Dictionary<PoolType, List<BuildingBlock>>();

            var totalTypes = list.Select(x => x.ItemType).Distinct().ToList();

            for (int i = 0; i < totalTypes.Count; i++)
            {
                buildingBlocksDictionary.Add(totalTypes[i], list.Where(x => x.ItemType == totalTypes[i]).ToList());
            }

            triggerDelayChecker = 0f;
        }

        private void Update()
        {
            if (Time.time >= triggerDelayChecker && !buildingCompleted)
            {
                triggerDelayChecker = Time.time + triggerDelay;
                var hits = Physics.OverlapSphere(transform.position, triggerRadius, tailLayer);
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].TryGetComponent(out BackManager backManager))
                    {
                        HandleTransfer(backManager);
                    }
                }
            }
        }

        private void HandleTransfer(BackManager backManager)
        {
            if (backManager.IsThereAnyItem())
            {
                var keysList = buildingBlocksDictionary.Keys.ToList();
                for (int i = 0; i < keysList.Count; i++)
                {
                    var item = backManager.GetSpecificItemFromTail(keysList[i]);
                    if (item != null && item.TryGetComponent(out StackedItem stackedItem))
                    {
                        var neededItemsList = buildingBlocksDictionary[keysList[i]];
                        if (neededItemsList.Count > 0)
                        {
                            var firstItem = neededItemsList[0];
                            neededItemsList.RemoveAt(0);
                            stackedItem.StartPlacing(firstItem.transform.position, () =>
                            {
                                firstItem.EnableMe(true);
                                PoolsManager.Instance.Despawn(stackedItem.MyType, stackedItem.gameObject);
                            }, firstItem.transform.rotation);

                            if (neededItemsList.Count == 0)
                            {
                                buildingBlocksDictionary.Remove(keysList[i]);
                            }

                            if (buildingBlocksDictionary.Count == 0)
                            {
                                CompleteBuilding();
                            }
                        }
                    }
                }
            }
        }

        private void CompleteBuilding()
        {
            buildingCompleted = true;
            transform.DOScale(animationScaleEnd, animationDuration).SetEase(Ease.InBack).SetLoops(2, LoopType.Yoyo);
            psGameObject.SetActive(true);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (triggerRadius > 0f)
            {
                Gizmos.color = gizmosColor;
                Gizmos.DrawWireSphere(transform.position, triggerRadius);
            }
        }
#endif
    }
}