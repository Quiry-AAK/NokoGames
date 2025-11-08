using DG.Tweening;
using UnityEngine;

namespace Scripts.Tail
{
    public class TailItem : MonoBehaviour
    {
        [SerializeField] private Quaternion targetRot;
        [SerializeField] private int rotationCount;
        [SerializeField] private float jumpDuration, jumpPower;

        public void StartPlacing(Transform tailNode)
        {
            transform.SetParent(tailNode);
            transform.DOLocalJump(Vector3.zero, jumpPower, 1, jumpDuration).SetEase(Ease.OutQuad);

            var startRotation = transform.eulerAngles;

            var totalRotation = 360f * rotationCount;

            var rotationDelta = targetRot.eulerAngles - startRotation;

            if (rotationDelta.y < 0)
            {
                rotationDelta.y += 360f;
            }

            transform.DOLocalRotate(new Vector3(0, startRotation.y + totalRotation + rotationDelta.y, 0), jumpDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear);
        }
    }
}