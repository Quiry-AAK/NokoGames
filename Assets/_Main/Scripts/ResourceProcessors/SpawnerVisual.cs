using System;
using DG.Tweening;
using UnityEngine;

namespace Scripts.ResourceProcessors
{
    public class SpawnerVisual : ResourceProcessorVisual
    {
        [SerializeField] private Animator spawnerAnimator;
        [SerializeField] private Transform spawnedObjectVisual;
        [SerializeField] private Vector3 initialPos, endPos;
        [SerializeField] private ParticleSystem[] particles;
        [Header("Gizmo")]
        [SerializeField] private bool drawGizmos;
        [SerializeField] private Color gizmosColor;

        private Tween moveTween;

        public Vector3 EndPos => endPos;

        public override void StartVisualizingSpawning(float spawnTime)
        {
            moveTween?.Kill();

            spawnedObjectVisual.transform.localPosition = initialPos;
            moveTween = spawnedObjectVisual.DOLocalMove(endPos, spawnTime).SetEase(Ease.Linear);

            spawnerAnimator.SetBool("Processing", true);

            foreach (var particle in particles)
            {
                if (particle.isPlaying)
                    continue;
                particle.Play();
            }

        }

        public override void StopVisualizingSpawning()
        {
            spawnedObjectVisual.transform.localPosition = initialPos;
            spawnerAnimator.SetBool("Processing", false);

            foreach (var particle in particles)
            {
                particle.Stop();
            }
        }

        public void DrawPositions(Transform parentTr)
        {
            if (!drawGizmos) return;
            Gizmos.color = gizmosColor;
            Gizmos.DrawWireSphere(parentTr.TransformPoint(initialPos), .2f);
            Gizmos.DrawWireSphere(parentTr.TransformPoint(endPos), .2f);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            DrawPositions(transform);
        }
#endif
    }
}