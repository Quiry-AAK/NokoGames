using Scripts.AI;
using Scripts.ObjectPools;
using Scripts.StackGrid;
using UnityEngine;

namespace Scripts.Tail
{
    public class TailItemReceiver : TailItemTransferHandler, IAIReceiveTaskCauser
    {
        [Space]
        [SerializeField] private PoolType receiveItemType;

        public PoolType ReceiveItemType => receiveItemType;

        protected override void HandleTransfer(BackManager backManager)
        {
            if (!itemStackArea.IsCapacityMaxReached() && backManager.IsThereAnyItem())
            {
                var item = backManager.GetSpecificItemFromTail(receiveItemType);
                if (item != null && item.TryGetComponent(out StackedItem stackedItem))
                {
                    stackedItem.StartPlacing(itemStackArea.GetPositionByPlacing(stackedItem));
                }
            }
        }
    }
}