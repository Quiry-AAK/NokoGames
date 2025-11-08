using Scripts.ObjectPools;
using UnityEngine;

namespace Scripts.Tail
{
    [RequireComponent(typeof(TailManager))]
    public class BackManager : MonoBehaviour
    {
        private TailManager tailManager;
        private int currentItemCount;

        private void Awake()
        {
            currentItemCount = 0;
            tailManager = GetComponent<TailManager>();
        }

        public void AddItemToTail(TailItem tailItem)
        {
            for (int i = 0; i < tailManager.GetCapacity(); i++)
            {
                if (tailManager.GetTailNodeTransform(i).childCount == 0)
                {
                    var nodeTr = tailManager.GetTailNodeTransform(currentItemCount);
                    tailItem.StartPlacing(nodeTr);
                    currentItemCount++;
                    return;
                }
            }
        }

        public GameObject GetItemFromTail()
        {
            for (int i = tailManager.GetCapacity() - 1; i >= 0; i--)
            {
                if (tailManager.GetTailNodeTransform(i).childCount >= 1)
                {
                    currentItemCount--;
                    return tailManager.GetTailNodeTransform(i).GetChild(0).gameObject;
                }
            }

            return null;
        }

        public GameObject GetSpecificItemFromTail(PoolType poolType)
        {
            for (int i = tailManager.GetCapacity() - 1; i >= 0; i--)
            {
                if (tailManager.GetTailNodeTransform(i).childCount >= 1 && tailManager.GetTailNodeTransform(i).GetChild(0).
                TryGetComponent(out IPoolTypeHolder poolTypeHolder) && poolTypeHolder.MyType == poolType)
                {
                    currentItemCount--;
                    ReplaceTailItems(i);
                    return tailManager.GetTailNodeTransform(i).GetChild(0).gameObject;
                }
            }

            /* 
            Buna özel capacity biraz gereksiz ağır yüklü olur çünkü bu method biraz ağır 
            bir method o yüzden bunu dönen objenin null olup olmaması üzerinden kontrol edicez.
            */
            return null;
        }

        private void ReplaceTailItems(int startFromWhere)
        {
            for (int i = startFromWhere; i < tailManager.GetCapacity() - 1; i++)
            {
                var currentNode = tailManager.GetTailNodeTransform(i);
                var nextNode = tailManager.GetTailNodeTransform(i + 1);

                if (nextNode.childCount >= 1 &&
                    nextNode.GetChild(0).TryGetComponent(out TailItem tailItem))
                {
                    tailItem.StartPlacing(currentNode);
                }
            }
        }

        public bool IsCapacityMaxReached()
        {
            return currentItemCount >= tailManager.GetCapacity();
        }

        public bool IsThereAnyItem()
        {
            return currentItemCount > 0;
        }
    }
}