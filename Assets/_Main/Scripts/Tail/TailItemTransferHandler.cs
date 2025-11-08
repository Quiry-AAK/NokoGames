using Scripts.AI;
using Scripts.StackGrid;
using UnityEngine;

namespace Scripts.Tail
{
    public abstract class TailItemTransferHandler : MonoBehaviour, IAITaskCauser
    {
        [SerializeField] protected ItemStackArea itemStackArea;
        [SerializeField] protected float triggerDelay = .1f, triggerRadius;
        [SerializeField] protected LayerMask tailLayer;

        [Header("Gizmos")]
        [SerializeField] protected Color gizmosColor;

        protected float triggerDelayChecker;

        public ItemStackArea ItemStackArea => itemStackArea;

        private void Awake()
        {
            triggerDelayChecker = 0f;
        }

        private void Update()
        {
            if (Time.time >= triggerDelayChecker)
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

        protected abstract void HandleTransfer(BackManager backManager);
    }
}