using DG.Tweening;
using Scripts.ObjectPools;
using Scripts.StackGrid;
using Scripts.Tail;
using UnityEngine;

namespace Scripts.Trash
{
    public class TrashCan : MonoBehaviour
    {
        [SerializeField] private float triggerDelay = .1f, triggerRadius;
        [SerializeField] private LayerMask tailLayer;
        [Header("Animation")]
        [SerializeField] private Transform icon;
        [SerializeField] private float animationDuration, animationEndScale;

        [Header("Gizmos")]
        [SerializeField] private Color gizmosColor;

        private Tween animationTween;

        private float triggerDelayChecker;

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

        private void HandleTransfer(BackManager backManager)
        {
            if (backManager.IsThereAnyItem())
            {
                var tmp = backManager.GetItemFromTail();
                if (tmp.TryGetComponent(out StackedItem stackedItem))
                {
                    stackedItem.StartPlacing(transform.position, () =>
                    {
                        animationTween?.Complete();
                        animationTween = icon.DOScale(animationEndScale, animationDuration).SetEase(Ease.InBack).SetLoops(2, LoopType.Yoyo);
                        PoolsManager.Instance.Despawn(stackedItem.MyType, stackedItem.gameObject);
                    });
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
    }
}