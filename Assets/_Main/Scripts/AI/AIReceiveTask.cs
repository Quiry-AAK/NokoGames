using Scripts.ObjectPools;
using UnityEngine;

namespace Scripts.AI
{
    public class AIReceiveTask : AITask
    {
        private IAIReceiveTaskCauser aIReceiveTaskCauser;

        protected override void Awake()
        {
            base.Awake();
            aIReceiveTaskCauser = GetComponent<IAIReceiveTaskCauser>();
        }

        public PoolType GetMyReceiveItemType()
        {
            return aIReceiveTaskCauser.ReceiveItemType;
        }

        public override bool ShouldStart()
        {
            return assignedAI.BackManager.IsThereAnyItem() && !aiTaskCauser.ItemStackArea.IsCapacityMaxReached();
        }

        public override bool ShouldFinish()
        {
            return !assignedAI.BackManager.IsThereAnyItem() || aiTaskCauser.ItemStackArea.IsCapacityMaxReached();
        }
    }
}