using Scripts.AI;
using Scripts.ObjectPools;
using Scripts.ResourceProcessors;
using Scripts.StackGrid;
using UnityEngine;

namespace Scripts.Tail
{
    public class TailItemSender : TailItemTransferHandler, IAISendTaskCauser
    {
        [SerializeField] private PoolType sendItemType;

        public PoolType SendItemType => sendItemType;

        protected override void HandleTransfer(BackManager backManager)
        {
            if (!backManager.IsCapacityMaxReached() && itemStackArea.IsThereAnyItem() &&
                                itemStackArea.GetAnItem().TryGetComponent(out TailItem item))
            {
                backManager.AddItemToTail(item);
            }
        }
    }
}