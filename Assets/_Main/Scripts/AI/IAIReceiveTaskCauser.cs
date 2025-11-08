using Scripts.ObjectPools;

namespace Scripts.AI
{
    public interface IAIReceiveTaskCauser
    {
        public PoolType ReceiveItemType { get; }
    }
}