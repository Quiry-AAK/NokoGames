using UnityEngine;
using Scripts.Tail;
using Scripts.ObjectPools;

namespace Scripts.AI
{
    public class AISendTask : AITask
    {
        private IAISendTaskCauser aiSendTaskCauser;

        protected override void Awake()
        {
            base.Awake();
            aiSendTaskCauser = GetComponent<IAISendTaskCauser>();
        }

        public PoolType GetMySendItemType()
        {
            return aiSendTaskCauser.SendItemType;
        }

        public override bool ShouldStart()
        {
            return aiTaskCauser.ItemStackArea.IsThereAnyItem();
        }

        public override bool ShouldFinish()
        {
            return !aiTaskCauser.ItemStackArea.IsThereAnyItem() || assignedAI.BackManager.IsCapacityMaxReached();
        }
    }
}