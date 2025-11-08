using Scripts.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Buy
{
    public class BuyArea : MonoBehaviour
    {
        [SerializeField] private int cost;
        [SerializeField] private Vector2 maskSizeY;
        [SerializeField] private Transform maskTr;
        [SerializeField] private float triggerDelay = .1f, triggerRadius;
        [SerializeField] private LayerMask coinLayer;
        [SerializeField] private UnityEvent OnBuyEvent;

        [SerializeField] private Color gizmosColor;

        private float triggerDelayChecker;

        private int defaultCost;

        private void Awake()
        {
            triggerDelayChecker = 0f;
            defaultCost = cost;

        }

        private void Update()
        {
            if (Time.time >= triggerDelayChecker)
            {
                triggerDelayChecker = Time.time + triggerDelay;
                var hits = Physics.OverlapSphere(transform.position, triggerRadius, coinLayer);
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].TryGetComponent(out CoinManager coinManager))
                    {
                        if (coinManager.GetCurrentCoinAmount() > 0 && cost > 0)
                        {
                            coinManager.RemoveCoin(1);
                            cost--;

                            var ls = maskTr.transform.localScale;
                            ls.y = Mathf.Lerp(maskSizeY.x, maskSizeY.y, 1 - cost / (float)defaultCost);
                            maskTr.transform.localScale = ls;

                            if (cost <= 0)
                            {
                                OnBuyEvent?.Invoke();
                            }
                        }
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
    }
}