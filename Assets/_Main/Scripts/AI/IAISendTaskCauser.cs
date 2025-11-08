using Scripts.ObjectPools;

namespace Scripts.AI
{
    public interface IAISendTaskCauser
    {
        public PoolType SendItemType { get; }
    }
}