using System;
using UnityEngine;

namespace Scripts.StackGrid
{
    [RequireComponent(typeof(GridArea))]
    public class ItemStackArea : MonoBehaviour
    {
        private GridArea gridArea;
        private ItemStackAreaProperties[] itemStackAreaProperties;
        private int currentItemsCount;

        public event Action<StackedItem> OnGetAnItemEvent;
        public event Action<Vector3, StackedItem> OnPlaceEvent;

        private void Awake()
        {
            gridArea = GetComponent<GridArea>();
            currentItemsCount = 0;
            var capacity = gridArea.GetTotalCapacity();
            itemStackAreaProperties = new ItemStackAreaProperties[capacity];
            for (int i = 0; i < capacity; i++)
            {
                itemStackAreaProperties[i] = new ItemStackAreaProperties(gridArea.GetSlotPosition(i));
            }
        }

        public Vector3 GetPositionByPlacing(StackedItem stackedItem)
        {
            for (int i = 0; i < itemStackAreaProperties.Length; i++)
            {
                if (itemStackAreaProperties[i].CheckAmINull())
                {
                    itemStackAreaProperties[i].SetMyStackedItem(stackedItem);
                    var tmpPos = itemStackAreaProperties[i].GetMyPos();
                    currentItemsCount++;
                    OnPlaceEvent?.Invoke(tmpPos, stackedItem);
                    return tmpPos;
                }
            }

            return Vector3.zero;
        }

        public StackedItem GetAnItem()
        {
            for (int i = itemStackAreaProperties.Length - 1; i >= 0; i--)
            {
                if (!itemStackAreaProperties[i].CheckAmINull())
                {
                    var tmp = itemStackAreaProperties[i].GetMyStackedItem(true);
                    currentItemsCount--;
                    OnGetAnItemEvent?.Invoke(tmp);
                    return tmp;
                }
            }

            return null;
        }

        public bool IsCapacityMaxReached()
        {
            return currentItemsCount >= gridArea.GetTotalCapacity();
        }

        public bool IsThereAnyItem()
        {
            return currentItemsCount > 0;
        }
    }

    public class ItemStackAreaProperties
    {
        private StackedItem stackedItem;
        private Vector3 areaPos;

        public ItemStackAreaProperties(Vector3 areaPos)
        {
            this.areaPos = areaPos;
        }

        public StackedItem GetMyStackedItem(bool remove)
        {
            var temp = stackedItem;
            if (remove)
                stackedItem = null;
            return temp;
        }

        public void SetMyStackedItem(StackedItem newStackedItem)
        {
            stackedItem = newStackedItem;
        }

        public Vector3 GetMyPos()
        {
            return areaPos;
        }

        public bool CheckAmINull()
        {
            return stackedItem == null;
        }
    }
}
