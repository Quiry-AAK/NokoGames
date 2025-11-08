using System;
using DG.Tweening;
using Scripts.ObjectPools;
using UnityEngine;

namespace Scripts.StackGrid
{
    public class StackedItem : MonoBehaviour, IPoolTypeHolder
    {
        [SerializeField] private Quaternion targetRot;
        [SerializeField] private PoolType myType;
        [SerializeField] private int rotationCount;
        [SerializeField] private float jumpPower, jumpDuration;
        public PoolType MyType => myType;

        public void StartPlacing(Vector3 pos, Action OnCompletedEvent = null, Quaternion? rot = null)
        {
            transform.SetParent(null);

            transform.DOJump(pos, jumpPower, 1, jumpDuration).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                OnCompletedEvent?.Invoke();
            });

            var startRotation = transform.eulerAngles;

            var totalRotation = 360f * rotationCount;

            var rotationDelta = rot.HasValue ? rot.Value.eulerAngles - startRotation : targetRot.eulerAngles - startRotation;

            if (rotationDelta.y < 0)
            {
                rotationDelta.y += 360f;
            }

            if (rot.HasValue)
            {
                transform.DORotate(new Vector3(rot.Value.eulerAngles.x, startRotation.y + totalRotation + rotationDelta.y, rot.Value.eulerAngles.z),
                            jumpDuration, RotateMode.FastBeyond360)
                                .SetEase(Ease.Linear);
            }
            else
            {
                transform.DORotate(new Vector3(targetRot.eulerAngles.x, startRotation.y + totalRotation + rotationDelta.y, targetRot.eulerAngles.z),
                jumpDuration, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear);
            }
        }
    }
}